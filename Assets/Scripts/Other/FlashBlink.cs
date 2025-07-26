using System;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    // / Variables ScriptableObject
    [SerializeField] private MonoBehaviour damageObject;
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.2f;

    // Variables Components
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;

    // Variables
    private float _blinkTimer;
    private bool IsBliking = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }
    private void Start()
    {
        if (damageObject is Player player)
        {
            player.OnPlayerFlashBlink += Player_OnPlayerFlashBlink;
        }
    }

    private void Update()
    {
        if (IsBliking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }
    }
    private void OnDestroy()
    {
        if (damageObject is Player player)
        {
            player.OnPlayerFlashBlink -= Player_OnPlayerFlashBlink;
        }
    }

    // Public methods
    public void StopBliking()
    {
        SetDefaultMaterial();
        IsBliking = false;
    }

    // Private methods
    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }
    private void SetBlikingMaterial()
    {
        _blinkTimer = blinkDuration;
        _spriteRenderer.material = blinkMaterial;
    }

    // Event methods
    private void Player_OnPlayerFlashBlink(object sender, EventArgs e)
    {
        SetBlikingMaterial();
    }
}
