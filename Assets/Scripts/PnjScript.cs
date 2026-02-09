using UnityEngine;

public class PnjScript : MonoBehaviour, IInteractable
{

    public GameObject GameObject;
    public RuntimeDialogGraph dialogGraph;
    public string PnjName;

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject != null)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.SetActive(true);
                GameManager.Instance.CanShowDialogue = true;
                DialogManager.Instance.CurrentDialogGraph = dialogGraph;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (GameObject != null)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.SetActive(false);
                GameManager.Instance.CanShowDialogue = false;
            }
        }
    }
}
