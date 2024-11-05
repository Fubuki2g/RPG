using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Talk : MonoBehaviour
{
    public GameObject dialogue;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI Name;

    [SerializeField] string words = "ここにセリフ";
    [SerializeField] string cname = "なまえ";

    private bool isInRange = false;
    private bool Tkey = false;

    private void Update()
    {
        // Tキーが押されたときに処理を行う
        if (!Tkey && isInRange && Input.GetKeyDown(KeyCode.Return) && PlayerController.Instance.walkable)
        {
            Text.text = words;
            Name.text = cname;
            dialogue.SetActive(true);
            Tkey = true;
            PlayerController.Instance.walkable = false;
            SoundManager.Instance.PlaySE_Sys(0);
        }
        else if (Tkey && Input.GetKeyDown(KeyCode.Return))
        {
            dialogue.SetActive(false);
            Tkey = false;
            PlayerController.Instance.walkable = true;
            SoundManager.Instance.PlaySE_Sys(0);
        }
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
            Tkey = false;
        }
    }
}
