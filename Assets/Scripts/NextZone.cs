using UnityEngine;

public class NextZone : MonoBehaviour
{

    public GameObject zone;

    private void OnTriggerEnter(Collider other)
    {
        if (zone != null) 
        {
            if(other.CompareTag("Player"))
            {
                zone.SetActive(true);
            }
        }
    }
}
