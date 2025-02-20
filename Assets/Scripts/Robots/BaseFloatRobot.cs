using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseFloatRobot : BaseRobot
{
    //navigation control
    NavMeshAgent agent;
    Vector3 location;

    void Update()
    {

        switch (botState)
        {
            case BotSM.patrol:
                PatrolState();
                Patrol();
                break;
            case BotSM.chase:
                Chase();
                ChaseState();
                break;
            case BotSM.attack:
                Attack();
                AttackState();
                break;
            case BotSM.search:
                Search();
                SearchState();
                break;
            case BotSM.die:
                DieState();
                break;
            default:
                Debug.Log("Unknown state");
                botState = BotSM.patrol;
                break;
        }

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (location != null)
            agent.SetDestination(location);
    }

    private void Patrol()
    {
        //Debug.Log("p");s

    }
    private void Chase()
    {
        if(See() != Vector3.zero)
        {
            awareness = 100;
            location = See();
        }
    }
    private void Search()
    {

        float distance = (location - transform.position).magnitude;
        if(distance < 5)
        {
            float r = 15;
            Vector3 randomDirection = Random.insideUnitSphere * r;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, r, 1);
            
            searchIndex++;
            Debug.Log(location);
            location = hit.position;
        }
    }
    private void RandomReachable()
    {
        float r = 15;
        Vector3 randomDirection = Random.insideUnitSphere * r;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, r, 1);
    }

    private void Attack()
    {
        Debug.Log("attack");
        attackReady = true;
    }
}
