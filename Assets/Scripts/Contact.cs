using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Plant(Green)":
            case "Plant(Red)":
            case "Plant(White)":
                GetComponent<Survival>().GainEnergy(collision.collider.tag);
                Destroy(collision.collider.gameObject);
                break;
            case "Animal":
                if (GetComponent<Fitness>().Evaluate(collision.collider))
                {
                    GetComponent<Fitness>().prevMatingTime = Time.time;
                    GetComponent<Crossover>().Create(this.name, collision.collider.name);
                    Debug.Log(this.name + ", Crossover - Create() with: " + collision.collider.name);
                }
                break;
            case "Wall":
                Debug.Log(this.name + " Collision: Wall");
                GetComponent<State>().SetState("Turning_Back");
                break;
            case "water":
                //Debug.Log(this.name + " Collision: Water");
                GetComponent<State>().SetState("Turning_Back");
                break;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Animal":
                if (GetComponent<Fitness>().Evaluate(collision.collider))
                {
                    GetComponent<Fitness>().prevMatingTime = Time.time;
                    GetComponent<Crossover>().Create(this.name, collision.collider.name);
                    Debug.Log(this.name + ", Crossover - Create() with: " + collision.collider.name);
                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Water"))
        {
            //Debug.Log(this.name + " Trigger: Water");
            GetComponent<State>().SetState("Turning_Back");
        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Water"))
        {
            //Debug.Log(this.name + " TriggerStay: Water");
            //set move to true as long as trigger stay is true
        }
    }
}