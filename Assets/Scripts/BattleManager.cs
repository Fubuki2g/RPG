using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BattleManager : Singleton<BattleManager>
{
    public Sprite[] Enemys; // �G�f�B�^�[��3�̃X�v���C�g���w��
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

    // �R���[�`�������s�����ǂ����������t���O
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
        // EnemySprite�I�u�W�F�N�g����Image�R���|�[�l���g���擾
        imageComponent = EnemySprite.GetComponent<Image>();

        // Enemys�̒����烉���_����1��Sprite��I��
        int randomIndex = Random.Range(0, 3);
        Sprite selectedSprite = Enemys[randomIndex];

        // Image�R���|�[�l���g�ɑI�΂ꂽSprite��ݒ�
        imageComponent.sprite = selectedSprite;

        firsttext.text = RandomEName[randomIndex] + "�������ꂽ�I";
        secondtext.text = "�R�}���h�H";
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

    // �R���[�`�����J�n���郁�\�b�h
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
            // �v���C���[�̑f�������G��葁���ꍇ
            if (DataManager.Instance.LoadInt("SPD") > EnemyStatusManager.Instance.Espeed)
            {
                SoundManager.Instance.PlaySE_Sys(3);
                firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̍U��";
                firsttextobj.SetActive(true);
                PlayerStatus.Instance.GiveDamage();
                secondtext.text = EnemyStatusManager.Instance.Ename + "��" + PlayerStatus.Instance.gdamage + "�̃_���[�W";
                secondtextobj.SetActive(true);

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        secondtext.text = "���x�����オ�����I";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�q�[�����o�����I";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�t�@�C�A���o�����I";
                        }

                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
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
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);
                
                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();
               
                
            }
            // �G�̑f�������v���C���[��葁���ꍇ
            else if (DataManager.Instance.LoadInt("SPD") < EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
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
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                SoundManager.Instance.PlaySE_Sys(3);
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̍U��";
                firsttextobj.SetActive(true);
                PlayerStatus.Instance.GiveDamage();
                secondtext.text = EnemyStatusManager.Instance.Ename + "��" + PlayerStatus.Instance.gdamage + "�̃_���[�W";
                secondtextobj.SetActive(true);

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        secondtext.text = "���x�����オ�����I";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�q�[�����o�����I";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�t�@�C�A���o�����I";
                        }

                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                nextcomand();
            }
            // �v���C���[�ƓG�̑f�����������ꍇ
            else if (DataManager.Instance.LoadInt("SPD") == EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
                if (randomNumber == 1)
                {
                    SoundManager.Instance.PlaySE_Sys(3);
                    firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̍U��";
                    firsttextobj.SetActive(true);
                    PlayerStatus.Instance.GiveDamage();
                    secondtext.text = EnemyStatusManager.Instance.Ename + "��" + PlayerStatus.Instance.gdamage + "�̃_���[�W";
                    secondtextobj.SetActive(true);

                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
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
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            secondtext.text = "���x�����オ�����I";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�q�[�����o�����I";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�t�@�C�A���o�����I";
                            }

                            yield return new WaitForSeconds(1.0f); // 1�b�҂�

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // �v���C���[���{�^���������̂�҂�
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
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1�b�҂�

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
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1�b�҂�

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    SoundManager.Instance.PlaySE_Sys(3);
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̍U��";
                    firsttextobj.SetActive(true);
                    PlayerStatus.Instance.GiveDamage();
                    secondtext.text = EnemyStatusManager.Instance.Ename + "��" + PlayerStatus.Instance.gdamage + "�̃_���[�W";
                    secondtextobj.SetActive(true);

                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
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
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            secondtext.text = "���x�����オ�����I";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�q�[�����o�����I";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�t�@�C�A���o�����I";
                            }

                            yield return new WaitForSeconds(1.0f); // 1�b�҂�

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // �v���C���[���{�^���������̂�҂�
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

            // �v���C���[�̂ق����f�����������ꍇ
            if (DataManager.Instance.LoadInt("SPD") > EnemyStatusManager.Instance.Espeed)
            {
                if (PlayerBattleController.Instance.heal)
                {
                    if (DataManager.Instance.LoadInt("nowMP") < 3)
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "������MP������Ȃ��I";
                        secondtextobj.SetActive(true);
                    }
                    else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�����������N����Ȃ�����";
                        secondtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "HP��30�񕜂���";
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓t�@�C�A���g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�t�@�C�A���g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "������MP������Ȃ��I";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(7);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�t�@�C�A���g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = EnemyStatusManager.Instance.Ename + "��" + 20 + "�̃_���[�W";
                        thirdtextobj.SetActive(true);
                        PlayerStatus.Instance.ConsumeMP(3);
                        PlayerStatus.Instance.GiveMagicDamage(20);
                    }
                    PlayerBattleController.Instance.fire = false;
                }

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        secondtext.text = "���x�����オ�����I";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�q�[�����o�����I";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�t�@�C�A���o�����I";
                        }

                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
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
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();

            }
            // �G�̂ق����f�����������ꍇ
            else if (DataManager.Instance.LoadInt("SPD") < EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
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

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                if (PlayerBattleController.Instance.heal)
                {
                    if (DataManager.Instance.LoadInt("nowMP") < 3)
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "������MP������Ȃ��I";
                        secondtextobj.SetActive(true);
                    }
                    else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�����������N����Ȃ�����";
                        secondtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "HP��30�񕜂���";
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓t�@�C�A���g����";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�t�@�C�A���g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "������MP������Ȃ��I";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(7);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�t�@�C�A���g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = EnemyStatusManager.Instance.Ename + "��" + 20 + "�̃_���[�W";
                        thirdtextobj.SetActive(true);
                        PlayerStatus.Instance.ConsumeMP(3);
                        PlayerStatus.Instance.GiveMagicDamage(20);
                    }
                    PlayerBattleController.Instance.fire = false;
                }

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        secondtext.text = "���x�����オ�����I";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�q�[�����o�����I";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�t�@�C�A���o�����I";
                        }

                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
                        }
                        SoundManager.Instance.PlaySE_Sys(0);

                    }

                    battleend = true;
                    yield break;
                }

                nextcomand();
            }
            // �v���C���[�ƓG�̑f�����������ꍇ
            else if (DataManager.Instance.LoadInt("SPD") == EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
                if (randomNumber == 1)
                {
                    if (PlayerBattleController.Instance.heal)
                    {
                        if (DataManager.Instance.LoadInt("nowMP") < 3)
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "������MP������Ȃ��I";
                            secondtextobj.SetActive(true);
                        }
                        else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�����������N����Ȃ�����";
                            secondtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "HP��30�񕜂���";
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
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓t�@�C�A���g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�t�@�C�A���g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "������MP������Ȃ��I";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(7);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�t�@�C�A���g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = EnemyStatusManager.Instance.Ename + "��" + 20 + "�̃_���[�W";
                            thirdtextobj.SetActive(true);
                            PlayerStatus.Instance.ConsumeMP(3);
                            PlayerStatus.Instance.GiveMagicDamage(20);
                        }
                        PlayerBattleController.Instance.fire = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
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
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            secondtext.text = "���x�����オ�����I";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�q�[�����o�����I";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�t�@�C�A���o�����I";
                            }

                            yield return new WaitForSeconds(1.0f); // 1�b�҂�

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // �v���C���[���{�^���������̂�҂�
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
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1�b�҂�

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
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1�b�҂�

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    if (PlayerBattleController.Instance.heal)
                    {
                        if (DataManager.Instance.LoadInt("nowMP") < 3)
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "������MP������Ȃ��I";
                            secondtextobj.SetActive(true);
                        }
                        else if (DataManager.Instance.LoadInt("nowMP") >= 3 && DataManager.Instance.LoadInt("nowHP") == DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�����������N����Ȃ�����";
                            secondtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓q�[�����g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "HP��30�񕜂���";
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
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�̓t�@�C�A���g����";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�t�@�C�A���g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "������MP������Ȃ��I";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(7);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�t�@�C�A���g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = EnemyStatusManager.Instance.Ename + "��" + 20 + "�̃_���[�W";
                            thirdtextobj.SetActive(true);
                            PlayerStatus.Instance.ConsumeMP(3);
                            PlayerStatus.Instance.GiveMagicDamage(20);
                        }
                        PlayerBattleController.Instance.fire = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (EnemyStatusManager.Instance.EcurrentHP <= 0)
                    {
                        BattleEnd();
                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
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
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            secondtext.text = "���x�����オ�����I";
                            if (DataManager.Instance.LoadInt("nowLv") == 3)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�q�[�����o�����I";
                            }
                            if (DataManager.Instance.LoadInt("nowLv") == 7)
                            {
                                thirdtextobj.SetActive(true);
                                thirdtext.text = "�t�@�C�A���o�����I";
                            }

                            yield return new WaitForSeconds(1.0f); // 1�b�҂�

                            while (!Input.GetKeyDown(KeyCode.Return))
                            {
                                yield return null; // �v���C���[���{�^���������̂�҂�
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

            // �v���C���[�̂ق����f�����������ꍇ
            if (DataManager.Instance.LoadInt("SPD") > EnemyStatusManager.Instance.Espeed)
            {
                if (PlayerBattleController.Instance.leaf)
                {
                    if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�₭�������g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "�����������N����Ȃ�����";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�₭�������g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "HP��30�񕜂���";
                        thirdtextobj.SetActive(true);
                        DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                        PlayerStatus.Instance.Heal(30);
                    }
                    PlayerBattleController.Instance.leaf = false;
                }
                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
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
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                nextcomand();

            }
            // �G�̂ق����f�����������ꍇ
            else if (DataManager.Instance.LoadInt("SPD") < EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
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
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                firsttextobj.SetActive(false);
                secondtextobj.SetActive(false);
                thirdtextobj.SetActive(false);
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }

                if (PlayerBattleController.Instance.leaf)
                {
                    if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                    {
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�₭�������g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "�����������N����Ȃ�����";
                        thirdtextobj.SetActive(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlaySE_Sys(4);
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        firsttextobj.SetActive(true);
                        secondtext.text = "�₭�������g����";
                        secondtextobj.SetActive(true);
                        thirdtext.text = "HP��30�񕜂���";
                        thirdtextobj.SetActive(true);
                        DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                        PlayerStatus.Instance.Heal(30);
                    }
                    PlayerBattleController.Instance.leaf = false;
                }

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                nextcomand();
            }
            // �v���C���[�ƓG�̑f�����������ꍇ
            else if (DataManager.Instance.LoadInt("SPD") == EnemyStatusManager.Instance.Espeed)
            {
                int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
                if (randomNumber == 1)
                {
                    if (PlayerBattleController.Instance.leaf)
                    {
                        if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�₭�������g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "�����������N����Ȃ�����";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�₭�������g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "HP��30�񕜂���";
                            thirdtextobj.SetActive(true);
                            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                            PlayerStatus.Instance.Heal(30);
                        }
                        PlayerBattleController.Instance.leaf = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        yield return null; // �v���C���[���{�^���������̂�҂�
                    }
                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    thirdtextobj.SetActive(false);
                    SoundManager.Instance.PlaySE_Sys(0);

                    if (DataManager.Instance.LoadInt("nowHP") <= 0)
                    {
                        PlayerDead();

                        yield return new WaitForSeconds(2.0f); // 1�b�҂�

                        FadeManager.Instance.LoadScene("CastleScene", 1);

                        yield break;
                    }

                    firsttextobj.SetActive(false);
                    secondtextobj.SetActive(false);
                    if (PlayerBattleController.Instance.leaf)
                    {
                        if (DataManager.Instance.LoadInt("nowHP") >= DataManager.Instance.LoadInt("maxHP"))
                        {
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�₭�������g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "�����������N����Ȃ�����";
                            thirdtextobj.SetActive(true);
                        }
                        else
                        {
                            SoundManager.Instance.PlaySE_Sys(4);
                            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                            firsttextobj.SetActive(true);
                            secondtext.text = "�₭�������g����";
                            secondtextobj.SetActive(true);
                            thirdtext.text = "HP��30�񕜂���";
                            thirdtextobj.SetActive(true);
                            DataManager.Instance.SaveInt("Leaf", DataManager.Instance.LoadInt("Leaf") - 1);
                            PlayerStatus.Instance.Heal(30);
                        }
                        PlayerBattleController.Instance.leaf = false;
                    }

                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
            firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�͂ɂ�������";
            firsttextobj.SetActive(true);
            yield return new WaitForSeconds(1.0f); // 1�b�҂�

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null; // �v���C���[���{�^���������̂�҂�
            }
            SoundManager.Instance.PlaySE_Sys(0);

            firsttextobj.SetActive(false);
            int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
            if (randomNumber == 1)
            {
                SoundManager.Instance.PlayBGM(0);
                SoundManager.Instance.PlaySE_Sys(6);
                firsttext.text = "���܂��ɂ�������";
                firsttextobj.SetActive(true);
                battleend = true;
            }
            if (randomNumber == 2)
            {
                firsttext.text = "�������܂�肱�܂ꂽ";
                firsttextobj.SetActive(true);

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
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
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

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
        firsttext.text = "�R�}���h�H";
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
        firsttext.text = EnemyStatusManager.Instance.Ename + "����������";
        secondtext.text = EnemyStatusManager.Instance.Eexp + "�̌o���l��";
        thirdtext.text = EnemyStatusManager.Instance.Egold + "�S�[���h����ɓ��ꂽ";
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
        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "�͂���ł��܂���";
        firsttextobj.SetActive(true);
        DataManager.Instance.SaveBool("Dead", true);

    }
    public IEnumerator NormalComand()
    {
        SoundManager.Instance.PlaySE_Sys(3);
        firsttextobj.SetActive(true);
        secondtextobj.SetActive(true);
        firsttext.text = EnemyStatusManager.Instance.Ename + "�̍U��";
        PlayerStatus.Instance.ReceiveDamage();
        secondtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + PlayerStatus.Instance.rdamage + "�̃_���[�W";

        yield return new WaitForSeconds(1.0f); // 1�b�҂�
    }

    public IEnumerator SpecialComand()
    {
        if (EnemyStatusManager.Instance.Ename == "�X���C��")
        {
            firsttextobj.SetActive(true);
            secondtextobj.SetActive(false);
            firsttext.text = EnemyStatusManager.Instance.Ename + "�͗V��ł���";

        }
        else if (EnemyStatusManager.Instance.Ename == "�A�����E�l")
        {
            if (EnemyStatusManager.Instance.EcurrentMP >= 5)
            {
                SoundManager.Instance.PlaySE_Sys(9);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "��";
                secondtext.text = "���[�t��������";
                PlayerStatus.Instance.ReceiveMagicDamage(10);
                EnemyStatusManager.Instance.EcurrentMP -= 5;
                thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 10 + "�̃_���[�W";
            }
            else
            {
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "��";
                secondtext.text = "���[�t��������";
                thirdtext.text = "������MP������Ȃ��I";
            }

        }
        else if (EnemyStatusManager.Instance.Ename == "�L�m�R")
        {
            int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
            if (randomNumber == 1)
            {
                SoundManager.Instance.PlaySE_Sys(8);
                firsttextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "�͓ːi���Ă���";
                PlayerStatus.Instance.ReceiveMagicDamage(10);
                secondtextobj.SetActive(true);
                secondtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 10 + "�̃_���[�W";

            }
            if (randomNumber == 2)
            {
                firsttextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "�͓ːi���Ă���";
                secondtextobj.SetActive(true);
                secondtext.text = "���������������͊O�ꂽ";

            }

        }
        else if (EnemyStatusManager.Instance.Ename == "�A�[���}��")
        {
            if (EnemyStatusManager.Instance.EcurrentMP >= 3)
            {
                SoundManager.Instance.PlaySE_Sys(8);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "��";
                secondtext.text = "�A�C�r�[���������";
                PlayerStatus.Instance.ReceiveMagicDamage(20);
                EnemyStatusManager.Instance.EcurrentMP -= 3;
                thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 20 + "�̃_���[�W";
            }
            else
            {
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                thirdtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "��";
                secondtext.text = "�A�C�r�[���������";
                thirdtext.text = "������MP������Ȃ��I";
            }
        }
        else if (EnemyStatusManager.Instance.Ename == "�{��")
        {
            int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
            if (randomNumber == 1)
            {
                SoundManager.Instance.PlaySE_Sys(9);
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(true);
                firsttext.text = EnemyStatusManager.Instance.Ename + "�͎�������";
                PlayerStatus.Instance.ReceiveMagicDamage(50);
                secondtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 50 + "�̃_���[�W";

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                imageComponent.sprite = Enemys[3];
                firsttextobj.SetActive(true);
                secondtextobj.SetActive(false);
                firsttext.text = EnemyStatusManager.Instance.Ename + "�͍ӂ��U����";
                EnemyStatusManager.Instance.EcurrentHP = 0;

                yield return new WaitForSeconds(1.0f); // 1�b�҂�

                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null; // �v���C���[���{�^���������̂�҂�
                }
                SoundManager.Instance.PlaySE_Sys(0);

                if (DataManager.Instance.LoadInt("nowHP") <= 0)
                {
                    PlayerDead();

                    yield return new WaitForSeconds(2.0f); // 1�b�҂�

                    FadeManager.Instance.LoadScene("CastleScene", 1);

                    yield break;
                }
                else if (DataManager.Instance.LoadInt("nowHP") > 0)
                {
                    BattleEnd();
                    yield return new WaitForSeconds(1.0f); // 1�b�҂�

                    while (!Input.GetKeyDown(KeyCode.Return))
                    {
                        yield return null; // �v���C���[���{�^���������̂�҂�
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
                        firsttext.text = DataManager.Instance.LoadString("PlayerName") + "��";
                        secondtext.text = "���x�����オ�����I";
                        if (DataManager.Instance.LoadInt("nowLv") == 3)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�q�[�����o�����I";
                        }
                        if (DataManager.Instance.LoadInt("nowLv") == 7)
                        {
                            thirdtextobj.SetActive(true);
                            thirdtext.text = "�t�@�C�A���o�����I";
                        }

                        yield return new WaitForSeconds(1.0f); // 1�b�҂�

                        while (!Input.GetKeyDown(KeyCode.Return))
                        {
                            yield return null; // �v���C���[���{�^���������̂�҂�
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
                firsttext.text = EnemyStatusManager.Instance.Ename + "�͎�������";
                secondtext.text = "�����������N����Ȃ�����";

            }
        }
        else if (EnemyStatusManager.Instance.Ename == "�S�[�X�g")
        {
            int randomNumber = Random.Range(1, 3); // 1����2�܂ł̃����_���Ȑ����𐶐�
            if (randomNumber == 1)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 5)
                {
                    SoundManager.Instance.PlaySE_Sys(7);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    thirdtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "��";
                    secondtext.text = "�t�@�C�A��������";
                    PlayerStatus.Instance.ReceiveMagicDamage(20);
                    EnemyStatusManager.Instance.EcurrentMP -= 5;
                    thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 20 + "�̃_���[�W";
                }
                else
                {
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    thirdtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "��";
                    secondtext.text = "�t�@�C�A��������";
                    thirdtext.text = "������MP������Ȃ��I";
                }

            }
            if (randomNumber == 2)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 10)
                {
                    SoundManager.Instance.PlaySE_Sys(10);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "�̓A�C�X��������";
                    PlayerStatus.Instance.ReceiveMagicDamage(30);
                    EnemyStatusManager.Instance.EcurrentMP -= 10;
                    secondtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 30 + "�̃_���[�W";
                }
                else
                {
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "�̓A�C�X��������";
                    secondtext.text = "������MP������Ȃ��I";
                }

            }
        }
        else if (EnemyStatusManager.Instance.Ename == "���b�`")
        {
            int randomNumber = Random.Range(1, 5);
            if (randomNumber == 1)
            {
                if (EnemyStatusManager.Instance.EcurrentMP >= 5)
                {
                    SoundManager.Instance.PlaySE_Sys(8);
                    firsttextobj.SetActive(true);
                    secondtextobj.SetActive(true);
                    firsttext.text = EnemyStatusManager.Instance.Ename + "�̓t���A��������";
                    PlayerStatus.Instance.ReceiveMagicDamage(30);
                    EnemyStatusManager.Instance.EcurrentMP -= 5;
                    secondtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 30 + "�̃_���[�W";
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
                    firsttext.text = EnemyStatusManager.Instance.Ename + "�̓��e�I��������";
                    PlayerStatus.Instance.ReceiveMagicDamage(40);
                    EnemyStatusManager.Instance.EcurrentMP -= 20;
                    secondtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 40 + "�̃_���[�W";
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
                    firsttext.text = EnemyStatusManager.Instance.Ename + "�̓q�[����������";
                    EnemyStatusManager.Instance.EcurrentHP += 30;
                    EnemyStatusManager.Instance.EcurrentMP -= 10;
                    if (EnemyStatusManager.Instance.EcurrentHP > EnemyStatusManager.Instance.EmaxHP)
                    {
                        EnemyStatusManager.Instance.EcurrentHP = EnemyStatusManager.Instance.EmaxHP;
                    }
                    secondtext.text = EnemyStatusManager.Instance.Ename + "�͂R�O�񕜂���";
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
                firsttext.text = EnemyStatusManager.Instance.Ename + "�͐����悭���肩�����Ă���";
                secondtext.text = "�����悭���肩�����Ă���";
                PlayerStatus.Instance.ReceiveMagicDamage(50);
                thirdtext.text = DataManager.Instance.LoadString("PlayerName") + "��" + 30 + "�̃_���[�W";

            }
        }

        yield return new WaitForSeconds(1.0f); // 1�b�҂�
    }


}
