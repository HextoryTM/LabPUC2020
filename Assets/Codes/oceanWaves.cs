using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oceanWaves : MonoBehaviour
{
    public Renderer render;

    void Update()
    {
        render.material.SetTextureOffset("_MainTex", new Vector2(Time.time * 0.01f, 0));
        render.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(Time.time * -0.01f, 0));
    }
}
