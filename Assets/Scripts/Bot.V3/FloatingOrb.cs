using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrb : MonoBehaviour
{

    public float movementSpeed = 6;
    public float amplitude = 0.015f;
    private float startingHeight;
    public float currentHeight;

    void Start()
    {
        startingHeight = 2;
    }

    void Update()
    {
        float verticalMovement = Mathf.Sin(movementSpeed * Time.time) * amplitude;
        currentHeight = verticalMovement + startingHeight;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }
}
