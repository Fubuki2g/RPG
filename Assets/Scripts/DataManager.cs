using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // 例: デフォルト値
    private const int DefaultIntValue = 0;
    private const float DefaultFloatValue = 0.0f;
    private const string DefaultStringValue = "";
    private const bool DefaultBoolValue = false;

    // int型のデータを保存
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    // int型のデータを読み込み
    public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key, DefaultIntValue);
    }

    // float型のデータを保存
    public void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    // float型のデータを読み込み
    public float LoadFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, DefaultFloatValue);
    }

    // string型のデータを保存
    public void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    // string型のデータを読み込み
    public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key, DefaultStringValue);
    }

    // bool型のデータを保存
    public void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    // bool型のデータを読み込み
    public bool LoadBool(string key)
    {
        return PlayerPrefs.GetInt(key, DefaultBoolValue ? 1 : 0) == 1;
    }

    // はじめからを選択した時に読み込むステータス
    public void DefaultStatus()
    {
        SaveInt("nowLv", 1); // プレーヤーの現在レベル
        SaveInt("nowEXP",0); // プレイヤーの現在経験値 
        SaveInt("upEXP",5);  // プレイヤーが次にレベルアップするまでの必要経験値 
        SaveInt("Gold",30);  // プレイヤーの所持金
        SaveInt("maxHP",20); // プレイヤーの最大HP
        SaveInt("nowHP",20); // プレイヤーの現在HP
        SaveInt("maxMP",0);  // プレイヤーの最大MP
        SaveInt("nowMP",0);  // プレイヤーの現在MP
        SaveInt("ATP",3);    // プレイヤーの攻撃力
        SaveInt("DFP",1);    // プレイヤーの守備力
        SaveInt("SPD",3);    // プレイヤーのすばやさ
        SaveInt("Leaf", 0);  // 薬草の個数
        SaveBool("IronSword", false);  // 鉄の剣の購入
        SaveBool("IronShield", false); // 鉄の盾の購入
        SaveBool("GetSword", false);   // 伝説の剣の入手
        SaveBool("GetShield", false);  // 伝説の盾の入手
        SaveBool("IronSwordEquip", false);  // 鉄の剣の装備
        SaveBool("IronShieldEquip", false); // 鉄の盾の装備
        SaveBool("GetSwordEquip", false);   // 伝説の剣の装備
        SaveBool("GetShieldEquip", false);  // 伝説の盾の装備
        SaveBool("Dead", false);
        SaveBool("LastBattle", false);

    }

    public void DataSave()
    {
        SaveInt("nowLvs",LoadInt("nowLv")); // プレーヤーの現在レベル
        SaveInt("nowEXPs", LoadInt("nowEXP")); // プレイヤーの現在経験値 
        SaveInt("upEXPs", LoadInt("upEXP"));  // プレイヤーが次にレベルアップするまでの必要経験値 
        SaveInt("Golds", LoadInt("Gold"));  // プレイヤーの所持金
        SaveInt("maxHPs", LoadInt("maxHP")); // プレイヤーの最大HP
        SaveInt("nowHPs", LoadInt("nowHP")); // プレイヤーの現在HP
        SaveInt("maxMPs", LoadInt("maxMP"));  // プレイヤーの最大MP
        SaveInt("nowMPs", LoadInt("nowMP"));  // プレイヤーの現在MP
        SaveInt("ATPs", LoadInt("ATP"));    // プレイヤーの攻撃力
        SaveInt("DFPs", LoadInt("DFP"));    // プレイヤーの守備力
        SaveInt("SPDs", LoadInt("SPD"));    // プレイヤーのすばやさ
        SaveInt("Leafs", LoadInt("Leaf"));  // 薬草の個数
        SaveBool("IronSwords", LoadBool("IronSword"));  // 鉄の剣の購入
        SaveBool("IronShields", LoadBool("IronShield")); // 鉄の盾の購入
        SaveBool("GetSwords", LoadBool("GetSword"));   // 伝説の剣の入手
        SaveBool("GetShields", LoadBool("GetShield"));  // 伝説の盾の入手
        SaveBool("IronSwordEquips", LoadBool("IronSwordEquip"));  // 鉄の剣の装備
        SaveBool("IronShieldEquips", LoadBool("IronShieldEquip")); // 鉄の盾の装備
        SaveBool("GetSwordEquips", LoadBool("GetSwordEquip"));   // 伝説の剣の装備
        SaveBool("GetShieldEquips", LoadBool("GetShieldEquip"));  // 伝説の盾の装備
        SaveBool("Deads", LoadBool("Dead"));
        SaveBool("LastBattles", LoadBool("LastBattle"));
    }

    public void DataLoad()
    {
        SaveInt("nowLv", LoadInt("nowLvs")); // プレーヤーの現在レベル
        SaveInt("nowEXP", LoadInt("nowEXPs")); // プレイヤーの現在経験値 
        SaveInt("upEXP", LoadInt("upEXPs"));  // プレイヤーが次にレベルアップするまでの必要経験値 
        SaveInt("Gold", LoadInt("Golds"));  // プレイヤーの所持金
        SaveInt("maxHP", LoadInt("maxHPs")); // プレイヤーの最大HP
        SaveInt("nowHP", LoadInt("nowHPs")); // プレイヤーの現在HP
        SaveInt("maxMP", LoadInt("maxMPs"));  // プレイヤーの最大MP
        SaveInt("nowMP", LoadInt("nowMPs"));  // プレイヤーの現在MP
        SaveInt("ATP", LoadInt("ATPs"));    // プレイヤーの攻撃力
        SaveInt("DFP", LoadInt("DFPs"));    // プレイヤーの守備力
        SaveInt("SPD", LoadInt("SPDs"));    // プレイヤーのすばやさ
        SaveInt("Leaf", LoadInt("Leafs"));  // 薬草の個数
        SaveBool("IronSword", LoadBool("IronSwords"));  // 鉄の剣の購入
        SaveBool("IronShield", LoadBool("IronShields")); // 鉄の盾の購入
        SaveBool("GetSword", LoadBool("GetSwords"));   // 伝説の剣の入手
        SaveBool("GetShield", LoadBool("GetShields"));  // 伝説の盾の入手
        SaveBool("IronSwordEquip", LoadBool("IronSwordEquips"));  // 鉄の剣の装備
        SaveBool("IronShieldEquip", LoadBool("IronShieldEquips")); // 鉄の盾の装備
        SaveBool("GetSwordEquip", LoadBool("GetSwordEquips"));   // 伝説の剣の装備
        SaveBool("GetShieldEquip", LoadBool("GetShieldEquips"));  // 伝説の盾の装備
        SaveBool("Dead", LoadBool("Deads"));
        SaveBool("LastBattle", LoadBool("LastBattles"));
    }

}
