using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Singleton<SceneChange>
{
    [SerializeField]
    private string sceneName; // エディターで指定するためのフィールド
    [SerializeField]
    private float initialX = 0f; // オブジェクトの初期X座標
    [SerializeField]
    private float initialY = 0f; // オブジェクトの初期Y座標
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
            // シーン移動前に初期位置を保存
            PlayerPrefs.SetFloat("InitialX", initialX);
            PlayerPrefs.SetFloat("InitialY", initialY);
            PlayerPrefs.SetInt("AreaMove", 1);

            // シーンを変更
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
