using KnightAdventure.Utils;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Variables Components
    private NavMeshAgent _navMeshAgent;

    // Variables Idle
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanseMax = 7f;
    [SerializeField] private float _roamingDistanseMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    // Variables Chasing
    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    // Variables Attacking
    [SerializeField] private bool _isAttackingEnemy = false;
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
    public bool IsRunning
    {
        get
        {
            if (_navMeshAgent.velocity == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    // Enum State
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    // Variables Events
    public event EventHandler OnEnemyAttack;

    // Variables ???
    private float _nextCheckDirectionTime = 0f;
    private float _chekDirectionDuration = 0.1f;
    private Vector3 _lastPosition;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false; // отключаем поворот агента
        _navMeshAgent.updateUpAxis = false; // отключаем обновление оси Y агента
        _currentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _roamingSpeed * _chasingSpeedMultiplier;
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
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy && Player.Instance.IsAlive == true)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }
        if (_isAttackingEnemy)
        {
            if (distanceToPlayer <= _attackingDistance && Player.Instance.IsAlive == true)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _currentState)
        {
            _currentState = newState;
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _roamingSpeed;
                _roamingTimer = 0f;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }
            else if (newState == State.Death)
            {

            }
        }
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
                    _roamingTimer = _roamingTimerMax;
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
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanseMin, _roamingDistanseMax);
    }
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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
        if (Player.Instance.IsAlive)
        {
            if (Time.time > _nextAttackTime)
            {
                OnEnemyAttack?.Invoke(this, EventArgs.Empty);
                _nextAttackTime = Time.time + _attackRate;
            }
        }
    }
}
