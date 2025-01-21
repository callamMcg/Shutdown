using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    int targetIndex;
    Transform target;

    Rigidbody rb;

    float turnTimer = 0;
    public float moveSpeed = 2;

    void Start()
    {
        targetIndex = 0;
        target = patrolPoints[targetIndex];
        rb = GetComponent<Rigidbody>();
    }

    public void UpdatePatrol()
    {
        SetTarget();
        Move();
    }

    void SetTarget()
    {
        Vector3 toTarget = transform.position - target.position;

        if(toTarget.magnitude < 1)
        {
            if(targetIndex == patrolPoints.Length - 1)
            {
                targetIndex = 0;
            }
            else
            {
                targetIndex++;
            }
            target = patrolPoints[targetIndex];
        }

    }

    void Move()
    {
        Vector3 direction = target.position - transform.position;
        direction = direction / direction.magnitude;

        if(direction == transform.forward)
        {
            turnTimer = Time.time;
        }
        else
        {
            float t = Time.time - turnTimer;

            var newRotation = Quaternion.LookRotation(direction);

            newRotation.z = 0.0f;
            newRotation.x = 0.0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, t / 60);
        }

        rb.velocity = transform.forward * moveSpeed;
    }

}
