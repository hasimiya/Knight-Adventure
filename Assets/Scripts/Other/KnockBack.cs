using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private float knockBackForce = 3f;
    [SerializeField] private float knockBackMovingTimerMax = 0.3f;

    // Variables Components
    private Rigidbody2D _rb;

    // Variables Timer
    private float _knockBackMovingTimer;

    // Variables Bool
    public bool IsGettingKnockedBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
        {
            StopKnockBackMovement();
        }
    }

    // Public Methods
    public void StopKnockBackMovement()
    {
        _rb.velocity = Vector2.zero;
        IsGettingKnockedBack = false;
    }
    public void GetKnockedBack(Transform damageSource)
    {
        IsGettingKnockedBack = true;
        _knockBackMovingTimer = knockBackMovingTimerMax;
        Vector2 differens = (transform.position - damageSource.position).normalized * knockBackForce / _rb.mass;
        _rb.AddForce(differens, ForceMode2D.Impulse);
    }
}
