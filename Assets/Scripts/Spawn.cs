using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject prefab;
    public int numObjects = 5;
    public float radius = 15.0f;

    public float spawnTimer = 1f;
    private float prevSpawnTime;
    void Start()
    {
        for (int i = 0; i < numObjects; i++)
        {
            prefab.name = this.name + "_" + i;
            Instantiate(prefab, GetRandomPosition(transform.position), this.transform.rotation, this.transform);
        }

        prevSpawnTime = Time.time;
    }
    private void Update()
    {
        if (Time.time - prevSpawnTime > spawnTimer)
        {
            Instantiate(prefab, GetRandomPosition(transform.position), this.transform.rotation, this.transform);
            prevSpawnTime = Time.time;
        }
    }

    private Vector3 GetRandomPosition(Vector3 center)
    {
        Vector3 pos;

        float angle = Random.Range(0, 360);
        float r = Random.Range(0, radius);
        pos.x = center.x + r * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + r * Mathf.Cos(angle * Mathf.Deg2Rad);

        return pos;
    }
}
