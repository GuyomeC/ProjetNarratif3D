using UnityEngine;

public class start : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Animator>().SetBool("Dialog", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
