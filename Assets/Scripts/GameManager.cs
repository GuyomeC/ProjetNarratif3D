using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public bool ShowDialogue = false;
    public int idPNJ;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
