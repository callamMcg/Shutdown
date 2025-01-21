using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotV3Brain : MonoBehaviour
{

    //---DECLORATIONS---

    //navigation control
    NavMeshAgent agent;
    Vector3 location;

    //orb control
    FloatingOrb orb;

    //state machine
    public enum BotSM
    {
        patrol, chase, shoot, search, die
    }
    public BotSM botState;

    //patrol variables
    public Transform[] patrol;
    int point = -1;
    float successRadius = 5;

    //shooting variables
    bool shot = false;
    public GameObject explosion;
    Vector3 target;

    //detection variables
    public Transform player;

    //state timers
    float searching = -1;
    float awareness = -1;
    float charge = 0;
    float reload = 3;

    bool dead = false;

    //ray variables
    LineRenderer lr;
    [SerializeField] Material mat;

    //---Event Functions---
    /*
     * 1 - set state to patrol
     * 2 - get needed components
     */
    void Start()
    {
        botState = BotSM.patrol;
        agent = GetComponent<NavMeshAgent>();
        lr = GetComponent<LineRenderer>();
        orb = transform.GetChild(0).gameObject.GetComponent<FloatingOrb>();
    }

    /*
     * 1 - call behaviour function based on the state 
     * 2 - if theres a location, go to it
     */
    void Update()
    {
        switch (botState)
        {
            case BotSM.patrol:
                Patrol();
                break;
            case BotSM.chase:
                Chase();
                break;
            case BotSM.shoot:
                Shoot();
                break;
            case BotSM.search:
                Search();
                break;
            case BotSM.die:
                Die();
                break;
            default:
                Debug.Log("Unknown state");
                botState = BotSM.patrol;
                break;
        }
        if(location != null)
        {
            agent.SetDestination(location);
        }
    }

    //---Ray Functions---
    /*
     * 1 - create an array of two vectors, the current location and the aimed location
     * 2 - set the positions of the line renderer to the array
     */
    void DeathBeam(Vector3 aim)
    {
        //1
        lr.positionCount = 2;
        Vector3[] positions = new Vector3[2] { transform.position + Vector3.up * orb.currentHeight, aim };
        //2
        lr.SetPositions(positions);
        lr.material = mat;
    }
    /*
     * 1 - set the amount of positions in the line renderer to zero
     */
    void RemoveBeam()
    {
        lr.positionCount = 0;
        Vector3[] positions = new Vector3[1] { transform.position + Vector3.up * 2 };
        lr.SetPositions(positions);
    }

    //---Detection functions---
    /*
     * 1 - make a vector that represents the space between the player and the bot
     * 2 - if the magnitude of the vector
     * 3 - and if the angle between the bots forward and the vector is smaller than the vision angle
     * 4 - and if a ray cast in the direction of the vector would hit a gameObject with the Player tag
     * 5 - return the players position
     * 6 - otherwise return a zero Vector3
     */
    Vector3 See(float range = 60, float angle = 30)
    {
        //1
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        float delatAngle = Vector3.Angle(transform.forward, toPlayer);
        RaycastHit hit;
        //2
        if (toPlayer.magnitude < range)
            //3
            if (delatAngle < angle && delatAngle > -angle)
                //4
                if (Physics.Raycast(transform.position + Vector3.up, toPlayer, out hit, Mathf.Infinity))
                    if(hit.collider.CompareTag("Player"))
                        //5
                        return player.position;
        //6
        return Vector3.zero;
    }
    /*
     * 1 - calculate the distance between the player and the but
     * 2 - if the distance is smaller than the range, return the player position
     * 3 - otherwise return a zero vector3
     */
    Vector3 Hear(float range = 10)
    {
        //1
        float distance = (player.position - transform.position).magnitude;
        //2
        if(distance < range)
            return player.position;
        //3
        return Vector3.zero;
    }

    //---State Behaviour Functions---
    /*
     * 1 - Set the speed to walking speed
     * 2 - Check the patrol exists, otherwise set the location to the current position
     * 3 -if the pointer = -1, find the clostest patrol point
     *  a - set pointer = 0
     *  b - create a distance variable which will be the distance to the closest patrol point
     *  c - loop over the patrol
     *  d - if the distance is smaller than the current closest, set the pointer to the itteratio and update the closest
     * 4- if the distance from the net patrol point, set the next patrol point
     *  a - if the pointer is pointing at the end of the array set the pointer to zero
     *  b - otherwise increase the pointer
     * 5 - set the location to the patrol point
     * 6 - if the player is seen or heard begin chasing
     */
    void Patrol()
    {
        orb.movementSpeed = 12;
        orb.amplitude = 0.3f;
        //1
        agent.speed = 3;
        //2
        if (patrol.Length > 0)
        {
            //3
            if (point == -1)
            {
                //a
                point = 0;
                //b
                float closest = 0;
                //c
                for (int i = 0; i < patrol.Length - 1; i++)
                {
                    //d
                    float dis = (transform.position - patrol[i].position).magnitude;
                    if (dis < closest)
                    {
                        closest = dis;
                        point = i;
                    }
                }
            }
            //4
            float distance = (location - transform.position).magnitude;
            if (distance < successRadius)
            {
                //a
                if (point == patrol.Length - 1)
                    point = 0;
                //b
                else
                    point++;
            }
            //5
            location = patrol[point].position;

        }
        else
        {
            Debug.Log("Unknown patrol");
            location = transform.position;
        }
        //6
        if (See() != Vector3.zero || Hear() != Vector3.zero)
        {
            point = -1;
            botState = BotSM.chase;
        }
    }
    /*
     * 1 - set the speed to running
     * 2 - if the player can be seen or heard, set the location to the player and increase awareness else decrease the awareness
     * 3 - if entering chase state, set awareness to 80
     * 4 - if close to the location but the player cant be heard or awareness is zero, begin a search
     * 5 - if close to the location and the player can be heard, stop moving
     * 6 - if the player can be seen within 15, increase charge and draw a beam, else decrease charge
     * 7 - if charge is greater than 10, begin shoot
     */
    void Chase()
    {

        orb.movementSpeed = 8;
        orb.amplitude = 0.2f;
        //1
        agent.speed = 16;

        //3
        if (awareness == -1)
            awareness = 80;

        //2
        if (See() != Vector3.zero)
        {
            location = See();
            awareness += 100 * Time.deltaTime;
        }
        else if (Hear() != Vector3.zero)
        {
            location = Hear();
            awareness += 100 * Time.deltaTime;
        }
        else
            awareness -= 40 * Time.deltaTime;

        float distance = (transform.position - location).magnitude;

        //4
        if ((Hear() == Vector3.zero && See() == Vector3.zero) || awareness < 0)
        {
            awareness = -1;
            botState = BotSM.search;
        }
        //5
        if (See(7, 180) != Vector3.zero)
        {
            location = transform.position;
            transform.LookAt(See(15, 180));
        }
        //6
        if (See(15) != Vector3.zero)
        {
            target = See() + Vector3.up;
            charge += Time.deltaTime;
            DeathBeam(target);
        }
        else
        {
            charge -= 0.5f * Time.deltaTime;
            RemoveBeam();
        }
        if (charge < 0)
            charge = 0;
        //7
        if(charge > 2)
            botState = BotSM.shoot;
    }
    /*
     * 1 - slow to a crawl
     * 2 - draw a deathBeam to the location
     * 3 - if not shot yet and charge is greater than 11, set shot to true, begin reload, spawn explosion
     * 4 - if reload is below zero, set state to search and reset variables
     */
    void Shoot()
    {
        orb.movementSpeed = 5;
        orb.amplitude = 0.1f;
        //1
        agent.speed = 0.5f;      
        //2
        DeathBeam(target);
        //3
        if (!shot && charge > 3)
        {
            shot = true;
            Instantiate(explosion, target, Quaternion.identity);
            reload = 3;
        }
        else if(charge > 6)
        {
            orb.movementSpeed = 0;
            orb.amplitude = 0;
        }
        else
        {
            orb.movementSpeed = 5;
            orb.amplitude = 0.1f;
            charge += Time.deltaTime;
        }

        if (shot) 
            reload -= Time.deltaTime;
        //4
        if (reload < 0)
        {
            shot = false;
            reload = 3;
            charge = 0;
            botState = BotSM.search;
        }
        if (reload < 2.7f)
            RemoveBeam();
    }
    /*
     * 1 - set speed to a run
     * 2 - if entering search, set timer to 15
     * 3 - else if timer is lower than zero, begin patrol
     * 4 - if the player is seen, begin to chase
     * 5 - if the player is heard, begin to chase
     * 6 - if close to location, rotate
     */
    void Search()
    {
        RemoveBeam();
        //1
        agent.speed = 12;
        //2
        if(searching == -1)
            searching = 15;
        //3
        else if (searching < 0)
        {
            searching = -1;
            botState = BotSM.patrol;
        }
        else
            searching -= Time.deltaTime;
        //4
        if(See(60, 45) != Vector3.zero)
        {
            searching = -1;
            location = See(60, 45);
            botState = BotSM.chase;
        }
        //5
        else if(Hear() != Vector3.zero)
        {
            searching = -1;
            location = Hear(15);
            botState = BotSM.chase;
        }
        //6
        if((transform.position - location).magnitude < successRadius)
        {
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
            orb.movementSpeed = 6;
            orb.amplitude = 0.5f;
        }
        else
        {
            orb.movementSpeed = 8;
            orb.amplitude = 0.2f;
        }
    }
    /*
     * 1 - if entering die state, set dead to true and invoke death
     * 2 - rotate forward
     * 3 - disable the navigation agent
     */
    void Die()
    {
        orb.movementSpeed = 0;
        orb.amplitude = 0;
        //1
        if (dead == false)
        {
            Invoke("Death", 0.6f);
            dead = true;
        }
        //2
        transform.Rotate(Vector3.right, 90/1.1f * Time.deltaTime);
        //3
        agent.enabled = false;
    }
    //destroy gameObject
    void Death()
    {
        Destroy(gameObject);
    }
}
