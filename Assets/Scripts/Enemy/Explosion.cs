using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Vector3 currentScale;
    // Start is called before the first frame update
    void Start()
    {
        currentScale = transform.localScale;
        Invoke("End", 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        currentScale = currentScale + Vector3.one * 60 * Time.deltaTime;
        transform.localScale = currentScale;
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Here");
    }

    void End()
    {
        Destroy(gameObject);
    }
}
