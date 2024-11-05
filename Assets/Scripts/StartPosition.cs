using UnityEngine;

public class StartPosition : MonoBehaviour
{
    [SerializeField]
    private Transform objectToMove; // 移動させるオブジェクト

    private void Start()
    {
        if (PlayerPrefs.GetInt("AreaMove") == 1)
        {
            float initialX = PlayerPrefs.GetFloat("InitialX");
            float initialY = PlayerPrefs.GetFloat("InitialY");

            // オブジェクトの位置を初期位置に設定
            objectToMove.position = new Vector3(initialX, initialY, objectToMove.position.z);

            PlayerPrefs.SetInt("AreaMove", 0);
        }
    }
}
