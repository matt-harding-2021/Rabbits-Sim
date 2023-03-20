using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Water")
        {
            Destroy(this.gameObject);
        }
    }
}
