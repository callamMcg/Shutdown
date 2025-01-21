using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Indication : MonoBehaviour
{
    GameObject bloodyPanel;

    private void Start()
    {
        bloodyPanel = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if (GameManager.GetHP() < 2)
            bloodyPanel.SetActive(true);
        else
            bloodyPanel.SetActive(false);
    }
}
