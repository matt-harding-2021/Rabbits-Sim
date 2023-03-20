using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    enum StateEnum { Wandering, Following, Avoiding, Turning_Back, Resting }
    StateEnum currentState;

    Vector3 pointOfInterest; // Location of an object the animal is following/avoiding.
    GameObject objectOfInterest;

    bool isLévy = false;

    bool isTurningBack = false;

    float timeAccum = 0;

    void Start()
    {
        currentState = StateEnum.Wandering;
    }

    void Update()
    {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case StateEnum.Wandering:
                {
                    if (!GetComponent<Movement>().isTurning)
                    {
                        if (!isLévy)
                        {
                            int newAngle;
                            do // Set a new angle to turn to
                            {
                                newAngle = Random.Range(-45, 45);
                            } while (newAngle > -10 && newAngle < 10); // Repeats if result is between -10 and +10.

                            GetComponent<Movement>().SetTurn(newAngle);

                            Collider[] surroundings = GetComponent<Detection>().GetSurroundings(transform.position);
                            DecideAction(colliders: surroundings, hits: null);
                        }

                        if (!GetComponent<Movement>().isMoving)
                        {
                            float newDist = Random.Range(1f, 5f);

                            if (!isLévy && Random.Range(1, 100) <= GetComponent<Genes>().GetGene("Lévy Tendency") / 10f) // Up to 10% of the time, move to a new area. 
                            {
                                newDist *= 40f;
                                GetComponent<Movement>().SetTurn(0);
                                isLévy = true;
                            }
                            else isLévy = false;
                            GetComponent<Movement>().SetMove(newDist);
                        }
                    }

                    RaycastHit[] raycastHits = GetComponent<Detection>().CastRays(transform.position, transform.localEulerAngles.y);
                    DecideAction(colliders: null, hits: raycastHits);
                }
                break;





            case StateEnum.Following:
                {
                    timeAccum += Time.deltaTime;
                    if (timeAccum > 0.25f)
                    {
                        Collider[] surroundings = GetComponent<Detection>().GetSurroundings(transform.position);
                        DecideAction(colliders: surroundings, hits: null);
                        timeAccum = 0;
                    }
                    RaycastHit[] raycastHits = GetComponent<Detection>().CastRays(transform.position, transform.localEulerAngles.y);
                    DecideAction(colliders: null, hits: raycastHits);
                    
                    if (objectOfInterest)
                    {
                        //if (objectOfInterest.CompareTag("Animal") && !GetComponent<Fitness>().Evaluate(objectOfInterest)) SetState("Wandering");
                        float dist = (objectOfInterest.transform.position - this.transform.position).magnitude;
                        Vector3 dir = (objectOfInterest.transform.position - this.transform.position).normalized;
                        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
                        GetComponent<Movement>().SetTurn(angle);
                        //Debug.Log("Angle Abs: " + Mathf.Abs(angle));
                        if (Mathf.Abs(angle) < 10f) //If the angle is straight enough, then a new move can be set to target destination.
                        {
                            if (dist > 1f)
                                GetComponent<Movement>().SetMove(dist);
                            else
                            {
                                SetState("Wandering");
                                objectOfInterest = null;
                            }
                        }
                    }
                    //else if (pointOfInterest) { }
                    else
                        SetState("Wandering");
                }
                break;





            case StateEnum.Avoiding:
                {
                    if (!GetComponent<Movement>().isTurning)
                    {
                        GetComponent<Movement>().SetMove(0);

                        Vector3 dir = (pointOfInterest - this.transform.position).normalized;
                        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
                        if (angle < 0) GetComponent<Movement>().SetTurn(10);
                        else GetComponent<Movement>().SetTurn(-10);

                        SetState("Wandering");
                        pointOfInterest = Vector3.zero;
                    }
                }
                break;





            case StateEnum.Turning_Back:
                {
                    if (!isTurningBack)
                    {
                        GetComponent<Movement>().SetMove(0);
                        GetComponent<Movement>().SetTurn(180);
                        isTurningBack = true;
                    }

                    if (isTurningBack && !GetComponent<Movement>().isTurning)
                    {
                        isTurningBack = false;
                        GetComponent<Movement>().SetMove(Random.Range(20, 50));
                        isLévy = true;
                        SetState("Wandering");
                    }
                }
                break;





            case StateEnum.Resting:
                {
                    GetComponent<Movement>().SetMove(0);
                    GetComponent<Movement>().SetTurn(0);
                }
                break;
        }
    }

    
    void DecideAction(Collider[] colliders, RaycastHit[] hits)
    {
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i])
                {
                    switch (colliders[i].tag)
                    {
                        case "Animal":
                            if (colliders[i].gameObject != this.gameObject)
                            {
                                if (GetComponent<Fitness>().Evaluate(colliders[i]))
                                {
                                    objectOfInterest = colliders[i].gameObject;
                                    pointOfInterest = Vector3.zero;
                                    SetState("Following");
                                }
                            }
                            break;
                        case "Plant(Green)":
                        case "Plant(Red)":
                        case "Plant(White)":
                            if (objectOfInterest != null) {
                                if (objectOfInterest.CompareTag("Animal")) break; //Animal tag takes presidence 
                                else if (objectOfInterest.CompareTag("Plant(Green)") || objectOfInterest.CompareTag("Plant(Red)") || objectOfInterest.CompareTag("Plant(White)"))
                                {
                                    if ((colliders[i].transform.position - this.transform.position).magnitude < (objectOfInterest.transform.position - this.transform.position).magnitude)
                                    {
                                        if (Mathf.Abs(colliders[i].gameObject.GetComponent<Rigidbody>().velocity.y) < 0.1f) //If the plant has stopped falling from spawn
                                        {
                                            objectOfInterest = colliders[i].gameObject;
                                            pointOfInterest = Vector3.zero;
                                            SetState("Following");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Mathf.Abs(colliders[i].gameObject.GetComponent<Rigidbody>().velocity.y) < 0.1f) //If the plant has stopped falling from spawn
                                {
                                    objectOfInterest = colliders[i].gameObject;
                                    pointOfInterest = Vector3.zero;
                                    SetState("Following");
                                }
                            }
                            break;
                    }
                }
            }
            //if (objectOfInterest == null) SetState("Wandering");
        }
        else if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider) { 
                    switch(hits[i].collider.tag)
                    {
                        case "Wall":
                            if ((hits[i].point - transform.position).magnitude < (pointOfInterest - transform.position).magnitude)
                            {
                                pointOfInterest = hits[i].point;
                                objectOfInterest = null;
                            }
                            SetState("Avoiding");
                            break;
                    }
                }
            }
        }
    }
    

    public void SetState(string moveState)
    {
        switch (moveState)
        {
            case "Wandering":
                currentState = StateEnum.Wandering;
                break;
            case "Following":
                currentState = StateEnum.Following;
                break;
            case "Avoiding":
                currentState = StateEnum.Avoiding;
                break;
            case "Turning_Back":
                currentState = StateEnum.Turning_Back;
                break;
            case "Resting":
                currentState = StateEnum.Resting;
                break;
            default:
                Debug.LogWarning("void SetState() in State.cs: undefined");
                break;
        }
    }
}
