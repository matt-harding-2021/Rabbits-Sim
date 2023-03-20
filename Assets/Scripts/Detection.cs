using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Detection : MonoBehaviour
{
    int numRays;
    float raycastAngle;
    float raycastLength;
    float raycastSpacing;

    float sphereRadius;
    Collider[] surroundings;

    void Start()
    {
        numRays = Mathf.CeilToInt(GetComponent<Genes>().GetGene("Number of Raycasts") / 10f) * 2;
        raycastAngle = GetComponent<Genes>().GetGene("Raycast Angle") * 2f;
        raycastLength = 3f + GetComponent<Genes>().GetGene("Raycast Distance") / 2f;

        raycastSpacing = raycastAngle / numRays;

        sphereRadius = GetComponent<Genes>().GetGene("Detection Sphere Radius") * 1.5f;
        surroundings = new Collider[10];
    }

    public RaycastHit[] CastRays(Vector3 position, float localAngle)
    {
        RaycastHit[] rayHit = new RaycastHit[numRays];

        float start = localAngle - raycastAngle / 2 + raycastSpacing / 2;
        for (int n = 0; n < numRays; n++)
        {
            float angle = (start + n * raycastSpacing) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            Physics.Raycast(new Ray(position, direction), out rayHit[n], raycastLength);
            Debug.DrawRay(new Ray(position, direction).origin, direction * raycastLength, Color.red); // (Debug) - Helps visualise whats going on
            //if(rayHit[n].collider)Debug.Log(rayHit[n].collider.tag);
        }
        return rayHit;
    }


    //public GameObject testSpherePrefab; // (Debug)
    //GameObject testSphere;              // (Debug)
    public Collider[] GetSurroundings(Vector3 position)
    {
        Physics.OverlapSphereNonAlloc(position, sphereRadius, surroundings);

        //(Debug) - Helps visualise whats going on
        /*
        testSphere = (GameObject)Instantiate(testSpherePrefab, transform.position, transform.rotation);
        testSphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);
        Destroy(testSphere, 0.2f); 
        */

        return surroundings;
    }
}
