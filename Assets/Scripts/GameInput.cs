using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputAction playerInputActions;
    public event EventHandler OnPlayerAttak;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Enable();
        playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }

    private void PlayerAttack_started(InputAction.CallbackContext context)
    {
        OnPlayerAttak?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector() => playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    public Vector3 GetMousePosition() => Mouse.current.position.ReadValue();
    // получаем позицию мыши в мировых координатах
}
