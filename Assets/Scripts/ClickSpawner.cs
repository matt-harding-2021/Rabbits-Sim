using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Camera sceneCamera;
    RaycastHit hit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(sceneCamera.ScreenPointToRay(Input.mousePosition), out hit);

            prefab.name = "Rabbit(" + transform.childCount + ")";
            Instantiate(prefab, hit.point + (transform.up * 2), this.transform.rotation, this.transform);
        }
    }
}