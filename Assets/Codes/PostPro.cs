using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostPro : MonoBehaviour
{
    public Material postMat;
    float color = 0;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postMat);
    }


    void Start()
    {
        postMat.SetColor("_Color", Color.black);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1);
        while(color < 1)
        {
            yield return new WaitForEndOfFrame();
            color += Time.deltaTime;
            postMat.SetColor("_Color", new Color(color, color, color));
        }
    }

    public void CallFadeOut() => StartCoroutine(FadeOut());

    IEnumerator FadeOut()
    {
        while (color > 0)
        {
            yield return new WaitForEndOfFrame();
            color -= Time.deltaTime;
            postMat.SetColor("_Color", new Color(color, color, color));
        }
    }
}
