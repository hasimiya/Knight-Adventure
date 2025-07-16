using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwordSwing;
    public void Attak()
    {
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }
}
