using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool isMoving = false;
    public bool isTurning = false;

    float moveDist = 0;
    float turnAngle = 0;

    float moveSpeed = 0;
    float turnSpeed = 0;

    float distAccum = 0;
    float angleAccum = 0;

    void Start()
    {
        moveSpeed = 10 + GetComponent<Genes>().GetGene("Move Speed") / 5;
        turnSpeed = 100 + GetComponent<Genes>().GetGene("Turn Speed") * 3;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            distAccum += moveSpeed * Time.deltaTime;

            if (distAccum >= moveDist)
            {
                isMoving = false;
                distAccum = 0;
            }
        }

        if (isTurning)
        {
            transform.Rotate(0.0f, Mathf.Sign(turnAngle) * turnSpeed * Time.deltaTime, 0.0f, Space.Self);

            angleAccum += turnSpeed * Time.deltaTime;

            if (angleAccum >= Mathf.Abs(turnAngle))
            {
                isTurning = false;
                angleAccum = 0;
            }
        }
    }
    public void SetMove(float dist)
    {
        moveDist = dist;
        distAccum = 0;
        if (dist == 0) isMoving = false;
        else isMoving = true;
    }

    public void SetTurn(float angle)
    {
        turnAngle = angle;
        angleAccum = 0;
        if (angle == 0) isTurning = false;
        else isTurning = true;
    }
}
