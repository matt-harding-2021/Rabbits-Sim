using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Genes : MonoBehaviour
{
    public bool isChild = false;
    struct GeneData
    {
        public string Name;
        public float Value;

        public void SetGene(string arg_name, float arg_value)
        {
            this.Name = arg_name;
            this.Value = arg_value;
        }
    }
    GeneData[] genes;
    public float[] geneVals;

    void Awake()
    {
        genes = new GeneData[16];
        geneVals = new float[16];

        if (isChild) this.name = "Rabbit(" + (Directory.GetFiles(@"Assets/Data/", "*.txt").Length) + ")";
        else this.name = "Rabbit(" + (Directory.GetFiles(@"Assets/Data/", "*.txt").Length + 1) + ")";

        Debug.Log(this.gameObject.name + ": Awake()");

        string filePath = Path.Combine(@"Assets/Data/", $"{this.name}.txt");

        if (!File.Exists(filePath)) 
            WriteFile(filePath);

        ReadFile(filePath);

        Adjust();

        for (int i = 0; i < genes.Length; i++) geneVals[i] = genes[i].Value; //Public array used to view the adjusted genes in the editor
    }

    void WriteFile(string filepath)
    {
        //--CAPABILITIES--//

        //Speed//
        File.AppendAllText(filepath, "Move Speed" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Turn Speed" + ":" + Random.Range(1, 100).ToString() + "\n");

        //Detection//
        File.AppendAllText(filepath, "Number of Raycasts" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Raycast Angle" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Raycast Distance" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Detection Sphere Radius" + ":" + Random.Range(1, 100).ToString() + "\n");

        //Exploration
        File.AppendAllText(filepath, "Lévy Tendency" + ":" + Random.Range(1, 100).ToString() + "\n");



        //--NEEDS--//

        //Diet//
        File.AppendAllText(filepath, "Eating Green" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Eating Red" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Eating White" + ":" + Random.Range(1, 100).ToString() + "\n");

        //Hunger//
        File.AppendAllText(filepath, "Hunger Rate" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Hunger Max" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Hunger Recovery" + ":" + Random.Range(1, 100).ToString() + "\n");

        //Age//
        File.AppendAllText(filepath, "Aging Rate" + ":" + Random.Range(1, 100).ToString() + "\n");
        File.AppendAllText(filepath, "Aging Max" + ":" + Random.Range(1, 100).ToString() + "\n");

        //Mating Cost//
        //File.AppendAllText(filepath, "Mating Cost" + ":" + Random.Range(1, 100).ToString() + "\n");


        //--GENDER--//
        File.AppendAllText(filepath, "Gender" + ":" + Random.Range(1, 100).ToString() + "\n");
    }
    void ReadFile(string filepath)
    {
        StreamReader reader = new StreamReader(filepath);
        for (int i = 0; i < genes.Length; i++)
        {
            string[] str = reader.ReadLine().Split(':');
            genes[i].SetGene(str[0], int.Parse(str[1]));
        }
        reader.Close();
    }

    public void Adjust()
    {
        //Debug.Log("Adjusting " + this.name);

        //"Average" and "Influencing" values for each group of genes
        //Capabilities
        float SpeedAvg = (genes[0].Value + genes[1].Value) / 2f,
              SpeedInf = (100f - SpeedAvg),

              DetectAvg = (genes[2].Value + genes[3].Value + genes[4].Value + genes[5].Value) / 4f,
              DetectInf = (100f - DetectAvg),

              ExploreAvg = (genes[6].Value) / 1f,
              ExploreInf = (100f - ExploreAvg);

        //Needs
        float DietAvg = (genes[7].Value + genes[8].Value + genes[9].Value) / 3f,
              DietInf = (100f - DietAvg),

              HungerAvg = (genes[10].Value + genes[11].Value + genes[12].Value) / 3f,
              HungerInf = (100f - HungerAvg),

              AgeAvg = (genes[13].Value + genes[14].Value) / 2f,
              AgeInf = (100f - AgeAvg);

              //MatingCostAvg = (genes[15].Value) / 1f,
              //MatingCostInf = (100f - MatingCostAvg);

        //Debug.Log("Speed Av: " + SpeedAvg + " Speed In: " + SpeedInf);
        //Debug.Log("Detect Av: " + DetectAvg + " Detect In: " + DetectInf);
        //Debug.Log("Explore Av: " + ExploreAvg + " Explore In: " + ExploreInf);

        //Calculate the new average with the influencing variables.
        /* New Average = (
         * Original Average + 
         * Sum of Influences / Num of Influences
         * ) / 2
         */
        float Influenced_SpeedAvg = (SpeedAvg + (DetectInf + ExploreInf) / 2f) / 2f,
              Influenced_DetectAvg = (DetectAvg + (SpeedInf + ExploreInf) / 2f) / 2f,
              Influenced_ExploreAvg = (ExploreAvg + (SpeedInf + DetectInf) / 2f) / 2f;

        //Debug.Log("Inf Speed Av: " + Influenced_SpeedAvg);
        //Debug.Log("Inf Detect Av: " + Influenced_DetectAvg);
        //Debug.Log("Inf Explore Av: " + Influenced_ExploreAvg);
        //Debug.Log("S.D.E: " + (Influenced_SpeedAvg + Influenced_DetectAvg + Influenced_ExploreAvg)); //Always = 50 * Number of groups

        float Influenced_DietAvg = (DietAvg + (HungerInf + AgeInf) / 2f) / 2f,  //+ MatingCostInf) / 3f)
              Influenced_HungerAvg = (HungerAvg + (DietInf + AgeInf) / 2f) / 2f,
              Influenced_AgeAvg = (AgeAvg + (DietInf + HungerInf) / 2f) / 2f;
              //Influenced_MatingCostAvg = (MatingCostAvg + (DietInf + HungerInf + AgeInf) / 3f) / 2f;

        //Debug.Log("D.H.T: " + (Influenced_DietAvg + Influenced_HungerAvg + Influenced_AgeAvg)); // + Influenced_MatingCostAvg)); //Always = 50 * Number of groups

        //Multiply each gene by (influenced avg / original avg)
        //Capabilities
        genes[0].Value *= Influenced_SpeedAvg / SpeedAvg;
        genes[1].Value *= Influenced_SpeedAvg / SpeedAvg;

        genes[2].Value *= Influenced_DetectAvg / DetectAvg;
        genes[3].Value *= Influenced_DetectAvg / DetectAvg;
        genes[4].Value *= Influenced_DetectAvg / DetectAvg;
        genes[5].Value *= Influenced_DetectAvg / DetectAvg;

        genes[6].Value *= Influenced_ExploreAvg / ExploreAvg;

        //Needs
        genes[7].Value *= Influenced_DietAvg / DietAvg;
        genes[8].Value *= Influenced_DietAvg / DietAvg;
        genes[9].Value *= Influenced_DietAvg / DietAvg;

        genes[10].Value *= Influenced_HungerAvg / HungerAvg;
        genes[11].Value *= Influenced_HungerAvg / HungerAvg;
        genes[12].Value *= Influenced_HungerAvg / HungerAvg;

        genes[13].Value *= Influenced_AgeAvg / AgeAvg;
        genes[14].Value *= Influenced_AgeAvg / AgeAvg;

        //genes[15].Value *= Influenced_MatingCostAvg / MatingCostAvg;

        for (int i = 0; i < genes.Length; i++)
        {
            if (genes[i].Value > 100f) genes[i].Value = 100;
            else if (genes[i].Value < 0f) genes[i].Value = 0;
        }


        //genes[16] is Gender

        //foreach (GeneData gene in genes) Debug.Log(gene.Name + " : " + gene.Value);
    }

    public float GetGene(string arg_name)
    {
        foreach (GeneData gene in genes) 
            if (gene.Name == arg_name) return gene.Value;

        Debug.LogError("int GetGene(" + arg_name + ") in Genes.cs: gene not found");
        return 0;
    }

    public void SetGenes(int[] newGenes)
    {
        for (int i = 0; i < genes.Length; i++) 
            genes[i].Value = newGenes[i];
        Adjust();
    }
}