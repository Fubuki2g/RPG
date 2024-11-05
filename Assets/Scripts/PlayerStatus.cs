using UnityEngine;
using TMPro;

public class PlayerStatus : Singleton<PlayerStatus>
{
    public int gdamage;            // 与えるダメージ
    public int rdamage;            // 受けるダメージ

    public TextMeshProUGUI Name;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI MP;
    public TextMeshProUGUI Lv;

    private void Update()
    {
        Name.text = DataManager.Instance.LoadString("PlayerName");
        HP.text = DataManager.Instance.LoadInt("nowHP").ToString();
        MP.text = DataManager.Instance.LoadInt("nowMP").ToString();
        Lv.text = DataManager.Instance.LoadInt("nowLv").ToString();
    }

    // プレイヤーがダメージを与える処理
    public void GiveDamage()
    {
        gdamage = DataManager.Instance.LoadInt("ATP") - EnemyStatusManager.Instance.EdefensePower;
        if (gdamage < 0)
        {
            gdamage = 0;
        }
        EnemyStatusManager.Instance.EcurrentHP -= gdamage;
        
    }

    // プレイヤーがダメージを受ける処理
    public void ReceiveDamage()
    {
        rdamage = EnemyStatusManager.Instance.EattackPower - DataManager.Instance.LoadInt("DFP");
        if (rdamage < 0)
        {
            rdamage = 0;
        }
        DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("nowHP") - rdamage);
        if (DataManager.Instance.LoadInt("nowHP") < 0)
        {
            DataManager.Instance.SaveInt("nowHP", 0);
        }
    }

    // プレイヤーが魔法でダメージを与える処理
    public void GiveMagicDamage(int givemagic)
    {
        EnemyStatusManager.Instance.EcurrentHP -= givemagic;
        if (EnemyStatusManager.Instance.EcurrentHP < 0)
        {
            EnemyStatusManager.Instance.EcurrentHP = 0;
        }
    }

    // プレイヤーが魔法でダメージを受ける処理
    public void ReceiveMagicDamage(int receivemagic)
    {
        DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("nowHP") - receivemagic);
        if (DataManager.Instance.LoadInt("nowHP") < 0)
        {
            DataManager.Instance.SaveInt("nowHP", 0);
        }
    }

    // プレイヤーがMPを消費する処理
    public void ConsumeMP(int mpCost)
    {
        DataManager.Instance.SaveInt("nowMP", DataManager.Instance.LoadInt("nowMP") - mpCost);
        if (DataManager.Instance.LoadInt("nowMP") < 0)
        {
            DataManager.Instance.SaveInt("nowMP",0);
        }
    }

    // プレイヤーがHPを回復する処理
    public void Heal(int healAmount)
    {
        DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("nowHP") + healAmount);
        if (DataManager.Instance.LoadInt("nowHP") > DataManager.Instance.LoadInt("maxHP"))
        {
            DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("maxHP"));
        }
    }

    // プレイヤーのステータスをリセットする処理
    public void ResetStatus()
    {
        DataManager.Instance.SaveInt("nowHP", DataManager.Instance.LoadInt("maxHP"));
        DataManager.Instance.SaveInt("nowMP", DataManager.Instance.LoadInt("maxMP"));
    }

    // プレイヤーのレベルが上がる処理
    public void LevelUp()
    {
        DataManager.Instance.SaveInt("nowLv",DataManager.Instance.LoadInt("nowLv") + 1);
        DataManager.Instance.SaveInt("nowEXP",0);
        DataManager.Instance.SaveInt("upEXP", DataManager.Instance.LoadInt("upEXP") * 2);
        DataManager.Instance.SaveInt("maxHP",DataManager.Instance.LoadInt("maxHP") + 10);
        DataManager.Instance.SaveInt("maxMP",DataManager.Instance.LoadInt("maxMP") + 5);
        DataManager.Instance.SaveInt("ATP", DataManager.Instance.LoadInt("ATP") + 2);
        DataManager.Instance.SaveInt("DFP", DataManager.Instance.LoadInt("DFP") + 1);
        DataManager.Instance.SaveInt("SPD", DataManager.Instance.LoadInt("SPD") + 1);

    }
}
