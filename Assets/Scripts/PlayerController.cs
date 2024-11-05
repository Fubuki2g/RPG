using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Singleton<PlayerController>
{
    public Sprite defaultSprite;        // �f�t�H���g�̉摜
    public Sprite[] defaultSprites;     // �f�t�H���g�̃A�j���[�V�����X�v���C�g�z��
    public Sprite[] upSprites;          // ������̃A�j���[�V�����X�v���C�g�z��
    public Sprite[] downSprites;        // �������̃A�j���[�V�����X�v���C�g�z��
    public Sprite[] leftSprites;        // �������̃A�j���[�V�����X�v���C�g�z��
    public Sprite[] rightSprites;       // �E�����̃A�j���[�V�����X�v���C�g�z��
    public float walkSpeed = 5f;        // ���s���x

    private SpriteRenderer spriteRenderer;
    private float timeBetweenSprites = 0.2f;  // �e�X�v���C�g�̕\������
    private float timer = 0f;
    private Sprite[] currentSprites;    // ���݂̃A�j���[�V�����X�v���C�g�z��
    private int currentSpriteIndex = 0;
    private bool isMoving = false;
    public bool walkable = true;

    public GameObject MenuCanvas;
    public GameObject MenuButton;
    public bool menu = false;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(0);

        walkable = true;

        // �}�E�X�J�[�\�����\���ɂ���
        Cursor.visible = false;

        // �}�E�X�̃��b�N���������A�}�E�X���E�B���h�E�O�Ɉړ����Ȃ��悤�ɂ���
        Cursor.lockState = CursorLockMode.Locked;

        walkable = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (defaultSprites.Length > 0)
        {
            currentSprites = defaultSprites; // �f�t�H���g�̃A�j���[�V�����X�v���C�g��ݒ�
        }
        else
        {
            spriteRenderer.sprite = defaultSprite; // �f�t�H���g�̉摜��\��
            currentSprites = new Sprite[0]; // ��̔z���ݒ�
        }
    }

    private void Update()
    {
        if (walkable)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (horizontalInput != 0 || verticalInput != 0)
            {
                isMoving = true;

                Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized;
                transform.Translate(movement * walkSpeed * Time.deltaTime);

                // Set the animation sprites based on the input direction
                if (horizontalInput > 0)
                    currentSprites = rightSprites;
                else if (horizontalInput < 0)
                    currentSprites = leftSprites;
                else if (verticalInput > 0)
                    currentSprites = upSprites;
                else if (verticalInput < 0)
                    currentSprites = downSprites;
            }

            timer += Time.deltaTime;
            if (timer >= timeBetweenSprites)
            {
                timer = 0f;
                currentSpriteIndex = (currentSpriteIndex + 1) % currentSprites.Length;
                spriteRenderer.sprite = currentSprites[currentSpriteIndex];
            }
        }
        else
        {
            isMoving = false;
        }

        if (!isMoving)
        {
            // Display the default sprite or animation when not moving
            if (defaultSprites.Length > 0)
            {
                timer += Time.deltaTime;
                if (timer >= timeBetweenSprites)
                {
                    timer = 0f;
                    currentSpriteIndex = (currentSpriteIndex + 1) % defaultSprites.Length;
                    spriteRenderer.sprite = defaultSprites[currentSpriteIndex];
                }
            }
            else
            {
                spriteRenderer.sprite = defaultSprite;
                currentSpriteIndex = 0;
                currentSprites = new Sprite[0];
            }
        }

        if (!menu && Input.GetKeyDown(KeyCode.M) && walkable)
        {
            SoundManager.Instance.PlaySE_Sys(0);
            MenuCanvas.SetActive(true);
            walkable = false;
            menu = true;
            EventSystem.current.SetSelectedGameObject(MenuButton);

        }
        else if (menu && Input.GetKeyDown(KeyCode.M) || menu && Input.GetKeyDown(KeyCode.C) || menu && Input.GetKeyDown(KeyCode.Backspace))
        {
            SoundManager.Instance.PlaySE_Sys(0);
            MenuManager.Instance.Menue[0].SetActive(true);
            MenuManager.Instance.Menue[1].SetActive(false);
            MenuManager.Instance.Menue[2].SetActive(false);
            MenuManager.Instance.Menue[3].SetActive(false);
            MenuManager.Instance.Menue[4].SetActive(false);
            MenuCanvas.SetActive(false);
            walkable = true;
            menu = false;
        }
    }
}
