using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour
{
    float timeAccum = 0;

    float hungerRate;
    public float hungerMax;
    public float hungerCurrent;

    float agingRate;
    float agingMax;
    float agingCurrent;

    void Start()
    {
        hungerRate = (1f + Mathf.Ceil((100f - GetComponent<Genes>().GetGene("Hunger Rate")) / 10f)) / 2f;
        hungerMax = GetComponent<Genes>().GetGene("Hunger Max");
        hungerCurrent = hungerMax / 2f;
        //Debug.Log("hunegr current: " + (hungerCurrent / hungerMax) * 100);
        /*
        Debug.Log("HCurrent: " + hungerCurrent);
        Debug.Log("HRate Gene: " + GetComponent<Genes>().GetGene("Hunger Rate"));
        Debug.Log("HRate: " + hungerRate);
        */

        agingRate = (1f + Mathf.Ceil((100f - GetComponent<Genes>().GetGene("Aging Rate")) / 10f)) / 2f;
        agingMax = GetComponent<Genes>().GetGene("Aging Max") * 2;
        agingCurrent = 0;
    }

    void Update()
    {
        timeAccum += Time.deltaTime;

        if (timeAccum >= 1f)
        {
            if (hungerCurrent > 0)
            {
                hungerCurrent -= hungerRate/2;
                timeAccum = 0;
            }
            else if (hungerCurrent <= 0)
            {
                Debug.Log("Destroy: " + this.name + " Hunger < 0: " + hungerCurrent);
                Destroy(this.gameObject);
            }

            if (agingCurrent < agingMax) {
                agingCurrent += agingRate/2;
            }
            else if (agingCurrent >= agingMax)
            {
                Debug.Log("Destroy: " + this.name + " Age > Max: " + agingCurrent);
                Destroy(this.gameObject);
            }
        }
    }

    public void GainEnergy(string tag)
    {
        switch (tag)
        {
            case "Plant(Green)":
                hungerCurrent += (GetComponent<Genes>().GetGene("Eating Green") / 100f) * GetComponent<Genes>().GetGene("Hunger Recovery");
                break;
            case "Plant(Red)":
                hungerCurrent += (GetComponent<Genes>().GetGene("Eating Red") / 100f) * GetComponent<Genes>().GetGene("Hunger Recovery");
                break;
            case "Plant(White)":
                hungerCurrent += (GetComponent<Genes>().GetGene("Eating White") / 100f) * GetComponent<Genes>().GetGene("Hunger Recovery");
                break;
        }
        if (hungerCurrent > hungerMax) hungerCurrent = hungerMax;
    }

    public void ExpendEnergy()
    {
        hungerCurrent -= (100 - GetComponent<Genes>().GetGene("Mating Cost"));
    }
}
