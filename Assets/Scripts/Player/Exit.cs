using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool exited = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player.
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!exited)
            {
                GameManager.ExitLevel();
                exited = true;
            }
        }
    }
}
