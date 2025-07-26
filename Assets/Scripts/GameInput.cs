using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Singleton Instance
    public static GameInput Instance { get; private set; }

    // Variables Components
    private PlayerInputAction _playerInputActions;

    // Variables Events
    public event EventHandler OnPlayerAttak;
    public event EventHandler OnPlayerDash;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _playerInputActions = new PlayerInputAction();
        _playerInputActions.Enable();
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Player.Dash.performed += PlayerDash_performed;
    }

    // Public Methods
    public Vector2 GetMovementVector() => _playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    public Vector2 GetMousePosition() => Mouse.current.position.ReadValue();
    // получаем позицию мыши в мировых координатах
    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }

    // Event Methods
    private void PlayerAttack_started(InputAction.CallbackContext context)
    {
        OnPlayerAttak?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerDash_performed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }
}
