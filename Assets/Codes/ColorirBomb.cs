using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorirBomb : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer render;
    private UnityEngine.Random random;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        render.material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
            Pintar(other);
    }

    private void Pintar(Collision other)
    {
        if (other.gameObject.GetComponent<Renderer>())
        {
            Colorir(other.gameObject.GetComponent<Renderer>());
        }
        else if (other.gameObject.GetComponentInChildren<Renderer>())
        {
            Colorir(other.gameObject.GetComponentInChildren<Renderer>());
        }
        else if (other.gameObject.GetComponentInParent<Renderer>())
        {
            Colorir(other.gameObject.GetComponentInParent<Renderer>());
        }

    }

    private void Colorir(Renderer rend)
    {
        Color oCor = rend.material.color;
        oCor = render.material.color;
        rend.material.color = oCor;
    }
}
