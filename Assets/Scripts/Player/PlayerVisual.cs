using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private GameObject playerShadow;

    // Variables Components
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private FlashBlink _flashBlink;

    // Variables CONST
    private const string TakeHitTrigger = "TakeHit";

    private const string IsRunningBool = "IsRunning";
    private const string IsDieBool = "IsDie";

    // Variebles Hash
    private static readonly int TakeHitTriggerHash = Animator.StringToHash(TakeHitTrigger);
    private static readonly int IsRunningBoolHash = Animator.StringToHash(IsRunningBool);
    private static readonly int IsDieBoolHash = Animator.StringToHash(IsDieBool);
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
        _animator.SetBool(IsRunningBoolHash, Player.Instance.IsRunning());
        if (Player.Instance.IsPlayerAlive())
        {
            AdjustPlayerFacingDirection();
        }
    }
    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
        Player.Instance.OnPlayerTakeHit -= Player_OnPlayerTakeHit;
    }
    // Private Methods
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        _spriteRenderer.flipX = mousePosition.x < playerPosition.x;
    }

    // Event Methods
    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IsDieBoolHash, true);
        playerShadow.SetActive(false);
        _flashBlink.StopBliking();
    }
    private void Player_OnPlayerTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TakeHitTriggerHash);
    }
}
