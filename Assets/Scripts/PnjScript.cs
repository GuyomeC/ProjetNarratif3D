using UnityEngine;

public class PnjScript : MonoBehaviour, IInteractable
{

    public GameObject GameObject;
    public int ID;
    public RuntimeDialogGraph dialogGraph;

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
                GameManager.Instance.ShowDialogue = true;
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
                GameManager.Instance.ShowDialogue = false;
            }
        }
    }
}
