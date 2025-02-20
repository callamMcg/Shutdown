using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRobot : MonoBehaviour
{

    //player object
    private Transform playerTransform;

    public bool shutdown = false;
    public bool attackReady = false;
    public float searchIndex;
    public float searchLimit = 5;
    public float awareness = 0;

    //state machine
    public enum BotSM
    {
        patrol, chase, attack, search, die
    }
    public BotSM botState;

    /*
     * 1 - make a vector that represents the space between the player and the bot
     * 2 - if the magnitude of the vector
     * 3 - and if the angle between the bots forward and the vector is smaller than the vision angle
     * 4 - and if a ray cast in the direction of the vector would hit a gameObject with the Player tag
     * 5 - return the players position
     * 6 - otherwise return a zero Vector3
     */
    public Vector3 See(float range = 60, float angle = 30)
    {
        //1
        Vector3 toPlayer = playerTransform.position - transform.position;
        toPlayer.y = 0;
        float delatAngle = Vector3.Angle(transform.forward, toPlayer);
        RaycastHit hit;
        //2
        if (toPlayer.magnitude < range)
            //3
            if (delatAngle < angle && delatAngle > -angle)
                //4
                if (Physics.Raycast(transform.position + Vector3.up, toPlayer, out hit, Mathf.Infinity))
                    if (hit.collider.CompareTag("Player"))
                        //5
                        return playerTransform.position;
        //6
        return Vector3.zero;
    }

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        botState = BotSM.patrol;
    }

    //State Transitions
    /* patrol
     * 1 - if the player is seen, chase
     * 2 - if shutdown is true, die
     */
    public async void PatrolState()
    {
        //1
        if (See() != Vector3.zero)
        {
            botState = BotSM.chase;
            awareness = 100;
        }
        //2
        if (shutdown)
            botState = BotSM.die;
    }
    /* chase
     * 1 - if the player is not seen, search
     * 2 - if attackReady is true, attack
     * 3 - if shutdown is true, die
     */
    public void ChaseState()
    {
        //1
        if (See() == Vector3.zero)
            awareness -= 50 * Time.deltaTime;
        if(awareness < 0)
            botState = BotSM.search;
        //2
        if (attackReady)
            botState = BotSM.attack;
        //3
        if (shutdown) 
            botState = BotSM.die;
    }
    /* attack
     * 1 - if shutdown is true, die
     * 2 - else, search
     */
    public void AttackState()
    {
        //1
        if (shutdown)
            botState = BotSM.die;
        //2
        else
            botState = BotSM.search;
    }
    /* search
     * 1 - if the player is seen, chase
     * 2 - if search index > search limit, patrol
     * 3 - if shutdownis true, die
     */
    public void SearchState()
    {
        //1
        if (See() != Vector3.zero)
        {
            searchIndex = 0;
            awareness = 100;
            botState = BotSM.chase;
        }
        //2
        if (searchIndex >= searchLimit)
        {
            searchIndex = 0;
            botState = BotSM.patrol;
        }
        //3
        if (shutdown)
            botState = BotSM.die;
    }
    public void DieState()
    {
        Invoke("Die", 0.6f);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
