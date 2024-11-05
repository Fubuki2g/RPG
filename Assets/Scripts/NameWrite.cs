using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NameWrite : MonoBehaviour
{
    TMP_InputField _inputField;

    public GameObject selectCanvas;
    public GameObject realyCanvas;

    public GameObject no;
    public GameObject input;

    public TextMeshProUGUI Name;

    void Start()
    {
        realyCanvas.SetActive(false);

        // �}�E�X�J�[�\�����\���ɂ���
        Cursor.visible = false;

        // �}�E�X�̃��b�N���������A�}�E�X���E�B���h�E�O�Ɉړ����Ȃ��悤�ɂ���
        Cursor.lockState = CursorLockMode.Locked;

        _inputField = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
    }

    void Update()
    {

    }

    public void InputName()
    {
        string name = _inputField.text;
        DataManager.Instance.SaveString("PlayerName", name);
        Name.text = DataManager.Instance.LoadString("PlayerName");
        selectCanvas.SetActive(false);
        realyCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(no);
    }

    public void Yes()
    {
        DataManager.Instance.DefaultStatus();
        DataManager.Instance.SaveBool("Tutorial", true);
        SceneManager.LoadScene("CastleScene");
    }

    public void No()
    {
        realyCanvas.SetActive(false);
        selectCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(input);
    }

}