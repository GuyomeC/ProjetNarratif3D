using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    public GameObject Cam;
    public GameObject CamTarget;


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
        if (Cam != null) 
        {
            if(other.CompareTag("Player"))
            {
                Cam.transform.position = CamTarget.transform.position;
                Cam.transform.rotation = CamTarget.transform.rotation;
            }
        }
    }
}
