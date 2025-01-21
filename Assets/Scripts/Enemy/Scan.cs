using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan : MonoBehaviour
{
    void Start()
    {
        
    }

    public void UpdateScan()
    {
        transform.Rotate(Vector3.up, 400 * Time.deltaTime);
    }
}
