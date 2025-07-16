using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisual : MonoBehaviour
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

    private void Sword_OnSwordSwing(object sender, EventArgs e)
    {
        animator?.SetTrigger(ATTAK_TRIGGER);
    }
}
