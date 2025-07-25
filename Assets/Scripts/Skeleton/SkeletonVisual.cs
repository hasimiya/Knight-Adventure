using System;
using UnityEngine;

public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;

    // Variables Components
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Variable CONST and Bool
    private const string ANIMATOR_ATTACK_TRIGGER = "Attack";
    private const string TAKEHIT_TRIGGER = "TakeHit";
    private const string IS_DIE_BOOL = "IsDie";
    private const string IS_RUNNING_BOOL = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER_FLOAT = "ChasingSpeedMultiplier";

    // Variables Attack
    //private bool _isAttackEnemy = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        _enemyAI.OnEnemyAttack += EnemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit += EnemyEntity_OnTakeHit;
        _enemyEntity.OnEnemyDeath += EnemyEntity_OnEnemyDeath;
    }
    private void Update()
    {
        _animator.SetBool(IS_RUNNING_BOOL, _enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER_FLOAT, _enemyAI.GetRoamingAnimationSpeed());
    }
    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= EnemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit -= EnemyEntity_OnTakeHit;
        _enemyEntity.OnEnemyDeath -= EnemyEntity_OnEnemyDeath;
    }

    // Public Methods
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }
    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    // Event Methods
    private void EnemyEntity_OnTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TAKEHIT_TRIGGER);
    }

    private void EnemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(ANIMATOR_ATTACK_TRIGGER);
        Debug.Log($"{gameObject.name} attack!");
    }
    private void EnemyEntity_OnEnemyDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IS_DIE_BOOL, true);
        _spriteRenderer.sortingOrder = -1;
        _enemyShadow.SetActive(false);
    }
}
