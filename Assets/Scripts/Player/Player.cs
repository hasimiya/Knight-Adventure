using System;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float _speed = 5f;
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private Rigidbody2D _rb;
    private Vector2 _inputVector;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Instance = this;
        GameInput.Instance.OnPlayerAttak += GameInput_OnPlayerAttak;
    }

    private void GameInput_OnPlayerAttak(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attak();
    }

    void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
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
    public bool IsRunning() => _isRunning;
    public Vector3 GetPlayerScreenPosition() => Camera.main.WorldToScreenPoint(transform.position);
    // это метод для получения позиции игрока в мировых координатах
}
