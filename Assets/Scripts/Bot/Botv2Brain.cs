using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botv2Brain : MonoBehaviour
{
    BotPatrol patrol;
    BotChase chase;
    BotShoot shoot;
    BotSearch search;
    Floating orb;

    Vector3 suspectLocation;

    public enum BotSM
    {
        patrol, chase, shoot, search, scan, die
    }
    public BotSM botState;

    void Start()
    {
        botState = BotSM.patrol;
        patrol = GetComponent<BotPatrol>();
        chase = GetComponent<BotChase>();
        shoot = GetComponent<BotShoot>();
        search = GetComponent<BotSearch>();
        orb = gameObject.transform.GetChild(0).GetComponent<Floating>();
    }

    void Update()
    {
        switch(botState)
        {
            case BotSM.patrol:
                patrol.Patrol();
                break;
            case BotSM.chase:
                chase.Chase();
                break;
            case BotSM.shoot:
                shoot.Shoot();
                break;
            case BotSM.search:
                search.Search(suspectLocation);
                break;
            case BotSM.scan:
                break;
            case BotSM.die:
                break;
            default:
                Debug.Log("Unknown state");
                botState = BotSM.patrol;
                break;
        }
    }

    public void BeginPatrol()
    {
        botState = BotSM.patrol;
        orb.movementSpeed = 10;
        orb.amplitude = 0.15f;
    }

    public void BeginChase()
    {
        botState = BotSM.chase;
        orb.movementSpeed = 12;
        orb.amplitude = 0.15f;
    }

    public void BeginShoot()
    {
        botState = BotSM.shoot;
        orb.movementSpeed = 3;
        orb.amplitude = 0.05f;
    }
    public void BeginSearch(Vector3 lastLocation)
    {
        suspectLocation = lastLocation;
        orb.movementSpeed = 8;
        orb.amplitude = 0.02f;
        botState = BotSM.search;
    }
    public void BeginScan()
    {
        botState = BotSM.scan;
    }
    public void BeginDie()
    {
        botState = BotSM.die;
    }
}
