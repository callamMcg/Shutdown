using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private bool collected = false;

    private float movementSpeed = 4;
    private float amplitude = 0.005f;
    private float startingHeight;

    private void OnCollisionEnter(Collision collision)
    {

        // Check if the collision is with the player.
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collected)
            {
                GameManager.CollectKey();
                collected = true;
            }
            // Destroy the object.
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        float verticalMovement = Mathf.Sin(movementSpeed * Time.time) * amplitude;

        transform.position = transform.position + Vector3.up * verticalMovement;

        transform.Rotate(Vector3.up, 60 * Time.deltaTime);
    }


    void Start()
    {
        startingHeight = 1.5f;
    }
}
