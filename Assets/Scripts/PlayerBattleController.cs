using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerBattleController : Singleton<PlayerBattleController>
{
    public bool atack = false;
    public bool magic = false;
    public bool heal = false;
    public bool fire = false;
    public bool item = false;
    public bool leaf = false;
    public bool escape = false;

    public GameObject BattleMenu;
    public GameObject MagicBox;
    public GameObject[] MagicButton;
    public GameObject ItemBox;
    public GameObject[] ItemButton;
    public TextMeshProUGUI[] ItemCount;
    public GameObject DefaultButton;
    public GameObject[] Back;

    private void Start()
    {
        MagicBox.SetActive(false);
        ItemBox.SetActive(false);
    }

    private void Update()
    {
        ItemCount[0].text = DataManager.Instance.LoadInt("Leaf").ToString();

        if (DataManager.Instance.LoadInt("nowLv") >= 3)
        {
            MagicButton[0].SetActive(true);
        }
        if (DataManager.Instance.LoadInt("nowLv") >= 7)
        {
            MagicButton[1].SetActive(true);
        }

        if (DataManager.Instance.LoadInt("Leaf") < 1)
        {
            ItemButton[0].SetActive(false);
        }
        else
        {
            ItemButton[0].SetActive(true);
        }
    }

    public void Atack()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        BattleMenu.SetActive(false);
        atack = true;
        BattleManager.Instance.StartYourSequence();
    }

    public void Magic()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        BattleMenu.SetActive(false);
        MagicBox.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Back[0]);
    }

    public void HealMagic()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        MagicBox.SetActive(false);
        magic = true;
        heal = true;
        BattleManager.Instance.StartYourSequence();

    }

    public void FireMagic()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        MagicBox.SetActive(false);
        magic = true;
        fire = true;
        BattleManager.Instance.StartYourSequence();

    }

    public void Item()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        BattleMenu.SetActive(false);
        ItemBox.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Back[1]);
    }

    public void HealItem()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        ItemBox.SetActive(false);
        item = true;
        leaf = true;
        BattleManager.Instance.StartYourSequence();

    }

    public void BackButton()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        BattleMenu.SetActive(true);
        ItemBox.SetActive(false);
        MagicBox.SetActive(false);
        EventSystem.current.SetSelectedGameObject(DefaultButton);

    }

    public void Escape()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        BattleMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        escape = true;
        BattleManager.Instance.StartYourSequence();
    }

}

