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
    }

    public GameObject GetReferenceObject()
    {
        return fakeObject;
    }

    // Update is called once per frame
    void Update()
    {
        fakeObject.transform.position = Vector3.Lerp(fakeObject.transform.position, player.transform.position, Time.deltaTime * 5);
        transform.position = fakeObject.transform.position + fakeObject.transform.forward * offset.z + fakeObject.transform.up * offset.y;

        transform.LookAt(player.transform.position + lookOffset);

        scrollOffset = Mathf.Clamp(scrollOffset + Input.mouseScrollDelta.y, -6, -2);
        offset = new Vector3(0, offset.y, scrollOffset);

        fakeObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
    }
}
