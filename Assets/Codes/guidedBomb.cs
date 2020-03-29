using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guidedBomb : MonoBehaviour
{
    public GameObject target;

    Rigidbody rdb;
    public float bombForce = 1000;
    // Start is called before the first frame update
    void Start()
    {
        rdb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.LookAt(target.transform);
        rdb.AddForce(transform.forward*50);

        
        if (Vector3.Distance(transform.position, target.transform.position) < 1)
        {
            Explode();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            Explode();
    }

    void Explode()
    {
        print("Boom!");
        Destroy(gameObject);
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.up, 10);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.rigidbody)
                    hit.rigidbody.AddExplosionForce(bombForce, transform.position, 10);
            }
        }
    }
}
