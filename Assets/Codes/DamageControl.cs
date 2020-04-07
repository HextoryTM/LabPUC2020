using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamageControl : MonoBehaviour
{
    public IAWalk iaWalk;
    int timesHitted = 0;
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
}
