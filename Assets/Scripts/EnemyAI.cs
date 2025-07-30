using KnightAdventure.Utils;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Variables Components
    private NavMeshAgent _navMeshAgent;

    // Variables Idle
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanseMax = 7f;
    [SerializeField] private float roamingDistanseMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    // Variables Chasing
    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeedMultiplier = 2f;

    // Variables Attacking
    [SerializeField] private bool isAttackingEnemy = false;
    private float _attackingDistance = 2f;
    private float _attackRate = 2f;
    private float _nextAttackTime = 0f;

    private State _currentState;

    // Variables Roaming
    private float _roamingTimer;
    private Vector3 _roamingPosition;
    private Vector3 _startingPosition;
    private float _roamingSpeed;
    private float _chasingSpeed;
    public bool IsRunning => _navMeshAgent.velocity != Vector3.zero;

    // Enum State
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    // Variables ???
    private float _nextCheckDirectionTime = 0f;
    private float _chekDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    // Variables Events
    public event EventHandler OnEnemyAttack;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false; // отключаем поворот агента
        _navMeshAgent.updateUpAxis = false; // отключаем обновление оси Y агента
        _currentState = startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _roamingSpeed * chasingSpeedMultiplier;
    }
    private void Update()
    {
        StateHundler();
        MovementDirectionHandler();
    }

    // Public Methods
    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }
    public void CheckCurrentState()
    {
        State newState;

        if (!Player.Instance.IsPlayerAlive())
        {
            newState = State.Roaming;
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

            if (isAttackingEnemy && distanceToPlayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
            else if (isChasingEnemy && distanceToPlayer <= chasingDistance)
            {
                newState = State.Chasing;
            }
            else
            {
                newState = State.Roaming;
            }
        }

        if (newState != _currentState)
        {
            _currentState = newState;
            switch (newState)
            {
                case State.Chasing:
                    _navMeshAgent.ResetPath();
                    _navMeshAgent.speed = _chasingSpeed;
                    break;
                case State.Roaming:
                    _navMeshAgent.ResetPath();
                    _navMeshAgent.speed = _roamingSpeed;
                    _roamingTimer = 0f;
                    break;
                case State.Attacking:
                    _navMeshAgent.ResetPath();
                    break;
            }
        }

        //State newState = State.Roaming;

        //if (Player.Instance.IsPlayerAlive())
        //{
        //    if (isChasingEnemy)
        //    {
        //        if (distanceToPlayer <= chasingDistance)
        //        {
        //            newState = State.Chasing;
        //        }
        //    }
        //    if (isAttackingEnemy)
        //    {
        //        if (distanceToPlayer <= _attackingDistance)
        //        {
        //            newState = State.Attacking;
        //        }
        //    }
        //}        

        //if (newState != _currentState)
        //{
        //    _currentState = newState;
        //    switch (newState)
        //    {
        //        case State.Chasing:
        //            _navMeshAgent.ResetPath();
        //            _navMeshAgent.speed = _chasingSpeed;
        //            break;
        //        case State.Roaming:
        //            _navMeshAgent.ResetPath();
        //            _navMeshAgent.speed = _roamingSpeed;
        //            _roamingTimer = 0f;
        //            break;
        //        case State.Attacking:
        //            _navMeshAgent.ResetPath();
        //            break;
        //    }
        //}
    }
    public void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }
    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }


    // Private Methods
    private void StateHundler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamingPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanseMin, roamingDistanseMax);
    }
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        transform.rotation = sourcePosition.x > targetPosition.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }
    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
                _nextCheckDirectionTime = Time.time + _chekDirectionDuration;
            }
        }
        else if (_currentState == State.Attacking)
        {
            ChangeFacingDirection(transform.position, Player.Instance.transform.position);
        }
        _lastPosition = transform.position;
        //_nextCheckDirectionTime = Time.time + _chekDirectionDuration;
    }
    // Attack
    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }
}