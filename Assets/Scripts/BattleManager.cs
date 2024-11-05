using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BattleManager : Singleton<BattleManager>
{
    public Sprite[] Enemys; // エディターで3つのスプライトを指定
    public GameObject EnemySprite;

    public string[] RandomEName;
    public TextMeshProUGUI firsttext;
    public TextMeshProUGUI secondtext;
    public TextMeshProUGUI thirdtext;
    public GameObject firsttextobj;
    public GameObject secondtextobj;
    public GameObject thirdtextobj;

    private Image imageComponent;

    public bool battleStart = false;
    public bool comandon = false;
    public bool battleend = false;

    // コルーチンを実行中かどうかを示すフラグ
    private bool isRunningCoroutine = false;

    private void Update()
    {
        if (battleend && Input.GetKeyDown(KeyCode.Return))
        {
            EncounterManager.Instance.encounterObject.SetActive(false);
            PlayerController.Instance.walkable = true;
            battleend = false;

            if (DataManager.Instance.LoadBool("LastBattle"))
            {
                PlayerController.Instance.walkable = false;
                DataManager.Instance.SaveBool("LastBattle", false);
                KingTalk.Instance.StartEnd();
            }

        }
    }

    public void BattleStart()
    {
        firsttextobj.SetActive(false);
        secondtextobj.SetActive(false);
        thirdtextobj.SetActive(false);
        // EnemySpriteオブジェクトからImageコンポーネントを取得
        imageComponent = EnemySprite.GetComponent<Image>();

        // Enemysの中からランダムに1つのSpriteを選ぶ
        int randomIndex = Random.Range(0, 3);
        Sprite selectedSprite = Enemys[randomIndex];

        // Imageコンポーネントに選ばれたSpriteを設定
        imageComponent.sprite = selectedSprite;

        firsttext.text = RandomEName[randomIndex] + "があらわれた！";
        secondtext.text = "コマンド？";
        firsttextobj.SetActive(true);
        secondtextobj.SetActive(true);
        battleStart = true;

        if (selectedSprite.name == "sraim")
        {
            EnemyStatusManager.Instance.Slime();
        }
        else if (selectedSprite.name == "alraune")
        {
            EnemyStatusManager.Instance.Alraune();
        }
        else if (selectedSprite.name == "kinoko")
        {
            EnemyStatusManager.Instance.Kinoko();
        }
        else if (selectedSprite.name == "ahriman")
        {
            EnemyStatusManager.Instance.Ahriman();
        }
        else if (selectedSprite.name == "bomb")
        {
            EnemyStatusManager.Instance.Bomb();
        }
        else if (selectedSprite.name == "ghost")
        {
            EnemyStatusManager.Instance.Ghost();
        }
        else if (selectedSprite.name == "ricchi")
        {
            EnemyStatusManager.Instance.Ricchi();
        }


    }

    // コルーチンを開始するメソッド
    public void StartYourSequence()
    {
        if (!isRunningCoroutine)
        {
            StartCoroutine(BattleComand());
        }
    }

    public IEnumerator BattleComand()
    {
        if (PlayerBattleController.Instance.atack)
        {
            PlayerBattleController.Instance.atack = false;
            firsttextobj.SetActive(false);
            secondtextobj.SetActive(false);
            thirdtextobj.SetActive(false);
            // プレイヤーの素早さが敵より早い場合
            if (DataManager.Instance.LoadInt("SPD") > EnemyStatusManager.Instance.Espeed)
            {
                SoundManager.Instance.PlaySE_Sys(3);
                firsttext.text = DataManager.Instance.LoadString("PlayerName") + "の攻撃";
                firsttextobj.SetActive(true);
                PlayerStatus.Instance.GiveDamage();
                secondtext.text = EnemyStatusManager.Instance.Ename + "に" + PlayerStatus.Instance.gdamage + "のダメージ";
                secondtextobj.SetActive(true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                    {
                        SoundManager.Instance.PlaySE_Sys(2);
                        PlayerStatus.Instance.LevelUp();
                        firsttextobj.SetActive(false);
                        secondtextobj.SetActive(false);
                        thirdtextobj.SetActive(false);
                        firsttextobj.SetActive(true);
                        secondtextobj.SetActive(true);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        secondtext.text = "レベルが上がった！";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ヒールを覚えた！";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ファイアを覚えた！";
                        }

                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);
                
                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();
               
                
            }
            // 敵の素早さがプレイヤーより早い場合
            else if (DataManager.Instance.LoadInt("SPD") < EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                SoundManager.Instance.PlaySE_Sys(3);
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                firsttext.text = DataManager.Instance.LoadString("PlayerName") + "の攻撃";
                firsttextobj.SetActive(true);
                PlayerStatus.Instance.GiveDamage();
                secondtext.text = EnemyStatusManager.Instance.Ename + "に" + PlayerStatus.Instance.gdamage + "のダメージ";
                secondtextobj.SetActive(true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                    {
                        SoundManager.Instance.PlaySE_Sys(2);
                        PlayerStatus.Instance.LevelUp();
                        firsttextobj.SetActive(false);
                        secondtextobj.SetActive(false);
                        thirdtextobj.SetActive(false);
                        firsttextobj.SetActive(true);
                        secondtextobj.SetActive(true);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        secondtext.text = "レベルが上がった！";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ヒールを覚えた！";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ファイアを覚えた！";
                        }

                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                nextcomand();
            }
            // プレイヤーと敵の素早さが同じ場合
            else if (DataManager.Instance.LoadInt("SPD") == EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    SoundManager.Instance.PlaySE_Sys(3);
                    firsttext.text = DataManager.Instance.LoadString("PlayerName") + "の攻撃";
                    firsttextobj.SetActive(true);
                    PlayerStatus.Instance.GiveDamage();
                    secondtext.text = EnemyStatusManager.Instance.Ename + "に" + PlayerStatus.Instance.gdamage + "のダメージ";
                    secondtextobj.SetActive(true);

                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                        if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                        {
                            SoundManager.Instance.PlaySE_Sys(2);
                            PlayerStatus.Instance.LevelUp();
                            firsttextobj.SetActive(false);
                            secondtextobj.SetActive(false);
                            thirdtextobj.SetActive(false);
                            firsttextobj.SetActive(true);
                            secondtextobj.SetActive(true);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            secondtext.text = "レベルが上がった！";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ヒールを覚えた！";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ファイアを覚えた！";
                            }

                            yield return new WaitForSeconds(1.0f); // 1秒待つ

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // プレイヤーがボタンを押すのを待つ
                            }
                            SoundManager.Instance.PlaySE_Sys(0);

                        }

                        battleend = true;
                        yield break;
                    }

                    if (randomNumber == 1)
                    {
                        yield return StartCoroutine(NormalComand());
                    }
                    if (randomNumber == 2)
                    {
                        yield return StartCoroutine(SpecialComand());
                    }

                    if (battleend)
                    {
                        yield break;
                    }

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1秒待つ

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    nextcomand();
                }
                else if (randomNumber == 2)
                {
                    if (randomNumber == 1)
                    {
                        yield return StartCoroutine(NormalComand());
                    }
                    if (randomNumber == 2)
                    {
                        yield return StartCoroutine(SpecialComand());
                    }

                    if (battleend)
                    {
                        yield break;
                    }

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1秒待つ

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    SoundManager.Instance.PlaySE_Sys(3);
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    firsttext.text = DataManager.Instance.LoadString("PlayerName") + "の攻撃";
                    firsttextobj.SetActive(true);
                    PlayerStatus.Instance.GiveDamage();
                    secondtext.text = EnemyStatusManager.Instance.Ename + "に" + PlayerStatus.Instance.gdamage + "のダメージ";
                    secondtextobj.SetActive(true);

                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                        if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                        {
                            SoundManager.Instance.PlaySE_Sys(2);
                            PlayerStatus.Instance.LevelUp();
                            firsttextobj.SetActive(false);
                            secondtextobj.SetActive(false);
                            thirdtextobj.SetActive(false);
                            firsttextobj.SetActive(true);
                            secondtextobj.SetActive(true);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            secondtext.text = "レベルが上がった！";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ヒールを覚えた！";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ファイアを覚えた！";
                            }

                            yield return new WaitForSeconds(1.0f); // 1秒待つ

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // プレイヤーがボタンを押すのを待つ
                            }
                            SoundManager.Instance.PlaySE_Sys(0);

                        }

                        battleend = true;
                        yield break;
                    }

                    nextcomand();
                }
            }

        }
        else if (PlayerBattleController.Instance.magic)
        {
            PlayerBattleController.Instance.magic = false;
            firsttextobj.SetActive(false);
            secondtextobj.SetActive(false);
            thirdtextobj.SetActive(false);

            // プレイヤーのほうが素早さが速い場合
            if (DataManager.Instance.LoadInt("SPD") > EnemyStatusManager.Instance.Espeed)
            {
                if (PlayerBattleController.Instance.heal)
                {
                    if (DataManager.Instance.LoadInt("nowMP") < 3)
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "しかしMPが足りない！";
                        secondtextobj.SetActive(true);
                    }
                    else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "しかし何も起こらなかった";
                        secondtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "HPを30回復した";
                        secondtextobj.SetActive(true);
                        PlayerStatus.Instance.ConsumeMP(3);
                        PlayerStatus.Instance.Heal(30);
                    }
                    PlayerBattleController.Instance.heal = false;
                }
                else if (PlayerBattleController.Instance.fire)
                {
                    if (DataManager.Instance.LoadInt("nowMP") < 3)
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はファイアを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "ファイアを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "しかしMPがたりない！";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(7);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        firsttextobj.SetActive(true);
                        secondtext.text = "ファイアを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = EnemyStatusManager.Instance.Ename + "に" + 20 + "のダメージ";
                        thirdtextobj.SetActive(true);
                        PlayerStatus.Instance.ConsumeMP(3);
                        PlayerStatus.Instance.GiveMagicDamage(20);
                    }
                    PlayerBattleController.Instance.fire = false;
                }

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                    {
                        SoundManager.Instance.PlaySE_Sys(2);
                        PlayerStatus.Instance.LevelUp();
                        firsttextobj.SetActive(false);
                        secondtextobj.SetActive(false);
                        thirdtextobj.SetActive(false);
                        firsttextobj.SetActive(true);
                        secondtextobj.SetActive(true);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        secondtext.text = "レベルが上がった！";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ヒールを覚えた！";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ファイアを覚えた！";
                        }

                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();

            }
            // 敵のほうが素早さが速い場合
            else if (DataManager.Instance.LoadInt("SPD") < EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                if (PlayerBattleController.Instance.heal)
                {
                    if (DataManager.Instance.LoadInt("nowMP") < 3)
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "しかしMPが足りない！";
                        secondtextobj.SetActive(true);
                    }
                    else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "しかし何も起こらなかった";
                        secondtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "HPを30回復した";
                        secondtextobj.SetActive(true);
                        PlayerStatus.Instance.ConsumeMP(3);
                        PlayerStatus.Instance.Heal(30);
                    }
                    PlayerBattleController.Instance.heal = false;
                }
                else if (PlayerBattleController.Instance.fire)
                {
                    if (DataManager.Instance.LoadInt("nowMP") < 3)
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はファイアを使った";
                        firsttextobj.SetActive(true);
                        secondtext.text = "ファイアを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "しかしMPがたりない！";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(7);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        firsttextobj.SetActive(true);
                        secondtext.text = "ファイアを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = EnemyStatusManager.Instance.Ename + "に" + 20 + "のダメージ";
                        thirdtextobj.SetActive(true);
                        PlayerStatus.Instance.ConsumeMP(3);
                        PlayerStatus.Instance.GiveMagicDamage(20);
                    }
                    PlayerBattleController.Instance.fire = false;
                }

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }

                    if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                    {
                        SoundManager.Instance.PlaySE_Sys(2);
                        PlayerStatus.Instance.LevelUp();
                        firsttextobj.SetActive(false);
                        secondtextobj.SetActive(false);
                        thirdtextobj.SetActive(false);
                        firsttextobj.SetActive(true);
                        secondtextobj.SetActive(true);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        secondtext.text = "レベルが上がった！";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ヒールを覚えた！";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ファイアを覚えた！";
                        }

                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                nextcomand();
            }
            // プレイヤーと敵の素早さが同じ場合
            else if (DataManager.Instance.LoadInt("SPD") == EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    if (PlayerBattleController.Instance.heal)
                    {
                        if (DataManager.Instance.LoadInt("nowMP") < 3)
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "しかしMPが足りない！";
                            secondtextobj.SetActive(true);
                        }
                        else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "しかし何も起こらなかった";
                            secondtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "HPを30回復した";
                            secondtextobj.SetActive(true);
                            PlayerStatus.Instance.ConsumeMP(3);
                            PlayerStatus.Instance.Heal(30);
                        }
                        PlayerBattleController.Instance.heal = false;
                    }
                    else if (PlayerBattleController.Instance.fire)
                    {
                        if (DataManager.Instance.LoadInt("nowMP") < 3)
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はファイアを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "ファイアを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "しかしMPがたりない！";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(7);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            firsttextobj.SetActive(true);
                            secondtext.text = "ファイアを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = EnemyStatusManager.Instance.Ename + "に" + 20 + "のダメージ";
                            thirdtextobj.SetActive(true);
                            PlayerStatus.Instance.ConsumeMP(3);
                            PlayerStatus.Instance.GiveMagicDamage(20);
                        }
                        PlayerBattleController.Instance.fire = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                        if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                        {
                            SoundManager.Instance.PlaySE_Sys(2);
                            PlayerStatus.Instance.LevelUp();
                            firsttextobj.SetActive(false);
                            secondtextobj.SetActive(false);
                            thirdtextobj.SetActive(false);
                            firsttextobj.SetActive(true);
                            secondtextobj.SetActive(true);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            secondtext.text = "レベルが上がった！";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ヒールを覚えた！";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ファイアを覚えた！";
                            }

                            yield return new WaitForSeconds(1.0f); // 1秒待つ

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // プレイヤーがボタンを押すのを待つ
                            }
                            SoundManager.Instance.PlaySE_Sys(0);

                        }

                        battleend = true;
                        yield break;
                    }

                    if (randomNumber == 1)
                    {
                        yield return StartCoroutine(NormalComand());
                    }
                    if (randomNumber == 2)
                    {
                        yield return StartCoroutine(SpecialComand());
                    }

                    if (battleend)
                    {
                        yield break;
                    }

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1秒待つ

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    nextcomand();
                }
                else if (randomNumber == 2)
                {
                    if (randomNumber == 1)
                    {
                        yield return StartCoroutine(NormalComand());
                    }
                    if (randomNumber == 2)
                    {
                        yield return StartCoroutine(SpecialComand());
                    }

                    if (battleend)
                    {
                        yield break;
                    }

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1秒待つ

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    if (PlayerBattleController.Instance.heal)
                    {
                        if (DataManager.Instance.LoadInt("nowMP") < 3)
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "しかしMPが足りない！";
                            secondtextobj.SetActive(true);
                        }
                        else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "しかし何も起こらなかった";
                            secondtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はヒールを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "HPを30回復した";
                            secondtextobj.SetActive(true);
                            PlayerStatus.Instance.ConsumeMP(3);
                            PlayerStatus.Instance.Heal(30);
                        }
                        PlayerBattleController.Instance.heal = false;

                    }
                    else if (PlayerBattleController.Instance.fire)
                    {
                        if (DataManager.Instance.LoadInt("nowMP") < 3)
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はファイアを使った";
                            firsttextobj.SetActive(true);
                            secondtext.text = "ファイアを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "しかしMPがたりない！";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(7);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            firsttextobj.SetActive(true);
                            secondtext.text = "ファイアを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = EnemyStatusManager.Instance.Ename + "に" + 20 + "のダメージ";
                            thirdtextobj.SetActive(true);
                            PlayerStatus.Instance.ConsumeMP(3);
                            PlayerStatus.Instance.GiveMagicDamage(20);
                        }
                        PlayerBattleController.Instance.fire = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                        if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                        {
                            SoundManager.Instance.PlaySE_Sys(2);
                            PlayerStatus.Instance.LevelUp();
                            firsttextobj.SetActive(false);
                            secondtextobj.SetActive(false);
                            thirdtextobj.SetActive(false);
                            firsttextobj.SetActive(true);
                            secondtextobj.SetActive(true);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            secondtext.text = "レベルが上がった！";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ヒールを覚えた！";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "ファイアを覚えた！";
                            }

                            yield return new WaitForSeconds(1.0f); // 1秒待つ

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // プレイヤーがボタンを押すのを待つ
                            }
                            SoundManager.Instance.PlaySE_Sys(0);

                        }

                        battleend = true;
                        yield break;
                    }

                    nextcomand();

                }
            }

        }
        else if (PlayerBattleController.Instance.item)
        {
            PlayerBattleController.Instance.item = false;
            firsttextobj.SetActive(false);
            secondtextobj.SetActive(false);
            thirdtextobj.SetActive(false);

            // プレイヤーのほうが素早さが速い場合
            if (DataManager.Instance.LoadInt("SPD") > EnemyStatusManager.Instance.Espeed)
            {
                if (PlayerBattleController.Instance.leaf)
                {
                    if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        firsttextobj.SetActive(true);
                        secondtext.text = "やくそうを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "しかし何も起こらなかった";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        firsttextobj.SetActive(true);
                        secondtext.text = "やくそうを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "HPを30回復した";
                        thirdtextobj.SetActive(true);
                        DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                        PlayerStatus.Instance.Heal(30);
                    }
                    PlayerBattleController.Instance.leaf = false;
                }
                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();

            }
            // 敵のほうが素早さが速い場合
            else if (DataManager.Instance.LoadInt("SPD") < EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                if (PlayerBattleController.Instance.leaf)
                {
                    if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        firsttextobj.SetActive(true);
                        secondtext.text = "やくそうを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "しかし何も起こらなかった";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        firsttextobj.SetActive(true);
                        secondtext.text = "やくそうを使った";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "HPを30回復した";
                        thirdtextobj.SetActive(true);
                        DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                        PlayerStatus.Instance.Heal(30);
                    }
                    PlayerBattleController.Instance.leaf = false;
                }

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                nextcomand();
            }
            // プレイヤーと敵の素早さが同じ場合
            else if (DataManager.Instance.LoadInt("SPD") == EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
                if (randomNumber == 1)
                {
                    if (PlayerBattleController.Instance.leaf)
                    {
                        if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            firsttextobj.SetActive(true);
                            secondtext.text = "やくそうを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "しかし何も起こらなかった";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            firsttextobj.SetActive(true);
                            secondtext.text = "やくそうを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "HPを30回復した";
                            thirdtextobj.SetActive(true);
                            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                            PlayerStatus.Instance.Heal(30);
                        }
                        PlayerBattleController.Instance.leaf = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (randomNumber == 1)
                    {
                        yield return StartCoroutine(NormalComand());
                    }
                    if (randomNumber == 2)
                    {
                        yield return StartCoroutine(SpecialComand());
                    }

                    if (battleend)
                    {
                        yield break;
                    }

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();
                        yield break;
                    }

                    nextcomand();
                }
                else if (randomNumber == 2)
                {
                    if (randomNumber == 1)
                    {
                        yield return StartCoroutine(NormalComand());
                    }
                    if (randomNumber == 2)
                    {
                        yield return StartCoroutine(SpecialComand());
                    }

                    if (battleend)
                    {
                        yield break;
                    }

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1秒待つ

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    if (PlayerBattleController.Instance.leaf)
                    {
                        if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            firsttextobj.SetActive(true);
                            secondtext.text = "やくそうを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "しかし何も起こらなかった";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                            firsttextobj.SetActive(true);
                            secondtext.text = "やくそうを使った";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "HPを30回復した";
                            thirdtextobj.SetActive(true);
                            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                            PlayerStatus.Instance.Heal(30);
                        }
                        PlayerBattleController.Instance.leaf = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    nextcomand();

                }
            }

        }
        else if (PlayerBattleController.Instance.escape)
        {
            PlayerBattleController.Instance.escape = false;
            firsttextobj.SetActive(false);
            secondtextobj.SetActive(false);
            thirdtextobj.SetActive(false);
            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はにげだした";
            firsttextobj.SetActive(true);
            yield return new WaitForSeconds(1.0f); // 1秒待つ

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // プレイヤーがボタンを押すのを待つ
            }
            SoundManager.Instance.PlaySE_Sys(0);

            firsttextobj.SetActive(false);
            int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
            if (randomNumber == 1)
            {
                SoundManager.Instance.PlayBGM(0);
                SoundManager.Instance.PlaySE_Sys(6);
                firsttext.text = "うまくにげきった";
                firsttextobj.SetActive(true);
                battleend = true;
            }
            if (randomNumber == 2)
            {
                firsttext.text = "しかしまわりこまれた";
                firsttextobj.SetActive(true);

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (randomNumber == 1)
                {
                    yield return StartCoroutine(NormalComand());
                }
                if (randomNumber == 2)
                {
                    yield return StartCoroutine(SpecialComand());
                }

                if (battleend)
                {
                    yield break;
                }

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();
            }

        }
    }

    public void nextcomand()
    {
        firsttextobj.SetActive(false);
        secondtextobj.SetActive(false);
        thirdtextobj.SetActive(false);
        firsttext.text = "コマンド？";
        firsttextobj.SetActive(true);
        PlayerBattleController.Instance.BattleMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(EncounterManager.Instance.defaultBattleComand);

    }

    public void BattleEnd()
    {
        SoundManager.Instance.PlaySE_Sys(1);
        SoundManager.Instance.PlayBGM(0);
        firsttextobj.SetActive(false);
        secondtextobj.SetActive(false);
        thirdtextobj.SetActive(false);
        imageComponent.sprite = Enemys[3];
        DataManager.Instance.SaveInt("nowEXP", DataManager.Instance.LoadInt("nowEXP") + EnemyStatusManager.Instance.Eexp);
        DataManager.Instance.SaveInt("Gold", DataManager.Instance.LoadInt("Gold") + EnemyStatusManager.Instance.Egold);
        firsttext.text = EnemyStatusManager.Instance.Ename + "をたおした";
        secondtext.text = EnemyStatusManager.Instance.Eexp + "の経験値と";
        thirdtext.text = EnemyStatusManager.Instance.Egold + "ゴールドを手に入れた";
        firsttextobj.SetActive(true);
        secondtextobj.SetActive(true);
        thirdtextobj.SetActive(true);

    }

    public void PlayerDead()
    {
        SoundManager.Instance.PlayBGM(2);
        firsttextobj.SetActive(false);
        secondtextobj.SetActive(false);
        thirdtextobj.SetActive(false);
        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "はしんでしまった";
        firsttextobj.SetActive(true);
        DataManager.Instance.SaveBool("Dead", true);

    }
    public IEnumerator NormalComand()
    {
        SoundManager.Instance.PlaySE_Sys(3);
        firsttextobj.SetActive(true);
        secondtextobj.SetActive(true);
        firsttext.text = EnemyStatusManager.Instance.Ename + "の攻撃";
        PlayerStatus.Instance.ReceiveDamage();
        secondtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + PlayerStatus.Instance.rdamage + "のダメージ";

        yield return new WaitForSeconds(1.0f); // 1秒待つ
    }

    public IEnumerator SpecialComand()
    {
        if (EnemyStatusManager.Instance.Ename == "スライム")
        {
            firsttextobj.SetActive(true);
            secondtextobj.SetActive(false);
            firsttext.text = EnemyStatusManager.Instance.Ename + "は遊んでいる";

        }
        else if (EnemyStatusManager.Instance.Ename == "アルラウネ")
        {
            if (EnemyStatusManager.Instance.EcurrentMP >= 5)
            {
                SoundManager.Instance.PlaySE_Sys(9);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は";
                secondtext.text = "リーフを唱えた";
                PlayerStatus.Instance.ReceiveMagicDamage(10);
                EnemyStatusManager.Instance.EcurrentMP -= 5;
                thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 10 + "のダメージ";
            }
            else
            {
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は";
                secondtext.text = "リーフを唱えた";
                thirdtext.text = "しかしMPが足りない！";
            }

        }
        else if (EnemyStatusManager.Instance.Ename == "キノコ")
        {
            int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
            if (randomNumber == 1)
            {
                SoundManager.Instance.PlaySE_Sys(8);
                firsttextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は突進してきた";
                PlayerStatus.Instance.ReceiveMagicDamage(10);
                secondtextobj.SetActive(true);
                secondtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 10 + "のダメージ";

            }
            if (randomNumber == 2)
            {
                firsttextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は突進してきた";
                secondtextobj.SetActive(true);
                secondtext.text = "しかしこうげきは外れた";

            }

        }
        else if (EnemyStatusManager.Instance.Ename == "アーリマン")
        {
            if (EnemyStatusManager.Instance.EcurrentMP >= 3)
            {
                SoundManager.Instance.PlaySE_Sys(8);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は";
                secondtext.text = "アイビームを放った";
                PlayerStatus.Instance.ReceiveMagicDamage(20);
                EnemyStatusManager.Instance.EcurrentMP -= 3;
                thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 20 + "のダメージ";
            }
            else
            {
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は";
                secondtext.text = "アイビームを放った";
                thirdtext.text = "しかしMPが足りない！";
            }
        }
        else if (EnemyStatusManager.Instance.Ename == "ボム")
        {
            int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
            if (randomNumber == 1)
            {
                SoundManager.Instance.PlaySE_Sys(9);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は自爆した";
                PlayerStatus.Instance.ReceiveMagicDamage(50);
                secondtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 50 + "のダメージ";

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                imageComponent.sprite = Enemys[3];
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(false);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は砕け散った";
                EnemyStatusManager.Instance.EcurrentHP = 0;

                yield return new WaitForSeconds(1.0f); // 1秒待つ

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // プレイヤーがボタンを押すのを待つ
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1秒待つ

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }
                else if (DataManager.Instance.LoadInt("nowHP") > 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1秒待つ

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // プレイヤーがボタンを押すのを待つ
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowEXP") >= DataManager.Instance.LoadInt("upEXP"))
                    {
                        SoundManager.Instance.PlaySE_Sys(2);
                        PlayerStatus.Instance.LevelUp();
                        firsttextobj.SetActive(false);
                        secondtextobj.SetActive(false);
                        thirdtextobj.SetActive(false);
                        firsttextobj.SetActive(true);
                        secondtextobj.SetActive(true);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "は";
                        secondtext.text = "レベルが上がった！";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ヒールを覚えた！";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "ファイアを覚えた！";
                        }

                        yield return new WaitForSeconds(1.0f); // 1秒待つ

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // プレイヤーがボタンを押すのを待つ
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

            }
            if (randomNumber == 2)
            {
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は自爆した";
                secondtext.text = "しかし何も起こらなかった";

            }
        }
        else if (EnemyStatusManager.Instance.Ename == "ゴースト")
        {
            int randomNumber = Random.Range(1, 3); // 1から2までのランダムな整数を生成
            if (randomNumber == 1)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 5)
                {
                    SoundManager.Instance.PlaySE_Sys(7);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    thirdtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "は";
                    secondtext.text = "ファイアを唱えた";
                    PlayerStatus.Instance.ReceiveMagicDamage(20);
                    EnemyStatusManager.Instance.EcurrentMP -= 5;
                    thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 20 + "のダメージ";
                }
                else
                {
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    thirdtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "は";
                    secondtext.text = "ファイアを唱えた";
                    thirdtext.text = "しかしMPが足りない！";
                }

            }
            if (randomNumber == 2)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 10)
                {
                    SoundManager.Instance.PlaySE_Sys(10);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "はアイスを唱えた";
                    PlayerStatus.Instance.ReceiveMagicDamage(30);
                    EnemyStatusManager.Instance.EcurrentMP -= 10;
                    secondtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 30 + "のダメージ";
                }
                else
                {
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "はアイスを唱えた";
                    secondtext.text = "しかしMPが足りない！";
                }

            }
        }
        else if (EnemyStatusManager.Instance.Ename == "リッチ")
        {
            int randomNumber = Random.Range(1, 5);
            if (randomNumber == 1)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 5)
                {
                    SoundManager.Instance.PlaySE_Sys(8);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "はフレアを唱えた";
                    PlayerStatus.Instance.ReceiveMagicDamage(30);
                    EnemyStatusManager.Instance.EcurrentMP -= 5;
                    secondtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 30 + "のダメージ";
                }
                else
                {
                    yield return StartCoroutine(NormalComand());
                }

            }
            if (randomNumber == 2)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 20)
                {
                    SoundManager.Instance.PlaySE_Sys(9);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "はメテオを唱えた";
                    PlayerStatus.Instance.ReceiveMagicDamage(40);
                    EnemyStatusManager.Instance.EcurrentMP -= 20;
                    secondtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 40 + "のダメージ";
                }
                else
                {
                    yield return StartCoroutine(NormalComand());
                }

            }
            if (randomNumber == 3)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 10 && EnemyStatusManager.Instance.EcurrentHP <= 100)
                {
                    SoundManager.Instance.PlaySE_Sys(4);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "はヒールを唱えた";
                    EnemyStatusManager.Instance.EcurrentHP += 30;
                    EnemyStatusManager.Instance.EcurrentMP -= 10;
                    if (EnemyStatusManager.Instance.EcurrentHP > EnemyStatusManager.Instance.EmaxHP)
                    {
                        EnemyStatusManager.Instance.EcurrentHP = EnemyStatusManager.Instance.EmaxHP;
                    }
                    secondtext.text = EnemyStatusManager.Instance.Ename + "は３０回復した";
                }
                else if (EnemyStatusManager.Instance.EcurrentHP > 100 || (EnemyStatusManager.Instance.EcurrentMP < 10 && EnemyStatusManager.Instance.EcurrentHP <= 100))
                {

                    yield return StartCoroutine(NormalComand());

                }

            }
            if (randomNumber == 4)
            {
                SoundManager.Instance.PlaySE_Sys(10);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "は勢いよく殴りかかってきた";
                secondtext.text = "勢いよく殴りかかってきた";
                PlayerStatus.Instance.ReceiveMagicDamage(50);
                thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "に" + 30 + "のダメージ";

            }
        }

        yield return new WaitForSeconds(1.0f); // 1秒待つ
    }


}
