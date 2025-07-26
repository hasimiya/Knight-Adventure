using System;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private SkeletonVisual skeletonVisual;
    [SerializeField] private EnemyAI enemyAI;

    // Variables Components
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2d;

    // Variables Health    
    private float _currentHealth;

    // Variables Event
    public event EventHandler OnTakeHit;
    public event EventHandler OnEnemyDeath;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        _currentHealth = enemySO.enemyHealth;
        PolygonColliderTurnOff();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, enemySO.enemyDamageAmount);
        }
    }

    // Public Methods
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();

        Debug.Log($"{gameObject.name}: {_currentHealth}");
    }
    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    // Private Methods
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _polygonCollider2D.enabled = false;
            _boxCollider2d.enabled = false;

            enemyAI.SetDeathState();
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
            Debug.Log($"{gameObject.name} destroy!");
        }
    }
}