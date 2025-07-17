using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        DetectDeath();
        Debug.Log($"{gameObject.name}: {_currentHealth}");
    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log($"{gameObject.name} destroy!");
        }
    }
}
