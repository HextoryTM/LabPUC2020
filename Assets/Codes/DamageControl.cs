using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamageControl : MonoBehaviour, IDamagable
{
    public IAWalk iaWalk;
    public int timesHitted = 0;

    public SkinnedMeshRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        timesHitted = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage() 
    {
        Destroy(gameObject, 3);
        GetComponent<IAWalk>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        iaWalk.currentState = IAWalk.IAState.Dying;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            Debug.Log("ProjectHit");
            Destroy(collision.gameObject);
            GetDamageProjectile();
        }
    }

    public void GetDamageProjectile()
    {
        timesHitted++;
        iaWalk.currentState = IAWalk.IAState.Hitted;

        if (timesHitted >= 3)
        {
            iaWalk.currentState = IAWalk.IAState.Dying;
        }
    }

    public void Damage()
    {
        Debug.Log("Enemy Damage");
        GetDamageProjectile();
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
