using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    Patrol patrol;
    Chase chase;
    Shoot shoot;
    Scan scan;
    Detection detection;

    float chaseTimer = -1;
    float shootTimer = 1.2f;
    float loadingTimer = 2;
    float scanTimer = -1;

    public int HP = 1;

    public enum EnemySM
    {
        patrol, chase, shoot, scan, die
    }
    public EnemySM enemySM;

    void Start()
    {
        patrol = GetComponent<Patrol>();
        chase = GetComponent<Chase>();
        shoot = GetComponent<Shoot>();
        scan = GetComponent<Scan>();
        detection = GetComponent<Detection>();
        enemySM = EnemySM.patrol;
    }

    void Update()
    {
        if (HP == 0)
        {
            enemySM = EnemySM.die;
        }
        switch (enemySM)
        {
            case EnemySM.patrol:
                patrol.UpdatePatrol();
                if (detection.Listen(4))
                    enemySM = EnemySM.chase;
                if (detection.Look(15, 45))
                    enemySM = EnemySM.chase;
                break;
            case EnemySM.chase:
                chase.UpdateChase();
                if(chaseTimer == -1)
                    chaseTimer = 2;
                if (detection.Look(20, 30))
                    chaseTimer = 2;
                else
                    chaseTimer -= Time.deltaTime;
                if (detection.Look(20, 20))
                    shootTimer -= Time.deltaTime;
                else
                    shootTimer = 1.2f;
                if (chaseTimer < 0)
                {
                    enemySM = EnemySM.patrol;
                    chaseTimer = -1;
                }
                else if (shootTimer < 0)
                {
                    shootTimer = 1.5f;
                    enemySM = EnemySM.shoot;
                }
                break;
            case EnemySM.shoot:
                shoot.UpdateShoot();
                loadingTimer -= Time.deltaTime;
                if (loadingTimer < 0)
                {
                    loadingTimer = 2;
                    enemySM = EnemySM.scan;
                }
                break;
            case EnemySM.scan:
                scan.UpdateScan();
                if(scanTimer == -1)
                    scanTimer = 1.5f;
                scanTimer -= Time.deltaTime;
                if (detection.Look(20, 20))
                    enemySM = EnemySM.chase;
                if (scanTimer < 0)
                {
                    scanTimer = -1;
                    enemySM = EnemySM.patrol;
                }
                break;
            case EnemySM.die:
                Debug.Log("die");
                //Destroy(gameObject);
                break;
        }
        
    }
}