using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BridgeScript : MonoBehaviour
{
    public GameObject Cam;
    public GameObject CamTarget;

    [Header("Paramètre de Fondu")] 
    public Image fadeImage;
    public float fadeDuration = 0.5f;

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
                StartCoroutine(FadeAndSwap());
            }
        }
    }

    IEnumerator FadeAndSwap()
    {
        yield return StartCoroutine(Fade(1));
        
        Cam.transform.position = CamTarget.transform.position;
        Cam.transform.rotation = CamTarget.transform.rotation;
        
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            fadeImage.color = new Color(0, 0,0, newAlpha);
            yield return null;
        }
        
        fadeImage.color = new Color(0, 0,0, targetAlpha);
    }
}
