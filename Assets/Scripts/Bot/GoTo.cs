using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoTo : MonoBehaviour
{
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Location(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
}
