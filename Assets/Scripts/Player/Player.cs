using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    // Variables ScriptableObject
    [SerializeField] private TrailRenderer playerTrail;

    // Variables Settings
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float maxHealth = 10;

    private readonly float _minMovingSpeed = 0.1f;
    private readonly float _damageRecoveryTime = 0.5f;

    private float _currentHealth;
    private float _normalSpeed;
    private float _dashSpeed = 4f;
    private float _dashDuration = 0.2f;

    // Variables objects
    private Camera _mainCamera;

    // Variables Components
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    private KnockBack _knockBack;



    // Variables Bool
    public bool IsAlive { get; private set; } = true;
    private bool _isRunning = false;
    private bool _canTakeDamage;
    private bool _canDash = true;

    // Variables Input
    private Vector2 _inputVector;

    // Variables Events
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerTakeHit;
    public event EventHandler OnPlayerFlashBlink;

    private void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;

        _rb = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _knockBack = GetComponent<KnockBack>();
    }
    void Start()
    {
        _canTakeDamage = true;
        _currentHealth = maxHealth;
        _normalSpeed = movingSpeed;

        GameInput.Instance.OnPlayerAttak += GameInput_OnPlayerAttak;
        GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
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
    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttak -= GameInput_OnPlayerAttak;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectible>(out ICollectible collectible))
        {
            collectible.Collect();
        }
    }

    // Public Methods
    public bool IsRunning() => _isRunning;
    public Vector3 GetPlayerScreenPosition() => _mainCamera.WorldToScreenPoint(transform.position);
    // это метод для получения позиции игрока в мировых координатах
    public void TakeDamage(Transform damageSource, float damage)
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage); // 
            _knockBack.GetKnockedBack(damageSource);
            StartCoroutine(ResetCanTakeDamage());

            OnPlayerTakeHit?.Invoke(this, EventArgs.Empty);
            //OnPlayerFlashBlink?.Invoke(this, EventArgs.Empty);
            Debug.Log(_currentHealth);
            DetectDeath();
        }
    }
    public bool IsPlayerAlive() => IsAlive;
    public void Heal(float healAmount)
    {
        if (_currentHealth < maxHealth)
        {
            _currentHealth = Mathf.Min(maxHealth, _currentHealth + healAmount);
            Debug.Log($"Player healed: {_currentHealth}");
        }
    }

    // Private Methods
    private void HandleMovement()
    {
        Vector2 newPosition = _rb.position + (_inputVector * movingSpeed * Time.deltaTime);

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

            DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            Debug.Log("Player is dead!");
        }
    }
    private void DisableMovement()
    {
        GameInput.Instance.DisableMovement();
    }
    private void Dash()
    {
        _canDash = false;
        movingSpeed *= _dashSpeed;
        playerTrail.emitting = true;
        StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(_dashDuration);
        movingSpeed = _normalSpeed;
        _canDash = true;
        playerTrail.emitting = false;
    }

    // Event Methods
    private void GameInput_OnPlayerAttak(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attak();
    }
    private void GameInput_OnPlayerDash(object sender, EventArgs e)
    {
        if (_canDash && _isRunning)
        {
            Dash();
            DebugLogUsedDash();
        }
    }
    private void DebugLogUsedDash()
    {
        Debug.Log("Player used dash!");
        Debug.Log($"IsDashing: {_canDash}");
        Debug.Log($"Player dash speed: {movingSpeed}");
    }
}
