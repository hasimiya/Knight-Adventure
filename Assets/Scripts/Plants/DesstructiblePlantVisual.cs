using System;
using UnityEngine;

public class DesstructiblePlantVisual : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private DesstructiblePlant _desstructiblePlant;
    [SerializeField] private GameObject _bushDeathVFXPrefab;
    private void Start()
    {
        if (_desstructiblePlant != null)
        {
            _desstructiblePlant.OnDestructibleTakeDamage += OnDestructibleTakeDamage;
        }
        else
        {
            Debug.LogWarning($"{name}: _desstructiblePlant is not assigned!");
        }
    }
    private void OnDestroy()
    {
        if (_desstructiblePlant != null)
        {
            _desstructiblePlant.OnDestructibleTakeDamage -= OnDestructibleTakeDamage;
        }
    }

    // Private Methods
    private void ShowDeathVFX()
    {
        Instantiate(_bushDeathVFXPrefab, _desstructiblePlant.transform.position, Quaternion.identity);
    }

    // Event Handlers
    private void OnDestructibleTakeDamage(object sender, EventArgs e)
    {
        ShowDeathVFX();
    }
}
