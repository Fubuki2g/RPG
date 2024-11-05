using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Singleton<PlayerController>
{
    public Sprite defaultSprite;        // デフォルトの画像
    public Sprite[] defaultSprites;     // デフォルトのアニメーションスプライト配列
    public Sprite[] upSprites;          // 上向きのアニメーションスプライト配列
    public Sprite[] downSprites;        // 下向きのアニメーションスプライト配列
    public Sprite[] leftSprites;        // 左向きのアニメーションスプライト配列
    public Sprite[] rightSprites;       // 右向きのアニメーションスプライト配列
    public float walkSpeed = 5f;        // 歩行速度

    private SpriteRenderer spriteRenderer;
    private float timeBetweenSprites = 0.2f;  // 各スプライトの表示時間
    private float timer = 0f;
    private Sprite[] currentSprites;    // 現在のアニメーションスプライト配列
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

        // マウスカーソルを非表示にする
        Cursor.visible = false;

        // マウスのロックを解除し、マウスがウィンドウ外に移動しないようにする
        Cursor.lockState = CursorLockMode.Locked;

        walkable = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (defaultSprites.Length > 0)
        {
            currentSprites = defaultSprites; // デフォルトのアニメーションスプライトを設定
        }
        else
        {
            spriteRenderer.sprite = defaultSprite; // デフォルトの画像を表示
            currentSprites = new Sprite[0]; // 空の配列を設定
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
