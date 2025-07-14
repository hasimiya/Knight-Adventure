using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanseMax = 7f;
    [SerializeField] private float roamingDistanseMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaming
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false; // отключаем поворот агента
        navMeshAgent.updateUpAxis = false; // отключаем обновление оси Y агента
        state = startingState;
    }
    private void Start()
    {
        startingPosition = transform.position;
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                break;
        }
    }
    private void Roaming()
    {
        roamingPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamingPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * Random.Range(roamingDistanseMin, roamingTimerMax);
    }
}
