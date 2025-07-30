using UnityEngine;

public class PowerUpHealth : MonoBehaviour, ICollectible
{
    // Variables ScriptableObject
    [SerializeField] private PowerUpSO powerUpSO;


    // Interface Metod
    public void Collect()
    {
        Player.Instance.Heal(powerUpSO.HealthBoost);
        Destroy(gameObject);
    }
}
