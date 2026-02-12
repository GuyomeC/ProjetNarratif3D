using UnityEngine;

public class fin : MonoBehaviour
{
    public ChangeScene changeScene;

    private void Start()
    {
        ChangeScene changeScene = FindFirstObjectByType<ChangeScene>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                changeScene.LoadScene("MenuScene");
            }
        }
    }
}
