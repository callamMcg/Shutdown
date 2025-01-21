using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform player;

    Rigidbody rb;

    float turnTimer = 0;
    public float moveSpeed = 4;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateChase()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction == transform.forward)
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
        if(direction.magnitude > 4)
            rb.velocity = transform.forward * moveSpeed;
        else
            rb.velocity = Vector3.zero;
    }

}
