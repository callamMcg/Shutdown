using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseFloatRobot : BaseRobot
{
    //navigation control
    NavMeshAgent agent;
    Vector3 location;

    //bot variables
    float speed = 0;
    float attackCharge = 0;

    /*Update
     * 1- if there is no navAgent, set the nav agent
     * 2- change the agents speed depending on the botSpeed
     * 3- call behaviour functions depending on the botState
     * 4- set the destination of the nav aganet to the location of interest
     */
    void Update()
    {
        //1
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        //2
        switch (botSpeed)
        {
            case BotSpeed.still:
                speed = 0;
                orb.movementSpeed = Mathf.Lerp(orb.movementSpeed, 2, 0.5f);
                break;
            case BotSpeed.slow:
                speed = 1.5f;
                orb.movementSpeed = Mathf.Lerp(orb.movementSpeed, 2.5f, 0.5f);
                break;
            case BotSpeed.medium:
                speed = 3.5f;
                orb.movementSpeed = Mathf.Lerp(orb.movementSpeed, 5, 1);
                break;
            case BotSpeed.fast:
                speed = 5.5f;
                orb.movementSpeed = Mathf.Lerp(orb.movementSpeed, 8, 1);
                break;
            default:
                Debug.Log("Unknown speed");
                botSpeed = BotSpeed.medium;
                break;
        }
        agent.speed = speed;

        //3
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

        //4
        if (location != null)
            agent.SetDestination(location);
    }

    //Behavioural Functions
    /*Patrol
     * 1- set speed to medium
     * 2- if no patrol points = location 
     * 3- set closest patrol point to location
     * 4- if within 5 of location
     * 5- set location to next patrol point
     */
    private void Patrol()
    {
        botSpeed = BotSpeed.medium;

    }

    /* Chase
     * 1- if the player is seen set awarness to 100
     *  2- if within attack range
     *   a- stop moving
     *   b- charge attack
     *   c- if attack charged > 3, set attack ready to true and reset charge
     *  3- otherwise
     *   a- set speed to fast
     *   b- lose attack charge
     * 4- if the player can't be seen, but can be heard
     *  a- set speed to slow
     *  b- set location to the player
     * 5- if the player can't be seen or heard, reduce awarness
     */
    private void Chase()
    {
        //1
        if (See() != Vector3.zero)
        {
            Vector3 player = See();
            location = player;
            awareness = 100;
            //2
            if (See(10, 30) == player)
            {
                //a
                botSpeed = BotSpeed.still;

                transform.LookAt(player);
                //b
                attackCharge += Time.deltaTime;
                //c
                if (attackCharge > 3)
                {
                    attackCharge = 0;
                    attackReady = true;
                }
            }
            //3
            else
            {
                //a
                botSpeed = BotSpeed.fast;
                //b
                attackCharge -= Time.deltaTime;
            }
        }
        //4
        else if (Hear() != Vector3.zero)
        {
            //a
            botSpeed = BotSpeed.slow;
            //b
            Vector3 player = Hear();
            location = player;
        }
        //5
        else
            awareness -= 50 * Time.deltaTime;
    }

    /*Search
     * 1- if location is within 5,
     * 2- get a new random vector greater than 5 but smaller than 15
     * 3- make the vector relative to the robot
     * 4- make the vector a reachable point
     * 5- increase the search index
     * 6- set the new location
     */
    private void Search()
    {
        botSpeed = BotSpeed.slow;

        //1
        location = transform.position;
        float distance = (location - transform.position).magnitude;
        if(distance < 1)
        {
            //2
            float r = 15;
            float tempDist = 0;
            Vector3 randomDirection = Vector3.zero;
            do
            {
                randomDirection = Random.insideUnitSphere * r;
                tempDist = randomDirection.magnitude;
            } while (tempDist < 6);

            //3
            randomDirection += transform.position;
            
            //4
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, r, 1);
            
            //5
            searchIndex++;
            
            //6
            location = hit.position;
        }
    }

    private void Attack()
    {
        botSpeed = BotSpeed.slow;

        Debug.Log("attack");
        attackReady = false;
    }
}
