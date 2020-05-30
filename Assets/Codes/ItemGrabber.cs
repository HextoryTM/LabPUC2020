using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    private GameObject weaponGrabbed;
    public Transform handPosition;

    public GameObject[] weaponPrefabs;

    private void Start()
    {
        if (CommonStatus.weaponOnHand != -1)
        {
            weaponGrabbed = Instantiate(weaponPrefabs[CommonStatus.weaponOnHand], Vector3.zero, Quaternion.identity);
            weaponGrabbed.name = weaponPrefabs[CommonStatus.weaponOnHand].name;
            EquipWeapon(weaponGrabbed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponDropped"))
        {
            other.GetComponent<PhisicalWeapon>().canDamage = false;
            if (weaponGrabbed)
            {
                UnequipWeapon(weaponGrabbed);
            }

            other.gameObject.layer = transform.gameObject.layer;
            weaponGrabbed = other.gameObject;

            EquipWeapon(weaponGrabbed);
        }
    }

    void EquipWeapon(GameObject weapon)
    {
        weapon.transform.parent = handPosition; //coloca como filho da mao
        weapon.transform.localPosition = Vector3.zero; //vai pra posicao da mao
        weapon.GetComponent<Rigidbody>().isKinematic = true; //desativa as fisicas do rigidbody
        weapon.transform.localRotation = Quaternion.identity; //reseta a rotacao da espada

        weapon.GetComponent<Collider>().enabled = false;
        weapon.GetComponent<PhisicalWeapon>().canDamage = true;

        for(int i = 0; i < weaponPrefabs.Length; i++)
        {
            if(weapon.name == weaponPrefabs[i].name)
            {
                CommonStatus.weaponOnHand = i;
            }
        }
    }

    void UnequipWeapon(GameObject weapon)
    {
        weapon.GetComponent<PhisicalWeapon>().canDamage = false;
        weapon.GetComponent<Collider>().enabled = true;
        weapon.transform.parent = null;

        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.transform.Translate(-transform.up);

        weapon.gameObject.layer = default;
        CommonStatus.weaponOnHand = -1;
    }
}
