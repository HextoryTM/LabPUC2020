using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Srine : MonoBehaviour
{
    public bool backToWorld = false;
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(!backToWorld)
                CommonStatus.lastPosition = other.transform.position;

            if (backToWorld)
            {
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                if (sceneToLoad != "")
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}
