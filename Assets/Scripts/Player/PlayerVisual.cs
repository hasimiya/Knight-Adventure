using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    // Variables Components
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _playerShadow;
    private FlashBlink _flashBlink;

    // Variables CONST
    private const string TAKEHIT_TRIGGER = "TakeHit";

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE_BOOL = "IsDie";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnPlayerTakeHit += Player_OnPlayerTakeHit;
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        if (Player.Instance.IsPlayerAlive())
        {
            AdjustPlayerFacingDirection();
        }
    }

    // Private Methods
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();
        if (mousePosition.x < playerPosition.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    // Event Methods
    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IS_DIE_BOOL, true);
        _playerShadow.SetActive(false);
        _flashBlink.StopBliking();
    }
    private void Player_OnPlayerTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TAKEHIT_TRIGGER);
    }
}
