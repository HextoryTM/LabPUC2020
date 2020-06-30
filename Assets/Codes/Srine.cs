using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Srine : MonoBehaviour
{
    public bool unlocked = false;
    public bool backToWorld = false;
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (unlocked)
            {
                if (!backToWorld)
                {
                    CommonStatus.lastPosition = other.transform.position - Vector3.forward * 2;
                    CommonStatus.Vida = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdWalk>().vidas;
                }

                if (backToWorld)
                {
                    StartCoroutine(MyLoadScene("MainScene"));
                }
                else
                {
                    if (sceneToLoad != "")
                    {
                        StartCoroutine(MyLoadScene(sceneToLoad));
                    }
                }
            }
        }
    }

    IEnumerator MyLoadScene(string scene)
    {
        Camera.main.SendMessage("CallFadeOut");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
    }
}
