using UnityEngine;

public class StartPosition : MonoBehaviour
{
    [SerializeField]
    private Transform objectToMove; // �ړ�������I�u�W�F�N�g

    private void Start()
    {
        if (PlayerPrefs.GetInt("AreaMove") == 1)
        {
            float initialX = PlayerPrefs.GetFloat("InitialX");
            float initialY = PlayerPrefs.GetFloat("InitialY");

            // �I�u�W�F�N�g�̈ʒu�������ʒu�ɐݒ�
            objectToMove.position = new Vector3(initialX, initialY, objectToMove.position.z);

            PlayerPrefs.SetInt("AreaMove", 0);
        }
    }
}
