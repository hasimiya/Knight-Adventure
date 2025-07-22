using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    // Variables Components
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    private KnockBack _knockBack;

    // Variables Settings
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxHealth = 10;

    private float _minMovingSpeed = 0.1f;
    private float _damageRecoveryTime = 0.5f;

    private float _currentHealth;

    private bool _isRunning = false;
    private bool _canTakeDamage;

    // Variables Bool
    public bool IsAlive { get; private set; } = true;

    // Variables Input
    private Vector2 _inputVector;

    // Variables Events
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerTakeHit;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _knockBack = GetComponent<KnockBack>();
    }
    void Start()
    {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        GameInput.Instance.OnPlayerAttak += GameInput_OnPlayerAttak;
    }
    void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack)
            return;
        HandleMovement();
    }

    // Public Methods
    public bool IsRunning() => _isRunning;
    public Vector3 GetPlayerScreenPosition() => Camera.main.WorldToScreenPoint(transform.position);
    // это метод для получения позиции игрока в мировых координатах
    public void TakeDamage(Transform damageSource, float damage)
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockedBack(damageSource);
            StartCoroutine(ResetCanTakeDamage());
            Debug.Log(_currentHealth);
            OnPlayerTakeHit?.Invoke(this, EventArgs.Empty);
            DetectDeath();
        }
    }

    // Private Methods
    private void GameInput_OnPlayerAttak(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attak();
    }

    private void HandleMovement()
    {
        Vector2 newPosition = _rb.position + _inputVector * _speed * Time.deltaTime;

        _rb.MovePosition(newPosition);
        if (Mathf.Abs(_inputVector.x) > _minMovingSpeed || Mathf.Abs(_inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }
    private IEnumerator ResetCanTakeDamage()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            IsAlive = false;
            _capsuleCollider.enabled = false;
            _boxCollider.enabled = false;
            _rb.velocity = Vector2.zero;

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            Debug.Log("Player is dead!");
        }
    }
}
