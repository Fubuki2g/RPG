using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class KingTalk : Singleton<KingTalk>
{
    public GameObject dialogue;
    public GameObject Yes;
    public GameObject No;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI Name;

    private bool isInRange = false;
    public bool saveyes = false;
    public bool saveno = false;
    public bool cont = false;
    public bool end = false;

    // �_�ł�����Ώ�
    [SerializeField] private Renderer target;
    // �_�Ŏ���[s]
    [SerializeField] private float cycle = 1;

    private double time;


    private void Start()
    {
        Yes.SetActive(false);
        No.SetActive(false);
        EncounterManager.Instance.noEncount = true;

        if(DataManager.Instance.LoadBool("Tutorial"))
        {
            DataManager.Instance.SaveBool("Tutorial", false);
            PlayerController.Instance.walkable = false;
            StartCoroutine(Tutorial());

        }
        if (DataManager.Instance.LoadBool("Dead"))
        {
            DataManager.Instance.SaveBool("Dead", false);
            PlayerController.Instance.walkable = false;
            StartCoroutine(Dead());

        }
    }

    public IEnumerator Tutorial()
    {
        dialogue.SetActive(true);
        Name.text = "���l";
        Text.text = "���̓��A�ɂ���Ƃ����Ă���\n�`���̑�������ɓ����̂���I\n�s���I�E�҂�I";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while(!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        PlayerController.Instance.walkable = true;
        yield break;
    }

    public IEnumerator Dead()
    {
        DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") / 2);
        PlayerStatus.Instance.ResetStatus();
        dialogue.SetActive(true);
        Name.text = "���l";
        Text.text = "�Ȃ�ƁA����ł��܂��Ƃ͏�Ȃ��I";

        yield return new WaitForSeconds(3.0f); // 3�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        PlayerController.Instance.walkable = true;
        yield break;
    }

    private void Update()
    {
        // Enter�L�[�������ꂽ�Ƃ��ɏ������s��
        if (isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable &&( !DataManager.Instance.LoadBool("GetSword") || !DataManager.Instance.LoadBool("GetShield")))
        {
            PlayerController.Instance.walkable = false;
            StartCoroutine(Save());
        }
        else if (isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable && DataManager.Instance.LoadBool("GetSword") && DataManager.Instance.LoadBool("GetShield"))
        {
            PlayerController.Instance.walkable = false;
            DataManager.Instance.SaveBool("LastBattle",true);
            StartCoroutine(Boss());

        }


    }

    public IEnumerator Save()
    {
        Name.text = "���l";
        Text.text = "���ʂ������̃��x���ɂȂ�܂ł���\n" + (DataManager.Instance.LoadInt("upEXP") - DataManager.Instance.LoadInt("nowEXP")) + "�|�C���g�̌o���l���K�v����B";
        dialogue.SetActive(true);

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "�����܂ł̖`�����L�^���邩�H";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Yes);

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        if (saveyes)
        {
            DataManager.Instance.DataSave();
            Text.text = "�L�^�������B\n�܂��`���𑱂��邩�H";
        }
        else if (saveno)
        {
            Text.text = "�������B\n�܂��`���𑱂��邩�H";
        }

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Yes);

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        if (cont)
        {
            Text.text = "�ł͂䂭�������E�҂�I";
            saveyes = false;
            saveno = false;
            cont = false;

            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // �v���C���[���{�^���������̂�҂�
            }
            SoundManager.Instance.PlaySE_Sys(0);
            dialogue.SetActive(false);
            PlayerController.Instance.walkable = true;
            yield break;

        }
        else if (end)
        {
            Text.text = "����ł͋x�ނƂ����B";

            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // �v���C���[���{�^���������̂�҂�
            }
            SoundManager.Instance.PlaySE_Sys(0);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif

            yield break;
        }

    }

    public void selectYes()
    {
        if (saveyes || saveno)
        {
            cont = true;
            Yes.SetActive(false);
            No.SetActive(false);
        }
        else
        {
            saveyes = true;
            Yes.SetActive(false);
            No.SetActive(false);
        }

    }

    public void selectNo()
    {
        if (saveyes || saveno)
        {
            end = true;
            Yes.SetActive(false);
            No.SetActive(false);
        }
        else
        {
            saveno = true;
            Yes.SetActive(false);
            No.SetActive(false);
        }
    }

    public IEnumerator Boss()
    {
        Name.text = "���l";
        Text.text = "�悭���`���̕������ɓ��ꂽ�E�҂�I";
        dialogue.SetActive(true);

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "�������O�̖`���������܂ł̂悤��";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "�����œ`���̕���ƗE�҂�\n�ǂ���������Ȃ�̂�����Ȃ��I";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        EncounterManager.Instance.BossEncounter();
    }

    public void StartEnd()
    {
        StartCoroutine(End());
    }

    public IEnumerator End()
    {
        Name.text = "���l";
        Text.text = "���̂���X�����E�҂�";
        dialogue.SetActive(true);

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "���ӂ�";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        // �����������o�߂�����
        time += Time.deltaTime;

        // ����cycle�ŌJ��Ԃ��l�̎擾
        // 0�`cycle�͈̔͂̒l��������
        var repeatValue = Mathf.Repeat((float)time, cycle);

        // ��������time�ɂ����閾�ŏ�Ԃ𔽉f
        target.enabled = repeatValue >= cycle * 0.5f;

        yield return new WaitForSeconds(2.0f); // 2�b�҂�

        FadeManager.Instance.LoadScene("ClearScene", 1);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
