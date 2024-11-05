using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float speed = 0.1f;

    public Sprite defaultSprite; // �f�t�H���g�̉摜
    public Sprite leftSprite;    // �������̉摜
    public Sprite rightSprite;   // �E�����̉摜
    public Sprite upSprite;      // ������̉摜
    public Sprite downSprite;    // �������̉摜

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite; // �ŏ��̓f�t�H���g�̉摜��\��

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 pos = transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.sprite = rightSprite; // �E�����̉摜��\��
            pos.x += speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.sprite = leftSprite; // �������̉摜��\��
            pos.x -= speed;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            spriteRenderer.sprite = upSprite; // ������̉摜��\��
            pos.y += speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            spriteRenderer.sprite = downSprite; // �������̉摜��\��
            pos.y -= speed;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite; // �f�t�H���g�̉摜��\��
        }

        transform.position = pos;

    }
}
