using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhisicalWeapon : MonoBehaviour
{
    public bool canDamage = false; //para evitar que a arma no chao de dano no jogador
    public bool hitting = false; //verificacao de seguranca para hittar apenas uma vez, e nao muitas em um so ataque. O seu valor e alterado na hora do ataque.

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //Ataque dos inimigos
        {
            if (canDamage)
            {
                if (!hitting)
                {
                    collision.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
                    StartCoroutine(Damaging(collision));
                }
            }
        }
        else if (canDamage) //Ataque do Jogador e etc
        {
            collision.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator Damaging(Collision other)
    {
        hitting = true;
        yield return new WaitForSeconds(1f);

        hitting = false;
    }
    
}
