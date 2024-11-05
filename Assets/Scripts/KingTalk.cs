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

    // 点滅させる対象
    [SerializeField] private Renderer target;
    // 点滅周期[s]
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
        Name.text = "王様";
        Text.text = "東の洞窟にあるといわれている\n伝説の装備を手に入れるのじゃ！\n行け！勇者よ！";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while(!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
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
        Name.text = "王様";
        Text.text = "なんと、死んでしまうとは情けない！";

        yield return new WaitForSeconds(3.0f); // 3秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        PlayerController.Instance.walkable = true;
        yield break;
    }

    private void Update()
    {
        // Enterキーが押されたときに処理を行う
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
        Name.text = "王様";
        Text.text = "おぬしが次のレベルになるまであと\n" + (DataManager.Instance.LoadInt("upEXP") - DataManager.Instance.LoadInt("nowEXP")) + "ポイントの経験値が必要じゃ。";
        dialogue.SetActive(true);

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "ここまでの冒険を記録するか？";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Yes);

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        if (saveyes)
        {
            DataManager.Instance.DataSave();
            Text.text = "記録したぞ。\nまだ冒険を続けるか？";
        }
        else if (saveno)
        {
            Text.text = "そうか。\nまだ冒険を続けるか？";
        }

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Yes);

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        if (cont)
        {
            Text.text = "ではゆくがいい勇者よ！";
            saveyes = false;
            saveno = false;
            cont = false;

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);
            dialogue.SetActive(false);
            PlayerController.Instance.walkable = true;
            yield break;

        }
        else if (end)
        {
            Text.text = "それでは休むといい。";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
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
        Name.text = "王様";
        Text.text = "よくぞ伝説の武具を手に入れた勇者よ！";
        dialogue.SetActive(true);

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "だがお前の冒険もここまでのようだ";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "ここで伝説の武具と勇者の\nどちらも無くなるのだからなあ！";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
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
        Name.text = "王様";
        Text.text = "おのれ忌々しい勇者め";
        dialogue.SetActive(true);

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Text.text = "ぐふっ";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        // 内部時刻を経過させる
        time += Time.deltaTime;

        // 周期cycleで繰り返す値の取得
        // 0～cycleの範囲の値が得られる
        var repeatValue = Mathf.Repeat((float)time, cycle);

        // 内部時刻timeにおける明滅状態を反映
        target.enabled = repeatValue >= cycle * 0.5f;

        yield return new WaitForSeconds(2.0f); // 2秒待つ

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
