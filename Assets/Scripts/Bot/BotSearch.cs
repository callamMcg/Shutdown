using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSearch : MonoBehaviour
{

    GoTo goTo;
    Detect detect;
    Botv2Brain brain;

    float ticker = 3;

    void Start()
    {
        goTo = GetComponent<GoTo>();
        detect = GetComponent<Detect>();
        brain = GetComponent<Botv2Brain>();
    }

    public void Search(Vector3 lastLocation)
    {
        goTo.SetSpeed(1);
        goTo.Location(lastLocation);

        float distance = (transform.position - lastLocation).magnitude;
        if(distance < 3)
        {
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
            ticker -= Time.deltaTime;
        }
        else
        {
            goTo.Location(lastLocation);
        }
        Debug.Log(ticker);
        if(ticker < 0)
        {
            ticker = 3;
            brain.BeginPatrol();
        }

        if(detect.Player(10, 80, 35, true) != Vector3.zero)
        {

            ticker = 3;
            brain.BeginChase();
        }
    }
}
