using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DataManager.Instance.DefaultStatus();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DataManager.Instance.SaveInt("nowLv", 10);
            DataManager.Instance.SaveInt("nowHP", 100);
            DataManager.Instance.SaveInt("maxHP",100);
            DataManager.Instance.SaveInt("nowMP",100);
            DataManager.Instance.SaveInt("maxMP",100);
            DataManager.Instance.SaveInt("ATP",100);
            DataManager.Instance.SaveInt("DFP",100);
            DataManager.Instance.SaveInt("SPD",100);
            DataManager.Instance.SaveInt("Gold", 1000);
            DataManager.Instance.SaveInt("Leaf", 10);
            DataManager.Instance.SaveBool("IronSword",true);
            DataManager.Instance.SaveBool("IronShield",true);
            DataManager.Instance.SaveBool("GetSword",true);
            DataManager.Instance.SaveBool("GetShield",true);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            DataManager.Instance.SaveInt("nowLv", 10);
            DataManager.Instance.SaveInt("nowHP", 100);
            DataManager.Instance.SaveInt("maxHP", 100);
            DataManager.Instance.SaveInt("nowMP", 100);
            DataManager.Instance.SaveInt("maxMP", 100);
            DataManager.Instance.SaveInt("ATP", 20);
            DataManager.Instance.SaveInt("DFP", 100);
            DataManager.Instance.SaveInt("SPD", 1);
            DataManager.Instance.SaveInt("Gold", 1000);
            DataManager.Instance.SaveInt("Leaf", 10);
            DataManager.Instance.SaveBool("IronSword", true);
            DataManager.Instance.SaveBool("IronShield", true);
            DataManager.Instance.SaveBool("GetSword", true);
            DataManager.Instance.SaveBool("GetShield", true);
        }
    }
}
