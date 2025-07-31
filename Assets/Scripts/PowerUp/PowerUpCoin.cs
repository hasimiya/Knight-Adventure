using UnityEngine;

public class PowerUpCoin : MonoBehaviour, ICollectible
{
    [SerializeField] private PowerUpSO powerUpSO;
    public void Collect()
    {
        Player.Instance.UpdateScore(powerUpSO.Value);
        Destroy(gameObject);
    }
}
