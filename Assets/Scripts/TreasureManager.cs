using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureManager : MonoBehaviour
{
    // �^�O���Ƃ̃X�v���C�g
    public GameObject dialogue;
    public TextMeshProUGUI Text;
    public Sprite openBox;
    public bool isInRange = false;
    public bool itemgot = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (isInRange)
            {
                PlayerController.Instance.walkable = false;
                PerformTagBasedAction();
                itemgot = true;
                isInRange = false;
            }
            else if (itemgot)
            {
                PlayerController.Instance.walkable = true;
                dialogue.SetActive(false);
                itemgot = false;
            }
        }
    }

    void PerformTagBasedAction()
    {
        // �A�^�b�`����Ă���I�u�W�F�N�g�̃^�O���擾
        string objectTag = gameObject.tag;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // �^�O�Ɋ�Â��ď����𕪊�
        switch (objectTag)
        {
            case "Box1":
                if (!DataManager.Instance.LoadBool("GetSword"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "�͕󔠂��J�����B\n�`���̌�����ɓ��ꂽ�I";
                    dialogue.SetActive(true);
                    DataManager.Instance.SaveBool("GetSword", true);
                }
                else if (DataManager.Instance.LoadBool("GetSword"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "�͕󔠂��J�����B\n�������󔠂͋���ۂ������I";
                    dialogue.SetActive(true);
                }
                break;

            case "Box2":
                if (!DataManager.Instance.LoadBool("GetShield"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "�͕󔠂��J�����B\n�`���̏�����ɓ��ꂽ�I";
                    dialogue.SetActive(true);
                    DataManager.Instance.SaveBool("GetShield", true);
                }
                else if (DataManager.Instance.LoadBool("GetShield"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "�͕󔠂��J�����B\n�������󔠂͋���ۂ������I";
                    dialogue.SetActive(true);
                }
                break;

            default:
                // ��L�̂ǂ̃P�[�X�ɂ����Ă͂܂�Ȃ��ꍇ�̃f�t�H���g����
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

}
