using System;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    [SerializeField] private Sword _sword;
    private Animator _animator;
    private const string ATTAK_TRIGGER = "Attack";
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void Sword_OnSwordSwing(object sender, EventArgs e)
    {
        _animator?.SetTrigger(ATTAK_TRIGGER);
    }
    public void TriggerEndAttackAnimation()
    {
        _sword.AttackColliderTurnOff();
    }
}
