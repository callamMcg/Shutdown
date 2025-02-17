using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance;

    //private void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private static int keys = 0;
    private static int HP = 2;
    private static float staminaRecharge = 0;
    private static float staminaTick = 0;
    private static int stamina = 100;
    private static bool ExitOpen = false;
    public GameObject exitDoor;
    float dyingTimer = 0.75f;

    public static void TakeDamage()
    {
        HP--;
    }

    public static void CollectKey()
    {
        keys++;
    }

    public static void ReduceStamina()
    {
        staminaTick += Time.deltaTime;
        if (staminaTick > 0.25f)
        {
            staminaTick = 0;
            stamina -= 5;
        }

        if (stamina < 0)
            stamina = 0;
    }
    public static bool Recharge()
    {
        staminaRecharge += Time.deltaTime;
        if(staminaRecharge > 1)
        {
            staminaRecharge = 0;
            stamina = 1;
            return true;
        }
        else
            return false;
    }
    public static void IncreaseStamina()
    {
        staminaTick += Time.deltaTime;
        if (staminaTick > 0.25f)
        {
            staminaTick = 0;
            stamina += 3;
        }
        if (stamina > 100)
            stamina = 100;
    }
    public static bool HasKeys()
    {
        return keys == 2;
    }

    public static int GetKeys()
    {
        return keys;
    }

    public static int GetHP()
    {
        return HP;
    }

    public static int GetStamina()
    {
        return stamina;
    }

    public static void ExitLevel()
    {
        SceneManager.LoadScene("Win");
    }

    public static void Die()
    {
        SceneManager.LoadScene("Loss");
    }

    public static void StartGame()
    {
        AudioManager.instance.PlayMusic();
        SceneManager.LoadScene("Level1");
    }

    public static void MainMenu()
    {
        keys = 0;
        HP = 2;
        ExitOpen = false;
        SceneManager.LoadScene("Opening");
    }

    void Update()
    {
        if(keys == 2)
            if (!ExitOpen)
            {
                Instantiate(exitDoor, new Vector3(79, 2.5f, 1), Quaternion.identity);
                ExitOpen = true;
            }
        if (HP < 1)
        {
            dyingTimer -= Time.deltaTime;
            if(dyingTimer < 0)
            {
                Die();
            }
        }
    }
}
