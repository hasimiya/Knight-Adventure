using System;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private Sword sword;

    // Variables Components
    private Animator animator;

    // Variables CONST
    private const string AttakTrigger = "Attack";

    // Variables Hash
    private static readonly int AttakTriggerHash = Animator.StringToHash(AttakTrigger);
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }
    private void OnDestroy()
    {
        sword.OnSwordSwing -= Sword_OnSwordSwing;
    }

    // Private Methods
    private void Sword_OnSwordSwing(object sender, EventArgs e)
    {
        animator?.SetTrigger(AttakTriggerHash);
    }
}
