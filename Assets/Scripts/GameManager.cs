using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public bool CanShowDialogue = false;
    public bool IsInDialogue = false;  
    public int idPNJ;
    public ChangeScene changeScene;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        changeScene = FindFirstObjectByType<ChangeScene>();
    }

    public void ChangeScene(string sceneName)
    {
        changeScene.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
