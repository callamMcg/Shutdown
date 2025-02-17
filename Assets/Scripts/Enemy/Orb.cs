using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float movementSpeed = 6;
    public float amplitude = 0.15f;
    //private float startingHeight;

    //void Start()
    //{
    //    startingHeight = 2;
    //}

    void Update()
    {
        float verticalMovement = Mathf.Sin(movementSpeed * Time.time) * amplitude;

        transform.position = transform.position + Vector3.up * verticalMovement;
    }
}
