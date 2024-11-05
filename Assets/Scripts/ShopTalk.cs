using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopTalk : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject Yes;
    public GameObject No;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI Name;

    public int ShopNumber;

    private bool isInRange = false;
    public bool buy;

    public float fadeDuration = 1.0f; // �t�F�[�h�̎���
    public Color startColor = new Color(1, 1, 1, 0); // �J�n���̐F (����)
    public Color endColor = new Color(1, 1, 1, 1); // �I�����̐F (�s����)

    public Image image; // �܂��� Renderer renderer;

    private void Start()
    {
        // ������Ԃ̐ݒ�
        if (image != null)
        {
            image.color = startColor;
        }
    }

    void Update()
    {
        if (ShopNumber == 0 && isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable)
        {
            PlayerController.Instance.walkable = false;
            StartCoroutine(Item());

        }
        else if (ShopNumber == 1 && isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable)
        {
            PlayerController.Instance.walkable = false;
            StartCoroutine(Weapon());

        }
        else if (ShopNumber == 2 && isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable)
        {
            PlayerController.Instance.walkable = false;
            StartCoroutine(Armor());

        }
        else if (ShopNumber == 3 && isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable)
        {
            PlayerController.Instance.walkable = false;
            StartCoroutine(Inn());

        }

    }

    public IEnumerator Item()
    {
        dialogue.SetActive(true);
        Name.text = "����X��";
        Text.text = "��������Ⴂ�����͖򑐐��X����I\n�򑐂P�P�O�S�[���h�����ǔ����Ă��������H";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(No);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Yes.SetActive(false);
        No.SetActive(false);
        if (buy && DataManager.Instance.LoadInt("Gold") >= 10)
        {
            Text.text = "�������グ���肪�Ƃ��������܂����`";
            DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 10);
            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") + 1);
        }
        else if (!buy)
        {
            Text.text = "��₩���͂��߂񂾂�I";
        }
        else if (buy && DataManager.Instance.LoadInt("Gold") < 10)
        {
            Text.text = "����������Ȃ���I";
        }

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        buy = false;
        PlayerController.Instance.walkable = true;
    }

    public IEnumerator Weapon()
    {
        if (DataManager.Instance.LoadBool("IronSword"))
        {
            dialogue.SetActive(true);
            Name.text = "���퉮�X��";
            Text.text = "��������؂ꂾ��";
        }
        else
        {
            dialogue.SetActive(true);
            Name.text = "���퉮�X��";
            Text.text = "��������Ⴂ�����͓S�̌����X����I\n�S�̌��P�R�O�O�S�[���h�����ǔ����Ă��������H";

            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            Yes.SetActive(true);
            No.SetActive(true);
            EventSystem.current.SetSelectedGameObject(No);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // �v���C���[���{�^���������̂�҂�
            }
            SoundManager.Instance.PlaySE_Sys(0);

            Yes.SetActive(false);
            No.SetActive(false);
            if (buy && DataManager.Instance.LoadInt("Gold") >= 300)
            {
                Text.text = "�������グ���肪�Ƃ��������܂����`\n��������̂�Y��Ȃ��ł�";
                DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 300);
                DataManager.Instance.SaveBool("IronSword", true);
            }
            else if (!buy)
            {
                Text.text = "��₩���͂��߂񂾂�I";
            }
            else if (buy && DataManager.Instance.LoadInt("Gold") < 300)
            {
                Text.text = "����������Ȃ���I";
            }
        }

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        buy = false;
        PlayerController.Instance.walkable = true;

    }

    public IEnumerator Armor()
    {

        if (DataManager.Instance.LoadBool("IronShield"))
        {
            dialogue.SetActive(true);
            Name.text = "�h��X��";
            Text.text = "��������؂ꂾ��";
        }
        else
        {
            dialogue.SetActive(true);
            Name.text = "�h��X��";
            Text.text = "��������Ⴂ�����͏����X����I\n�S�̏��P�R�O�O�S�[���h�����ǔ����Ă��������H";

            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            Yes.SetActive(true);
            No.SetActive(true);
            EventSystem.current.SetSelectedGameObject(No);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // �v���C���[���{�^���������̂�҂�
            }
            SoundManager.Instance.PlaySE_Sys(0);

            Yes.SetActive(false);
            No.SetActive(false);
            if (buy && DataManager.Instance.LoadInt("Gold") >= 300)
            {
                Text.text = "�������グ���肪�Ƃ��������܂����`\n��������̂�Y��Ȃ��ł�";
                DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 300);
                DataManager.Instance.SaveBool("IronShield", true);
            }
            else if (!buy)
            {
                Text.text = "��₩���͂��߂񂾂�I";
            }
            else if (buy && DataManager.Instance.LoadInt("Gold") < 300)
            {
                Text.text = "����������Ȃ���I";
            }
        }

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        buy = false;
        PlayerController.Instance.walkable = true;

    }

    public IEnumerator Inn()
    {
        dialogue.SetActive(true);
        Name.text = "�h���X��";
        Text.text = "��������Ⴂ�����͏h������I\n�ꔑ�R�O�S�[���h�����ǔ��܂��Ă��������H";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(No);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Yes.SetActive(false);
        No.SetActive(false);
        if (buy && DataManager.Instance.LoadInt("Gold") >= 30)
        {
            Text.text = "������肨�₷��";

            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            StartCoroutine(FadeToColor(image, startColor, endColor, fadeDuration));

            yield return new WaitForSeconds(1.0f); // 1�b�҂�
            Text.text = "";

            StartCoroutine(FadeToColor(image, endColor, startColor, fadeDuration));

            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 30);
            PlayerStatus.Instance.ResetStatus();

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // �v���C���[���{�^���������̂�҂�
            }
            SoundManager.Instance.PlaySE_Sys(0);
            Text.text = "�����Ă�����Ⴂ�I";
        }
        else if (!buy)
        {
            Text.text = "��₩���͂��߂񂾂�I";
        }
        else if (buy && DataManager.Instance.LoadInt("Gold") < 30)
        {
            Text.text = "����������Ȃ���I";
        }

        yield return new WaitForSeconds(1.0f); // 1�b�҂�

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // �v���C���[���{�^���������̂�҂�
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        buy = false;
        PlayerController.Instance.walkable = true;
    }

    public void Shopyes()
    {
        buy = true;
    }

    public void Shopno()
    {
        buy = false;
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

    private IEnumerator FadeToColor(Image targetImage, Color start, Color end, float duration)
    {
        float currentTime = 0.0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(start.a, end.a, currentTime / duration);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, alpha);

            currentTime += Time.deltaTime;
            yield return null;
        }

        targetImage.color = end;
    }

}
