using System;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damageObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool IsBliking = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }
    private void Start()
    {
        if (_damageObject is Player)
        {
            (_damageObject as Player).OnPlayerFlashBlink += Player_OnPlayerFlashBlink;
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
        if (_damageObject is Player)
        {
            (_damageObject as Player).OnPlayerFlashBlink -= Player_OnPlayerFlashBlink;
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
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    // Event methods
    private void Player_OnPlayerFlashBlink(object sender, EventArgs e)
    {
        SetBlikingMaterial();
    }
}
