using System;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float speed = 5f;
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private Rigidbody2D rb;
    Vector2 inputVector;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Instance = this;
        GameInput.Instance.OnPlayerAttak += GameInput_OnPlayerAttak;
    }

    private void GameInput_OnPlayerAttak(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attak();
    }

    void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
    {
        Vector2 newPosition = rb.position + inputVector * speed * Time.deltaTime;

        rb.MovePosition(newPosition);
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    public bool IsRunning() => isRunning;
    public Vector3 GetPlayerScreenPosition() => Camera.main.WorldToScreenPoint(transform.position);
    // это метод для получения позиции игрока в мировых координатах
}
