using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Singleton<SceneChange>
{
    [SerializeField]
    private string sceneName; // �G�f�B�^�[�Ŏw�肷�邽�߂̃t�B�[���h
    [SerializeField]
    private float initialX = 0f; // �I�u�W�F�N�g�̏���X���W
    [SerializeField]
    private float initialY = 0f; // �I�u�W�F�N�g�̏���Y���W
    [SerializeField]
    public bool NoEncount = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (NoEncount)
        {
            PlayerPrefs.SetInt("Encount", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Encount", 1);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.walkable = false;
            // �V�[���ړ��O�ɏ����ʒu��ۑ�
            PlayerPrefs.SetFloat("InitialX", initialX);
            PlayerPrefs.SetFloat("InitialY", initialY);
            PlayerPrefs.SetInt("AreaMove", 1);

            // �V�[����ύX
            FadeManager.Instance.LoadScene(sceneName,1);
        }
    }

    public void scenechange(string Scene)
    {
        FadeManager.Instance.LoadScene(Scene, 1);
    }

    public void SceneChangeName()
    {
        FadeManager.Instance.LoadScene("NameSelectScene", 1);
    }

    public void SceneChangeLoad()
    {
        DataManager.Instance.DataLoad();
        FadeManager.Instance.LoadScene("CastleScene",1);
    }

    public void SceneChangeTitle()
    {
        FadeManager.Instance.LoadScene("TitleScene", 1);
    }
}
