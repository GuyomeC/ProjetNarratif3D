using UnityEngine;

public class NextZone : MonoBehaviour
{

    public GameObject zone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
