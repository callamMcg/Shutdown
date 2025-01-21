using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotShoot : MonoBehaviour
{
    GoTo goTo;
    Detect detect;
    Botv2Brain brain;

    LineRenderer lr;
    [SerializeField] Material mat;

    void drawLine(Vector3 aim)
    {
        lr.positionCount = 2;
        Vector3[] positions = new Vector3[2] { transform.position + Vector3.up * 2, aim };
        lr.SetPositions(positions);
        lr.material = mat;
    }
    void removeLine()
    {
        lr.positionCount = 0;
    }

    public LayerMask objectsLayer;

    Vector3 player;

    public GameObject explosion;

    float reload = 2;
    float charge = 0;
    bool shot = false;

    Vector3 toPlayer;
    Vector3 aim;
    bool aimed = false;

    void Start()
    {
        goTo = GetComponent<GoTo>();
        detect = GetComponent<Detect>();
        brain = GetComponent<Botv2Brain>();
        lr = GetComponent<LineRenderer>();

    }

    public void Shoot()
    {

        goTo.SetSpeed(2);
        if(charge > 2)
        {
            if (!shot && charge > 2.2f)
            {
                shot = true;
                Instantiate(explosion, aim, Quaternion.identity);
            }
            else
                charge += Time.deltaTime;
            reload -= Time.deltaTime;

            Debug.Log(reload);
            if (reload < 1.7f)
                removeLine();
            
            if(reload < 0)
            {
                shot = false;
                reload = 2;
                charge = 0;
            }
        }
        else
        {
            if (detect.Player(0, 10, 30) == Vector3.zero)
            {
                removeLine();
                if (detect.Player(15, 0, 0) == Vector3.zero)
                {
                    if (player == Vector3.zero)
                    {
                        Debug.Log("Fuck knows how this happened");
                        brain.BeginPatrol();
                    }
                    else
                        brain.BeginSearch(player);
                }
                else
                    goTo.Location(detect.Player(15, 0, 0));
            }
            else
            {

                player = detect.Player(0, 10, 30);
                if (detect.Player(0, 10, 10) == Vector3.zero)
                    goTo.Location(player);
                else
                    goTo.Location(transform.position);

                aim = player + Vector3.up;

                drawLine(aim);

                charge += Time.deltaTime;
            }
        }
        /*
        timer -= Time.deltaTime;
        float deltaAngle = Vector3.Angle(transform.forward, toPlayer);
        if (timer > 0.5f)
            if(detect.Player(0, 50, 30))
                transform.Rotate(Vector3.up, deltaAngle * Time.deltaTime);
        else if (timer < -0.15f)
            removeLine();
        else if (timer < 0.5f)
        {
            if (!aimed)
            {
                toPlayer = player.position - transform.position;
                Vector3 error = new Vector3(Ran(0.5f), Ran(0.5f), Ran(0.5f));
                aim = toPlayer + error;
                aimed = true;
            }
            drawLine(aim);
        }


        if (timer < 0 )
        {
            reload -= Time.deltaTime;
            if( reload < 0)
            {
                timer = 2;
                reload = 2;
                shot = false;
                aimed = false;
                if (detect.Player(10, 80, 30))
                    brain.BeginChase();
                else
                    brain.BeginSearch(transform.position + toPlayer);
            }
        }
        */
    }

    float Ran(float range)
    {
        return Random.Range(-range, range);
    }
}
