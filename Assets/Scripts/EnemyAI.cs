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
        Roaming
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false; // ��������� ������� ������
        navMeshAgent.updateUpAxis = false; // ��������� ���������� ��� Y ������
        state = startingState;
    }
    private void Update()
    {
        switch (state)
        {
            default:
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
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
        ChangeFacingDirection(startingPosition, roamingPosition);
        navMeshAgent.SetDestination(roamingPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * Random.Range(roamingDistanseMin, roamingDistanseMax);
    }
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
