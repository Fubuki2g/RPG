using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float speed = 0.1f;

    public Sprite defaultSprite; // デフォルトの画像
    public Sprite leftSprite;    // 左向きの画像
    public Sprite rightSprite;   // 右向きの画像
    public Sprite upSprite;      // 上向きの画像
    public Sprite downSprite;    // 下向きの画像

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite; // 最初はデフォルトの画像を表示

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 pos = transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.sprite = rightSprite; // 右向きの画像を表示
            pos.x += speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.sprite = leftSprite; // 左向きの画像を表示
            pos.x -= speed;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            spriteRenderer.sprite = upSprite; // 上向きの画像を表示
            pos.y += speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            spriteRenderer.sprite = downSprite; // 下向きの画像を表示
            pos.y -= speed;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite; // デフォルトの画像を表示
        }

        transform.position = pos;

    }
}
