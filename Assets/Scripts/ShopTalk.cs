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

    public float fadeDuration = 1.0f; // フェードの時間
    public Color startColor = new Color(1, 1, 1, 0); // 開始時の色 (透明)
    public Color endColor = new Color(1, 1, 1, 1); // 終了時の色 (不透明)

    public Image image; // または Renderer renderer;

    private void Start()
    {
        // 初期状態の設定
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
        Name.text = "道具屋店主";
        Text.text = "いらっしゃいここは薬草専門店だよ！\n薬草１つ１０ゴールドだけど買っていくかい？";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(No);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Yes.SetActive(false);
        No.SetActive(false);
        if (buy && DataManager.Instance.LoadInt("Gold") >= 10)
        {
            Text.text = "お買い上げありがとうございました～";
            DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 10);
            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") + 1);
        }
        else if (!buy)
        {
            Text.text = "冷やかしはごめんだよ！";
        }
        else if (buy && DataManager.Instance.LoadInt("Gold") < 10)
        {
            Text.text = "お金が足りないよ！";
        }

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
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
            Name.text = "武器屋店主";
            Text.text = "もう売り切れだよ";
        }
        else
        {
            dialogue.SetActive(true);
            Name.text = "武器屋店主";
            Text.text = "いらっしゃいここは鉄の剣専門店だよ！\n鉄の剣１つ３００ゴールドだけど買っていくかい？";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            Yes.SetActive(true);
            No.SetActive(true);
            EventSystem.current.SetSelectedGameObject(No);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            Yes.SetActive(false);
            No.SetActive(false);
            if (buy && DataManager.Instance.LoadInt("Gold") >= 300)
            {
                Text.text = "お買い上げありがとうございました～\n装備するのを忘れないでね";
                DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 300);
                DataManager.Instance.SaveBool("IronSword", true);
            }
            else if (!buy)
            {
                Text.text = "冷やかしはごめんだよ！";
            }
            else if (buy && DataManager.Instance.LoadInt("Gold") < 300)
            {
                Text.text = "お金が足りないよ！";
            }
        }

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
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
            Name.text = "防具屋店主";
            Text.text = "もう売り切れだよ";
        }
        else
        {
            dialogue.SetActive(true);
            Name.text = "防具屋店主";
            Text.text = "いらっしゃいここは盾専門店だよ！\n鉄の盾１つ３００ゴールドだけど買っていくかい？";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            Yes.SetActive(true);
            No.SetActive(true);
            EventSystem.current.SetSelectedGameObject(No);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            Yes.SetActive(false);
            No.SetActive(false);
            if (buy && DataManager.Instance.LoadInt("Gold") >= 300)
            {
                Text.text = "お買い上げありがとうございました～\n装備するのを忘れないでね";
                DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 300);
                DataManager.Instance.SaveBool("IronShield", true);
            }
            else if (!buy)
            {
                Text.text = "冷やかしはごめんだよ！";
            }
            else if (buy && DataManager.Instance.LoadInt("Gold") < 300)
            {
                Text.text = "お金が足りないよ！";
            }
        }

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        dialogue.SetActive(false);
        buy = false;
        PlayerController.Instance.walkable = true;

    }

    public IEnumerator Inn()
    {
        dialogue.SetActive(true);
        Name.text = "宿屋店主";
        Text.text = "いらっしゃいここは宿屋だよ！\n一泊３０ゴールドだけど泊まっていくかい？";

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        Yes.SetActive(true);
        No.SetActive(true);
        EventSystem.current.SetSelectedGameObject(No);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
        }
        SoundManager.Instance.PlaySE_Sys(0);

        Yes.SetActive(false);
        No.SetActive(false);
        if (buy && DataManager.Instance.LoadInt("Gold") >= 30)
        {
            Text.text = "ゆっくりおやすみ";

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            StartCoroutine(FadeToColor(image, startColor, endColor, fadeDuration));

            yield return new WaitForSeconds(1.0f); // 1秒待つ
            Text.text = "";

            StartCoroutine(FadeToColor(image, endColor, startColor, fadeDuration));

            yield return new WaitForSeconds(1.0f); // 1秒待つ

            DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") - 30);
            PlayerStatus.Instance.ResetStatus();

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);
            Text.text = "いってらっしゃい！";
        }
        else if (!buy)
        {
            Text.text = "冷やかしはごめんだよ！";
        }
        else if (buy && DataManager.Instance.LoadInt("Gold") < 30)
        {
            Text.text = "お金が足りないよ！";
        }

        yield return new WaitForSeconds(1.0f); // 1秒待つ

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null; // プレイヤーがボタンを押すのを待つ
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
