using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    public int iaCount = 4;
    public List<GameObject> ias;
    public bool over { get; private set; }
    public int inimigos { get; private set; }

    private GameManager gm;
    private void Awake()
    {
        inimigos = 0;
    }

    void Start()
    {
        GetAllIA();
    }

    // Update is called once per frame
    void Update()
    {
        VerifyIA();
    }

    void GetAllIA()
    {
        IAWalk[] temp = new IAWalk[iaCount];

        temp = GetComponentsInChildren<IAWalk>();

        for (int i = 0; i < temp.Length; i++)
            ias.Add(temp[i].gameObject);
    }

    void VerifyIA()
    {
        int index = 0;
        inimigos = 0;
        for (int i = 0; i < ias.Count; i++)
        {
            if (ias[i] == null)
                index++;
            else
                inimigos++;
        }

        if (index == iaCount)
        {
            over = true;
            Victory();
        }
        else
            over = false;
    }

    private void Victory()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.SetVictory();
    }
}
