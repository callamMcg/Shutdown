using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public Transform player;
    public LayerMask objectsLayer;

    bool Listen(float range)
    {
        float distance = (player.position - transform.position).magnitude;
        if (distance > range)
            return false;
        return true;
    }

    bool Look(float range, float angle)
    {
        Vector3 toPlayer = player.position - transform.position;

        if (toPlayer.magnitude > range)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, toPlayer, out hit, Mathf.Infinity, objectsLayer))
            return false;

        toPlayer.y = 0;
        float delatAngle = Vector3.Angle(transform.forward, toPlayer);
        if (delatAngle > angle || delatAngle < -angle)
            return false;

        return true;
    }

    public Vector3 Player(float listenRange, float visualRange, float visualAngle, bool bug = false)
    {
        if(Listen(listenRange))
        {
            if(bug)
                Debug.Log("hear");
            return player.position;
        }
        if (Look(visualRange, visualAngle))
        {
            if (bug)
                Debug.Log("see");
            return player.position;
        }
        return Vector3.zero;
    }
}
