using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject DefeatPanel, VictoryPanel;

    public void SetDefeat()
    {
        DefeatPanel.SetActive(true);
    }

    public void SetVictory()
    {
        VictoryPanel.SetActive(true);
    }

    public void Restart()
    {
        DefeatPanel.SetActive(false);
        VictoryPanel.SetActive(false);
    }
}
