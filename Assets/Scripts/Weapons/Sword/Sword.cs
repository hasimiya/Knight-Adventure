using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private float damageAmount = 2;

    // Variables Components
    private PolygonCollider2D _polygonCollider2D;

    // Variables Events
    public event EventHandler OnSwordSwing;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    private void Start()
    {
        AttackColliderTurnOff();
    }
    public void Attak()
    {
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
        AttackColliderTurnOffOn();
    }
    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
    private void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }
    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(damageAmount);
        }
    }
}
