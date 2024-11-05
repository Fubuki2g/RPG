using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // ��: �f�t�H���g�l
    private const int DefaultIntValue = 0;
    private const float DefaultFloatValue = 0.0f;
    private const string DefaultStringValue = "";
    private const bool DefaultBoolValue = false;

    // int�^�̃f�[�^��ۑ�
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    // int�^�̃f�[�^��ǂݍ���
    public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key, DefaultIntValue);
    }

    // float�^�̃f�[�^��ۑ�
    public void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    // float�^�̃f�[�^��ǂݍ���
    public float LoadFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, DefaultFloatValue);
    }

    // string�^�̃f�[�^��ۑ�
    public void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    // string�^�̃f�[�^��ǂݍ���
    public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key, DefaultStringValue);
    }

    // bool�^�̃f�[�^��ۑ�
    public void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    // bool�^�̃f�[�^��ǂݍ���
    public bool LoadBool(string key)
    {
        return PlayerPrefs.GetInt(key, DefaultBoolValue ? 1 : 0) == 1;
    }

    // �͂��߂����I���������ɓǂݍ��ރX�e�[�^�X
    public void DefaultStatus()
    {
        SaveInt("nowLv", 1); // �v���[���[�̌��݃��x��
        SaveInt("nowEXP",0); // �v���C���[�̌��݌o���l 
        SaveInt("upEXP",5);  // �v���C���[�����Ƀ��x���A�b�v����܂ł̕K�v�o���l 
        SaveInt("Gold",30);  // �v���C���[�̏�����
        SaveInt("maxHP",20); // �v���C���[�̍ő�HP
        SaveInt("nowHP",20); // �v���C���[�̌���HP
        SaveInt("maxMP",0);  // �v���C���[�̍ő�MP
        SaveInt("nowMP",0);  // �v���C���[�̌���MP
        SaveInt("ATP",3);    // �v���C���[�̍U����
        SaveInt("DFP",1);    // �v���C���[�̎����
        SaveInt("SPD",3);    // �v���C���[�̂��΂₳
        SaveInt("Leaf", 0);  // �򑐂̌�
        SaveBool("IronSword", false);  // �S�̌��̍w��
        SaveBool("IronShield", false); // �S�̏��̍w��
        SaveBool("GetSword", false);   // �`���̌��̓���
        SaveBool("GetShield", false);  // �`���̏��̓���
        SaveBool("IronSwordEquip", false);  // �S�̌��̑���
        SaveBool("IronShieldEquip", false); // �S�̏��̑���
        SaveBool("GetSwordEquip", false);   // �`���̌��̑���
        SaveBool("GetShieldEquip", false);  // �`���̏��̑���
        SaveBool("Dead", false);
        SaveBool("LastBattle", false);

    }

    public void DataSave()
    {
        SaveInt("nowLvs",LoadInt("nowLv")); // �v���[���[�̌��݃��x��
        SaveInt("nowEXPs", LoadInt("nowEXP")); // �v���C���[�̌��݌o���l 
        SaveInt("upEXPs", LoadInt("upEXP"));  // �v���C���[�����Ƀ��x���A�b�v����܂ł̕K�v�o���l 
        SaveInt("Golds", LoadInt("Gold"));  // �v���C���[�̏�����
        SaveInt("maxHPs", LoadInt("maxHP")); // �v���C���[�̍ő�HP
        SaveInt("nowHPs", LoadInt("nowHP")); // �v���C���[�̌���HP
        SaveInt("maxMPs", LoadInt("maxMP"));  // �v���C���[�̍ő�MP
        SaveInt("nowMPs", LoadInt("nowMP"));  // �v���C���[�̌���MP
        SaveInt("ATPs", LoadInt("ATP"));    // �v���C���[�̍U����
        SaveInt("DFPs", LoadInt("DFP"));    // �v���C���[�̎����
        SaveInt("SPDs", LoadInt("SPD"));    // �v���C���[�̂��΂₳
        SaveInt("Leafs", LoadInt("Leaf"));  // �򑐂̌�
        SaveBool("IronSwords", LoadBool("IronSword"));  // �S�̌��̍w��
        SaveBool("IronShields", LoadBool("IronShield")); // �S�̏��̍w��
        SaveBool("GetSwords", LoadBool("GetSword"));   // �`���̌��̓���
        SaveBool("GetShields", LoadBool("GetShield"));  // �`���̏��̓���
        SaveBool("IronSwordEquips", LoadBool("IronSwordEquip"));  // �S�̌��̑���
        SaveBool("IronShieldEquips", LoadBool("IronShieldEquip")); // �S�̏��̑���
        SaveBool("GetSwordEquips", LoadBool("GetSwordEquip"));   // �`���̌��̑���
        SaveBool("GetShieldEquips", LoadBool("GetShieldEquip"));  // �`���̏��̑���
        SaveBool("Deads", LoadBool("Dead"));
        SaveBool("LastBattles", LoadBool("LastBattle"));
    }

    public void DataLoad()
    {
        SaveInt("nowLv", LoadInt("nowLvs")); // �v���[���[�̌��݃��x��
        SaveInt("nowEXP", LoadInt("nowEXPs")); // �v���C���[�̌��݌o���l 
        SaveInt("upEXP", LoadInt("upEXPs"));  // �v���C���[�����Ƀ��x���A�b�v����܂ł̕K�v�o���l 
        SaveInt("Gold", LoadInt("Golds"));  // �v���C���[�̏�����
        SaveInt("maxHP", LoadInt("maxHPs")); // �v���C���[�̍ő�HP
        SaveInt("nowHP", LoadInt("nowHPs")); // �v���C���[�̌���HP
        SaveInt("maxMP", LoadInt("maxMPs"));  // �v���C���[�̍ő�MP
        SaveInt("nowMP", LoadInt("nowMPs"));  // �v���C���[�̌���MP
        SaveInt("ATP", LoadInt("ATPs"));    // �v���C���[�̍U����
        SaveInt("DFP", LoadInt("DFPs"));    // �v���C���[�̎����
        SaveInt("SPD", LoadInt("SPDs"));    // �v���C���[�̂��΂₳
        SaveInt("Leaf", LoadInt("Leafs"));  // �򑐂̌�
        SaveBool("IronSword", LoadBool("IronSwords"));  // �S�̌��̍w��
        SaveBool("IronShield", LoadBool("IronShields")); // �S�̏��̍w��
        SaveBool("GetSword", LoadBool("GetSwords"));   // �`���̌��̓���
        SaveBool("GetShield", LoadBool("GetShields"));  // �`���̏��̓���
        SaveBool("IronSwordEquip", LoadBool("IronSwordEquips"));  // �S�̌��̑���
        SaveBool("IronShieldEquip", LoadBool("IronShieldEquips")); // �S�̏��̑���
        SaveBool("GetSwordEquip", LoadBool("GetSwordEquips"));   // �`���̌��̑���
        SaveBool("GetShieldEquip", LoadBool("GetShieldEquips"));  // �`���̏��̑���
        SaveBool("Dead", LoadBool("Deads"));
        SaveBool("LastBattle", LoadBool("LastBattles"));
    }

}
