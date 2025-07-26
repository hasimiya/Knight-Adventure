using System;
using UnityEngine;

public class SkeletonVisual : MonoBehaviour
{
    // Variables ScriptableObject
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;

    // Variables Components
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Variable CONST
    private const string AnimatorAttakTrigger = "Attack";
    private const string TakeHitTrigger = "TakeHit";

    private const string IsDieBool = "IsDie";
    private const string IsRunningBool = "IsRunning";

    private const string ChasingSpeedMultiplierFloat = "ChasingSpeedMultiplier";

    // Variables Hash
    private static readonly int AnimatorAttakTriggerHash = Animator.StringToHash(AnimatorAttakTrigger);
    private static readonly int TakeHitTriggerHash = Animator.StringToHash(TakeHitTrigger);
    private static readonly int IsDieBoolHash = Animator.StringToHash(IsDieBool);
    private static readonly int IsRunningBoolHash = Animator.StringToHash(IsRunningBool);
    private static readonly int ChasingSpeedMultiplierFloatHash = Animator.StringToHash(ChasingSpeedMultiplierFloat);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        enemyAI.OnEnemyAttack += EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += EnemyEntity_OnTakeHit;
        enemyEntity.OnEnemyDeath += EnemyEntity_OnEnemyDeath;
    }
    private void Update()
    {
        _animator.SetBool(IsRunningBoolHash, enemyAI.IsRunning);
        _animator.SetFloat(ChasingSpeedMultiplierFloatHash, enemyAI.GetRoamingAnimationSpeed());
    }
    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= EnemyEntity_OnTakeHit;
        enemyEntity.OnEnemyDeath -= EnemyEntity_OnEnemyDeath;
    }

    // Public Methods
    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }
    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }

    // Event Methods
    private void EnemyEntity_OnTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TakeHitTriggerHash);
    }

    private void EnemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(AnimatorAttakTriggerHash);
        Debug.Log($"{gameObject.name} attack!");
    }
    private void EnemyEntity_OnEnemyDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IsDieBoolHash, true);
        _spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }
}
