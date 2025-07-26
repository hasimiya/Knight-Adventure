using System;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private Sword _sword;

    // Variables Components
    private Animator _animator;

    // Variables CONST
    private const string AttakTrigger = "Attack";

    // Variables Hash
    private static readonly int AttakTriggerHash = Animator.StringToHash(AttakTrigger);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _sword.OnSwordSwing += Sword_OnSwordSwing;
    }
    private void OnDestroy()
    {
        _sword.OnSwordSwing -= Sword_OnSwordSwing;
    }

    // Public Methods
    public void TriggerEndAttackAnimation()
    {
        _sword.AttackColliderTurnOff();
    }

    // Private Methods
    private void Sword_OnSwordSwing(object sender, EventArgs e)
    {
        _animator?.SetTrigger(AttakTriggerHash);
    }
}
