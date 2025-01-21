using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatrol : MonoBehaviour
{
    GoTo goTo;
    Detect detect;
    Botv2Brain brain;

    public Transform[] patrol;
    int point = -1;

    Vector3 location;

    public float successRadius = 5;

    void Start()
    {
        goTo = GetComponent<GoTo>();
        detect = GetComponent<Detect>();
        brain = GetComponent<Botv2Brain>();
    }

    public void Patrol()
    {
        goTo.SetSpeed(3);
        if (patrol.Length > 0)
        {
            if(point == -1)
                FindClosestPoint();

            float distance = (location - transform.position).magnitude;
            if(distance < successRadius)
                NextPoint();
        }
        else
        {
            Debug.Log("Unknown patrol");
            location = transform.position;
        }

        if (detect.Player(10,80, 30) != Vector3.zero)
        {
            point = -1;
            brain.BeginChase();
        }

        goTo.Location(location);
    }

    void FindClosestPoint()
    {
        point = 0;
        float closest = 0;
        for (int i = 0; i < patrol.Length -1; i++)
        {
            float distance = (transform.position - patrol[i].position).magnitude;
            if (distance < closest)
            {
                closest = distance;
                point = i;
            }
        }

        location = patrol[point].position;
    }

    void NextPoint()
    {
        if (point == patrol.Length - 1)
            point = 0;
        else
            point++;
        location = patrol[point].position;
    }
}
