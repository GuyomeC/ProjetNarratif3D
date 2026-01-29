using UnityEngine;

public class ObjectToSpawn : MonoBehaviour, IInteractable
{

    public GameObject GameObject;

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
            }
        }
    }
}
