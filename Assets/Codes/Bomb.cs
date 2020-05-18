using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public float bombForce=1000;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 3);
    }

    void Explode()
    {
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3);
        Destroy(gameObject);
        RaycastHit[] hits;
        hits=Physics.SphereCastAll(transform.position, 5, Vector3.up, 5);

        if (hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                if (hit.rigidbody)
                {
                    if (hit.transform.tag != "Player")
                    {
                        hit.rigidbody.isKinematic = false;
                        hit.rigidbody.AddExplosionForce(bombForce, transform.position, 5);
                        hit.collider.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
}
