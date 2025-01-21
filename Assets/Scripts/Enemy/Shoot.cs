using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform player;
    public GameObject explosion;

    Rigidbody rb;
    public bool loading = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateShoot()
    {
        rb.velocity = Vector3.zero;
        if (!loading)
        {
            loading = true;
            Fire();
            rb.AddForce(transform.up * 100 + transform.forward * -100);
            Invoke("Reload", 2.1f);
        }
    }

    void Fire()
    {
        Vector3 ranError = new Vector3(Num(), Num(), Num());
        Vector3 toPlayer = player.position - transform.position;
        Vector3 shot = toPlayer + ranError;

        Instantiate(explosion, transform.position + shot , Quaternion.identity);
    }

    float Num()
    {
        return Random.Range(1.5f, 1.5f);
    }
    void Reload()
    {
        loading = false;
    }
}
