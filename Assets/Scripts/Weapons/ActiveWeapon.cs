using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    // Singleton Instance
    public static ActiveWeapon Instance { get; private set; }

    // Variables ScriptableObject
    [SerializeField] private Sword sword;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Player.Instance.IsPlayerAlive())
        {
            FollowMousePosition();
        }
    }
    public Sword GetActiveWeapon()
    {
        return sword;
    }
    private void FollowMousePosition()
    {
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        transform.rotation = mousePosition.x < playerPosition.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
