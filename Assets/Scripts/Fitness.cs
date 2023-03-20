using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fitness : MonoBehaviour
{
    public float initTime;
    public float prevMatingTime;
    void Start()
    {
        initTime = Time.time;
        prevMatingTime = Time.time;
    }


    public bool Evaluate(Collider collider)
    {
        //if (collider.GetComponent<Genes>().GetGene("Gender") % 2 != this.GetComponent<Genes>().GetGene("Gender") % 2)
        {

            if (GetComponent<Survival>().hungerCurrent > GetComponent<Survival>().hungerMax / 2 && collider.GetComponent<Survival>().hungerCurrent > collider.GetComponent<Survival>().hungerMax / 2)
            {
                if (Time.time - initTime > 15f && Time.time - prevMatingTime > 5f)
                {
                    if (Time.time - collider.GetComponent<Fitness>().initTime > 15f && Time.time - collider.GetComponent<Fitness>().prevMatingTime > 5f)
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }
}
