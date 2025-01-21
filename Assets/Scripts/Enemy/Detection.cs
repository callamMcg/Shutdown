using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Transform player;
    public LayerMask objectsLayer;

    public bool Listen(float range)
    {
        Vector3 toPlayer = player.position - transform.position;
        if(toPlayer.magnitude > range)
        {
            return false;
        }
        return true;
    }

    public bool Look(float range, float angle)
    {
        Vector3 toPlayer = player.position - transform.position;

        if (toPlayer.magnitude > range)
            return false;

        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, toPlayer, out hit, Mathf.Infinity, objectsLayer))
            return false;

        toPlayer.y = 0;
        float delatAngle = Vector3.Angle(transform.forward, toPlayer);
        if( delatAngle > angle || delatAngle < -angle)
            return false;

        return true;
    }
}
