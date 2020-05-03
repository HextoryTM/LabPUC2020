using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public MeshRenderer render;

    public void Damage()
    {
        //Destroy(gameObject);
        StopAllCoroutines();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        int blinks = 10;

        while (blinks > 0)
        {
            render.enabled = !render.enabled;
            yield return new WaitForSeconds(0.1f);
            blinks--;
        }

        render.enabled = true;
        
    }
}
