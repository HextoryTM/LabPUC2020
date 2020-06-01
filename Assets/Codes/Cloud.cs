using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, Time.deltaTime*0.05f, 0);
    }
}
