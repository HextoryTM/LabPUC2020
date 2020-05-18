using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    private ThirdWalk playerWalk;
    private IAController iaController;
    public Text vidaText;
    public Text inimigosText;

    private void Start()
    {
        FindReference();
    }

    private void Update()
    {
        Atualizar();
    }

    void Atualizar()
    {
        if (playerWalk != null && vidaText != null)
        {
            vidaText.text = "Vidas: " + playerWalk.vidas;
        }
        else
            FindReference();

        if (iaController != null && inimigosText != null)
        {
            inimigosText.text = "Inimigos: " + iaController.inimigos;
        }
        else
            FindReference();
    }

    void FindReference()
    {
        playerWalk = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdWalk>();
        iaController = GameObject.FindGameObjectWithTag("IAController").GetComponent<IAController>();
    }
}
