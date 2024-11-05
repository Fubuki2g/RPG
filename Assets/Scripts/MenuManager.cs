using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject[] Menue;           // 項目ごとのキャンバス
    public GameObject TextBox;           // 下のテキスト用のテキストボックス
    public TextMeshProUGUI DoText;       // 使用時、装備時のテキスト
    public GameObject[] ItemUseButton;   // アイテム使用のボタン
    public TextMeshProUGUI[] ItemValue;  // アイテムごとの個数を表示するテキスト
    public bool itemFLG;                 // アイテムの使用中のフラグ
    public GameObject[] MagicButton;     // 魔法使用のボタン
    public bool magicFLG;                // 魔法使用中のフラグ
    public GameObject[] ArmButton;       // 装備ごとのボタン
    public GameObject[] ArmText;         // 装備中かどうかを表紙するテキスト
    public bool ironswordFLG;            // 鉄の剣処理のフラグ
    public bool ironshieldFLG;           // 鉄の盾処理のフラグ
    public bool superswordFLG;           // 伝説の剣処理のフラグ
    public bool supershieldFLG;          // 伝説の盾処理のフラグ
    public TextMeshProUGUI[] StatusText; // ステータスを表示するテキスト
    public GameObject[] BackButton;      // 初期選択ボタン
    public GameObject ItemButton;        // もどった先の初期選択ボタン
    public Button[] ActiveButton;        // ボタンが機能するかしないかの変更
    public bool buttonactive;            // ボタンが機能するかのフラグ

    public void Start()
    {
        itemFLG = false;
        magicFLG = false;
        ironswordFLG = false;
        ironshieldFLG = false;
        superswordFLG = false;
        supershieldFLG = false;
        buttonactive = true;
    }


    public void Update()
    {

        if (DataManager.Instance.LoadInt("Leaf") > 0)
        {
            ItemUseButton[0].SetActive(true);
            ItemValue[0].text = DataManager.Instance.LoadInt("Leaf").ToString();
        }
        else
        {
            ItemUseButton[0].SetActive(false);
        }

        if (DataManager.Instance.LoadInt("nowLv") >= 3)
        {
            MagicButton[0].SetActive(true);
        }
        else
        {
            MagicButton[0].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("IronSword"))
        {
            ArmButton[0].SetActive(true);
        }
        else
        {
            ArmButton[0].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("IronShield"))
        {
            ArmButton[1].SetActive(true);
        }
        else
        {
            ArmButton[1].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("GetSword"))
        {
            ArmButton[2].SetActive(true);
        }
        else
        {
            ArmButton[2].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("GetShield"))
        {
            ArmButton[3].SetActive(true);
        }
        else
        {
            ArmButton[3].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("IronSwordEquip"))
        {
            ArmText[0].SetActive(true);
        }
        else
        {
            ArmText[0].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("IronShieldEquip"))
        {
            ArmText[1].SetActive(true);
        }
        else
        {
            ArmText[1].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("GetSwordEquip"))
        {
            ArmText[2].SetActive(true);
        }
        else
        {
            ArmText[2].SetActive(false);
        }

        if (DataManager.Instance.LoadBool("GetShieldEquip"))
        {
            ArmText[3].SetActive(true);
        }
        else
        {
            ArmText[3].SetActive(false);
        }

        StatusText[0].text = DataManager.Instance.LoadString("PlayerName");
        StatusText[1].text = DataManager.Instance.LoadInt("nowLv").ToString();
        StatusText[2].text = DataManager.Instance.LoadInt("nowHP").ToString();
        StatusText[3].text = DataManager.Instance.LoadInt("maxHP").ToString();
        StatusText[4].text = DataManager.Instance.LoadInt("nowMP").ToString();
        StatusText[5].text = DataManager.Instance.LoadInt("maxMP").ToString();
        StatusText[6].text = DataManager.Instance.LoadInt("ATP").ToString();
        StatusText[7].text = DataManager.Instance.LoadInt("DFP").ToString();
        StatusText[8].text = DataManager.Instance.LoadInt("Gold").ToString();

    }

    public void Item()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        Menue[0].SetActive(false);
        Menue[1].SetActive(true);
        EventSystem.current.SetSelectedGameObject(BackButton[0]);
    }

    public void HealLeaf()
    {
        ButtonActive();
        if (!itemFLG)
        {
            itemFLG = true;
            StartCoroutine(healleaf());
        }

    }

    public IEnumerator healleaf()
    {
        if (DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
        {
            TextBox.SetActive(true);
            DoText.text = "使っても意味がない";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            TextBox.SetActive(false);
            itemFLG = false;
            ButtonActive();
            EventSystem.current.SetSelectedGameObject(BackButton[0]);
        }
        else if (DataManager.Instance.LoadInt("Leaf") < 1)
        {
            TextBox.SetActive(true);
            DoText.text = "持っていないよ";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            TextBox.SetActive(false);
            itemFLG = false;
            ButtonActive();
            EventSystem.current.SetSelectedGameObject(BackButton[0]);
        }
        else
        {
            SoundManager.Instance.PlaySE_Sys(4);
            TextBox.SetActive(true);
            DoText.text = "HPが回復した";
            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
            DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("nowHP") + 30);
            if (DataManager.Instance.LoadInt("nowHP") > DataManager.Instance.LoadInt("maxHP"))
            {
                DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("maxHP"));
            }

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            TextBox.SetActive(false);
            itemFLG = false;
            ButtonActive();
            EventSystem.current.SetSelectedGameObject(BackButton[0]);
        }

    }

    public void Equip()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        Menue[0].SetActive(false);
        Menue[2].SetActive(true);
        EventSystem.current.SetSelectedGameObject(BackButton[2]);
    }

    public void IronSword()
    {
        ButtonActive();
        if (!ironswordFLG)
        {
            SoundManager.Instance.PlaySE_Sys(5);
            ironswordFLG = true;
            StartCoroutine(equip());
        }

    }

    public void IronShield()
    {
        ButtonActive();
        if (!ironshieldFLG)
        {
            SoundManager.Instance.PlaySE_Sys(5);
            ironshieldFLG = true;
            StartCoroutine(equip());
        }

    }

    public void SuperSword()
    {
        ButtonActive();
        if (!superswordFLG)
        {
            SoundManager.Instance.PlaySE_Sys(5);
            superswordFLG = true;
            StartCoroutine(equip());
        }

    }

    public void SuperShield()
    {
        ButtonActive();
        if (!supershieldFLG)
        {
            SoundManager.Instance.PlaySE_Sys(5);
            supershieldFLG = true;
            StartCoroutine(equip());
        }

    }

    public IEnumerator equip()
    {
        if (ironswordFLG)
        {
            // 鉄の剣を装備しているとき
            if (DataManager.Instance.LoadBool("IronSwordEquip") && !DataManager.Instance.LoadBool("GetSwordEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "鉄の剣を外した";
                DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") - 10);
                DataManager.Instance.SaveBool("IronSwordEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                ironswordFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
                yield break;

            }
            // 何も装備していないとき
            else if (!DataManager.Instance.LoadBool("IronSwordEquip") && !DataManager.Instance.LoadBool("GetSwordEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "鉄の剣を装備した";
                DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") + 10);
                DataManager.Instance.SaveBool("IronSwordEquip", true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                ironswordFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
            // 伝説の剣を装備しているとき
            else if (DataManager.Instance.LoadBool("GetSwordEquip") && !DataManager.Instance.LoadBool("IronSwordEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "鉄の剣を装備した";
                DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") - 20);
                DataManager.Instance.SaveBool("IronSwordEquip", true);
                DataManager.Instance.SaveBool("GetSwordEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                ironswordFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
        }
        else if (ironshieldFLG)
        {
            // 鉄の盾を装備しているとき
            if (DataManager.Instance.LoadBool("IronShieldEquip") && !DataManager.Instance.LoadBool("GetShieldEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "鉄の盾を外した";
                DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") - 10);
                DataManager.Instance.SaveBool("IronShieldEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                ironshieldFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);

            }
            // 何も装備していないとき
            else if (!DataManager.Instance.LoadBool("IronShieldEquip") && !DataManager.Instance.LoadBool("GetShieldEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "鉄の盾を装備した";
                DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") + 10);
                DataManager.Instance.SaveBool("IronShieldEquip", true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                ironshieldFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
            // 伝説の盾を装備しているとき
            else if (DataManager.Instance.LoadBool("GetShieldEquip") && !DataManager.Instance.LoadBool("IronShieldEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "鉄の盾を装備した";
                DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") - 20);
                DataManager.Instance.SaveBool("IronShieldEquip", true);
                DataManager.Instance.SaveBool("GetShieldEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                ironshieldFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
        }
        else if (superswordFLG)
        {
            // 伝説の剣を装備しているとき
            if (!DataManager.Instance.LoadBool("IronSwordEquip") && DataManager.Instance.LoadBool("GetSwordEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "伝説の剣を外した";
                DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") - 30);
                DataManager.Instance.SaveBool("GetSwordEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                superswordFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);

            }
            // 何も装備していないとき
            else if (!DataManager.Instance.LoadBool("IronSwordEquip") && !DataManager.Instance.LoadBool("GetSwordEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "伝説の剣を装備した";
                DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") + 30);
                DataManager.Instance.SaveBool("GetSwordEquip", true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                superswordFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
            // 鉄の剣を装備しているとき
            else if (!DataManager.Instance.LoadBool("GetSwordEquip") && DataManager.Instance.LoadBool("IronSwordEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "伝説の剣を装備した";
                DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") + 20);
                DataManager.Instance.SaveBool("GetSwordEquip", true);
                DataManager.Instance.SaveBool("IronSwordEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                superswordFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
        }
        else if (supershieldFLG)
        {
            // 伝説の盾を装備しているとき
            if (!DataManager.Instance.LoadBool("IronShieldEquip") && DataManager.Instance.LoadBool("GetShieldEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "伝説の盾を外した";
                DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") - 30);
                DataManager.Instance.SaveBool("GetShieldEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                supershieldFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);

            }
            // 何も装備していないとき
            else if (!DataManager.Instance.LoadBool("IronShieldEquip") && !DataManager.Instance.LoadBool("GetShieldEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "伝説の盾を装備した";
                DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") + 30);
                DataManager.Instance.SaveBool("GetShieldEquip", true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                supershieldFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
            // 鉄の盾を装備しているとき
            else if (!DataManager.Instance.LoadBool("GetShieldEquip") && DataManager.Instance.LoadBool("IronShieldEquip"))
            {
                TextBox.SetActive(true);
                DoText.text = "伝説の盾を装備した";
                DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") + 20);
                DataManager.Instance.SaveBool("GetShieldEquip", true);
                DataManager.Instance.SaveBool("IronShieldEquip", false);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                TextBox.SetActive(false);
                supershieldFLG = false;
                ButtonActive();
                EventSystem.current.SetSelectedGameObject(BackButton[2]);
            }
        }

    }

    public void Magic()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        Menue[0].SetActive(false);
        Menue[3].SetActive(true);
        EventSystem.current.SetSelectedGameObject(BackButton[1]);
    }

    public void HealMagic()
    {
        ButtonActive();
        if (!magicFLG)
        {
            magicFLG = true;
            StartCoroutine(healmagic());
        }
    }

    public IEnumerator healmagic()
    {
        if (DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
        {
            TextBox.SetActive(true);
            DoText.text = "何も起こらなかった";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            TextBox.SetActive(false);
            magicFLG = false;
            ButtonActive();
            EventSystem.current.SetSelectedGameObject(BackButton[1]);
        }
        else if (DataManager.Instance.LoadInt("nowMP") < 3)
        {
            TextBox.SetActive(true);
            DoText.text = "MPがたりないようだ";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            TextBox.SetActive(false);
            magicFLG = false;
            ButtonActive();
            EventSystem.current.SetSelectedGameObject(BackButton[1]);
        }
        else
        {
            SoundManager.Instance.PlaySE_Sys(4);
            TextBox.SetActive(true);
            DoText.text = "HPが回復した";
            DataManager.Instance.SaveInt("nowMP", DataManager.Instance.LoadInt("nowMP") - 3);
            DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("nowHP") + 30);
            if (DataManager.Instance.LoadInt("nowHP") > DataManager.Instance.LoadInt("maxHP"))
            {
                DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("maxHP"));
            }

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            TextBox.SetActive(false);
            magicFLG = false;
            ButtonActive();
            EventSystem.current.SetSelectedGameObject(BackButton[1]);
        }

    }

    public void Status()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        Menue[0].SetActive(false);
        Menue[4].SetActive(true);
        EventSystem.current.SetSelectedGameObject(BackButton[3]);
    }

    public void Back()
    {
        SoundManager.Instance.PlaySE_Sys(0);
        Menue[0].SetActive(true);
        Menue[1].SetActive(false);
        Menue[2].SetActive(false);
        Menue[3].SetActive(false);
        Menue[4].SetActive(false);
        EventSystem.current.SetSelectedGameObject(ItemButton);
    }

    public void ButtonActive()
    {
        if (buttonactive)
        {
            ActiveButton[0].interactable = false;
            ActiveButton[1].interactable = false;
            ActiveButton[2].interactable = false;
            ActiveButton[3].interactable = false;
            ActiveButton[4].interactable = false;
            ActiveButton[5].interactable = false;
            ActiveButton[6].interactable = false;
            ActiveButton[7].interactable = false;
            ActiveButton[8].interactable = false;
            buttonactive = false;
        }
        else
        {
            ActiveButton[0].interactable = true;
            ActiveButton[1].interactable = true;
            ActiveButton[2].interactable = true;
            ActiveButton[3].interactable = true;
            ActiveButton[4].interactable = true;
            ActiveButton[5].interactable = true;
            ActiveButton[6].interactable = true;
            ActiveButton[7].interactable = true;
            ActiveButton[8].interactable = true;
            buttonactive = true;
        }
    }
}
