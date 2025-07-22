using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // Variables Components
    private Rigidbody2D _rb;

    // Variables 
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackMovingTimerMax = 0.3f;
    private float _knockBackMovingTimer;
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

    public void StopKnockBackMovement()
    {
        _rb.velocity = Vector2.zero;
        IsGettingKnockedBack = false;
    }
    public void GetKnockedBack(Transform damageSource)
    {
        IsGettingKnockedBack = true;
        _knockBackMovingTimer = _knockBackMovingTimerMax;
        Vector2 differens = (transform.position - damageSource.position).normalized * _knockBackForce / _rb.mass;
        _rb.AddForce(differens, ForceMode2D.Impulse);
    }
}
