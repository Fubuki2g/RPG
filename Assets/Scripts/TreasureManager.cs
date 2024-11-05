using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureManager : MonoBehaviour
{
    // タグごとのスプライト
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
        // アタッチされているオブジェクトのタグを取得
        string objectTag = gameObject.tag;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // タグに基づいて処理を分岐
        switch (objectTag)
        {
            case "Box1":
                if (!DataManager.Instance.LoadBool("GetSword"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "は宝箱を開けた。\n伝説の剣を手に入れた！";
                    dialogue.SetActive(true);
                    DataManager.Instance.SaveBool("GetSword", true);
                }
                else if (DataManager.Instance.LoadBool("GetSword"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "は宝箱を開けた。\nしかし宝箱は空っぽだった！";
                    dialogue.SetActive(true);
                }
                break;

            case "Box2":
                if (!DataManager.Instance.LoadBool("GetShield"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "は宝箱を開けた。\n伝説の盾を手に入れた！";
                    dialogue.SetActive(true);
                    DataManager.Instance.SaveBool("GetShield", true);
                }
                else if (DataManager.Instance.LoadBool("GetShield"))
                {
                    spriteRenderer.sprite = openBox;
                    Text.text = DataManager.Instance.LoadString("PlayerName") + "は宝箱を開けた。\nしかし宝箱は空っぽだった！";
                    dialogue.SetActive(true);
                }
                break;

            default:
                // 上記のどのケースにも当てはまらない場合のデフォルト処理
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
