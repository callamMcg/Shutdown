using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotChase : MonoBehaviour
{
    GoTo goTo;
    Detect detect;
    Botv2Brain brain;


    Vector3 location;

    float awareness = 80;

    void Start()
    {
        goTo = GetComponent<GoTo>();
        detect = GetComponent<Detect>();
        brain = GetComponent<Botv2Brain>();
    }

    public void Chase()
    {
        goTo.SetSpeed(1);

        if (detect.Player(15, 80, 30) != Vector3.zero)
        {
            location = detect.Player(15, 80, 30);

            awareness += 100 * Time.deltaTime;
            if(awareness > 100)
                awareness = 100;
        }
        else
            awareness -= 100 * Time.deltaTime;

        if (location == null)
            Debug.Log("Unknown location");
        else if (awareness > 0)
            if (detect.Player(0, 5, 15) != Vector3.zero)
                brain.BeginShoot();
            else
                goTo.Location(location);
        else
        {
            awareness = 80;
            brain.BeginSearch(location);
        }
    }
}
