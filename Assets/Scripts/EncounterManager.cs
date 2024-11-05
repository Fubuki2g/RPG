using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterManager : Singleton<EncounterManager>
{
    public float encounterDistance = 100.0f; // �G���J�E���g�����܂ł̈ړ�����
    public GameObject encounterObject; // �G�f�B�^�[��ŃA�^�b�`���ꂽ�G���J�E���g�I�u�W�F�N�g

    private float totalDistance = 0.0f;

    public GameObject defaultBattleComand;
    public GameObject Escape;

    public bool noEncount;

    private void Start()
    {
        encounterObject.SetActive(false);
        if (PlayerPrefs.GetInt("Encount") == 0)
        {
            noEncount = true;
        }
        else
        {
            noEncount = false;
        }
    }

    private void Update()
    {
        if (!noEncount)
        {
            if (PlayerController.Instance.walkable)
            {
                totalDistance += Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")); // �v���C���[�̈ړ��������v�Z
            }

            if (totalDistance >= encounterDistance)
            {
                totalDistance = 0.0f; // �G���J�E���g�����������狗�������Z�b�g
                TriggerEncounter();
                PlayerController.Instance.walkable = false;
            }
        }
    }

    private void TriggerEncounter()
    {
        if (encounterObject != null)
        {
            encounterObject.SetActive(true); // �G���J�E���g�I�u�W�F�N�g��\��
            PlayerBattleController.Instance.BattleMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(defaultBattleComand);
            SoundManager.Instance.PlayBGM(1);
        }
        BattleManager.Instance.BattleStart();
    }

    public void BossEncounter()
    {
        if (encounterObject != null)
        {
            encounterObject.SetActive(true); // �G���J�E���g�I�u�W�F�N�g��\��
            PlayerBattleController.Instance.BattleMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(defaultBattleComand);
            Escape.SetActive(false);
            SoundManager.Instance.PlayBGM(1);
        }
        BattleManager.Instance.BattleStart();
    }
}
