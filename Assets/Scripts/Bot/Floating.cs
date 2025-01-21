using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating: MonoBehaviour
{
    public float movementSpeed = 6;
    public float amplitude = 0.015f;
    private float startingHeight;

    void Start()
    {
        startingHeight = 2;
    }

    void Update()
    {
        float verticalMovement = Mathf.Sin(movementSpeed * Time.time) * amplitude;
        transform.position = new Vector3(transform.position.x, startingHeight + verticalMovement, transform.position.z);
    }
}
