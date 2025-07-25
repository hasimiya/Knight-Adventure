using System;
using UnityEngine;

public class SwordSlashVisual : MonoBehaviour
{
    [SerializeField] private Sword sword;
    private Animator animator;
    private const string ATTAK_TRIGGER = "Attack";
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
        animator?.SetTrigger(ATTAK_TRIGGER);
    }
}
