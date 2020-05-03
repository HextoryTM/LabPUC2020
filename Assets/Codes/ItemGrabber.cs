using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    private GameObject weaponGrabbed;
    public Transform handPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponDropped"))
        {
            if (weaponGrabbed)
            {
                weaponGrabbed.transform.parent = null;

                weaponGrabbed.GetComponent<Rigidbody>().isKinematic = false;
                weaponGrabbed.transform.Translate(Vector3.right);

                weaponGrabbed.gameObject.layer = default;
            }

            weaponGrabbed = other.gameObject;

            other.gameObject.layer = transform.gameObject.layer;

            other.transform.parent = handPosition; //coloca como filho da mao
            other.transform.localPosition = Vector3.zero; //vai pra posicao da mao
            other.GetComponent<Rigidbody>().isKinematic = true; //desativa as fisicas do rigidbody
            other.transform.localRotation = Quaternion.identity; //reseta a rotacao da espada
        }
    }
}
