using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    private ThirdWalk playerWalk;
    private IAController iaController;
    public Text vidaText;
    public Text inimigosText;

    public GameObject skillPanel;
    public Image[] skills;
    public Sprite[] allSkillsImg;

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
        SetSkillsImage();

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

    void SetSkillsImage()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].sprite = getSkillSprite(i);
        }
    }

    private Sprite getSkillSprite(int index)
    {
        return allSkillsImg[index];
    }
}
