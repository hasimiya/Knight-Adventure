using UnityEngine;

public class SelfDestroyVFX : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (_particleSystem && !_particleSystem.IsAlive())
        {
            DestroySelf();
        }
    }

    // Private Methods
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
