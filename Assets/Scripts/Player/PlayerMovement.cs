using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 25;
    public float sensitivity = 360;

    public float stamina = 100;
    public float reacharge = 0;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(GameManager.GetHP() > 0)
        {
            Move();
            Turn();
            SpeedControl();
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0, z);

        if (movement.magnitude > 1)
            movement = movement / movement.magnitude;

        movement = movement * speed;
        movement = transform.TransformDirection(movement);

        rb.velocity = movement;
    }

    void Turn()
    {
        float y = Input.GetAxis("Mouse X");
        y = y  * sensitivity * Time.deltaTime;

        transform.Rotate(new Vector3(0, y, 0));
    }

    void SpeedControl()
    {
        stamina = GameManager.GetStamina();
        if (stamina == 0)
        {
            GameManager.Recharge();
            speed = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameManager.ReduceStamina();
                speed *= 1.1f;
            }
            else
            {
                GameManager.IncreaseStamina();
                speed *= 0.8f;
                if (stamina > 100)
                    stamina = 100;
            }

            speed = Mathf.Clamp(speed, 16, 22);
        }
    }
}
