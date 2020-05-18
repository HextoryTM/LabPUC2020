using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCam : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset, lookOffset;
    public float scrollOffset;

    GameObject fakeObject;
    // Start is called before the first frame update

    private void Awake()
    {
        fakeObject = new GameObject();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public GameObject GetReferenceObject()
    {
        return fakeObject;
    }

    // Update is called once per frame
    void Update()
    {
        fakeObject.transform.position = Vector3.Lerp(fakeObject.transform.position, player.transform.position, Time.deltaTime * 5);

        Vector3 dirBack = transform.position - (player.transform.position + lookOffset); //calcular a distancia para tras da camera
        float distanceToHit = 10;
        if (Physics.Raycast(player.transform.position + lookOffset, dirBack, out RaycastHit hit, 10, 65279)) //Raycast para ver o limite da camera
        {
            distanceToHit = hit.distance;
            Debug.DrawLine(player.transform.position + lookOffset, hit.point);
        }

        Vector3 backVector = (fakeObject.transform.forward * offset.z); //vetor de distanciamento
        backVector = Vector3.ClampMagnitude(backVector, distanceToHit - 0.5f); //limite de tamanho do vetor

        transform.position = fakeObject.transform.position + backVector + fakeObject.transform.up * offset.y; //aplicacao da posicao da camera

        transform.LookAt(player.transform.position + lookOffset); //olhar para o jogador

        scrollOffset = Mathf.Clamp(scrollOffset + Input.mouseScrollDelta.y, -6, -2); //ajusta de dinstancia pelo scroll
        offset = new Vector3(0, offset.y, scrollOffset);

        fakeObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 3, 0)); //aplicacao da rotacao
    }
}
