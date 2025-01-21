using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    float timer = 0.2f;
    bool hit = false;

    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
        foreach (var hitCollider in hitColliders)
            if (hitCollider.CompareTag("Player"))
                if (!hit)
                {
                    hit = true;
                    GameManager.TakeDamage();
                }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
            Destroy(gameObject);
        transform.localScale = transform.localScale + Vector3.one * 20 * Time.deltaTime;
    }
}