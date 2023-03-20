using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Crossover : MonoBehaviour
{
    public GameObject prefab;
    float[] newGenes;
    public string[] parents;
    public void Create(string nameSelf, string nameOther)
    {
        parents = new string[2];
        newGenes = new float[16];

        parents[0] = nameSelf;
        parents[1] = nameOther;
        Debug.Log(this.name + " Create(" + nameSelf + ", " + nameOther + ")");

        string filePathSelf = Path.Combine(@"Assets/Data/", $"{nameSelf}.txt");
        string filePathOther = Path.Combine(@"Assets/Data/", $"{nameOther}.txt");
        StreamReader readerSelf = new StreamReader(filePathSelf);
        StreamReader readerOther = new StreamReader(filePathOther);

        for (int i = 0; i < newGenes.Length; i++)
        {
            string[] strSelf = readerSelf.ReadLine().Split(':');
            string[] strOther = readerOther.ReadLine().Split(':');

            if (Random.Range(0, 100) < 5) //5% chance of mutation
            {
                newGenes[i] = Random.Range(1, 100);
                Debug.Log(this.name + " Mutation: " + strSelf[0]);
            }
            else
            {
                if (i <= 6) //Crossover point
                    newGenes[i] = int.Parse(strSelf[1]);
                else
                    newGenes[i] = int.Parse(strOther[1]);
            }

            Debug.Log(strSelf[0] + ": " + newGenes[i]);
        }
        readerSelf.Close();
        readerOther.Close();

        string filePathNew = Path.Combine(@"Assets/Data/", $"Rabbit({Directory.GetFiles(@"Assets/Data/", "*.txt").Length + 1}).txt");

        if (!File.Exists(filePathNew))
        {
            Debug.Log("AppendingText to " + filePathNew);
            File.AppendAllText(filePathNew, "Move Speed" + ":" + newGenes[0].ToString() + "\n");
            File.AppendAllText(filePathNew, "Turn Speed" + ":" + newGenes[1].ToString() + "\n");

            File.AppendAllText(filePathNew, "Number of Raycasts" + ":" + newGenes[2].ToString() + "\n");
            File.AppendAllText(filePathNew, "Raycast Angle" + ":" + newGenes[3].ToString() + "\n");
            File.AppendAllText(filePathNew, "Raycast Distance" + ":" + newGenes[4].ToString() + "\n");
            File.AppendAllText(filePathNew, "Detection Sphere Radius" + ":" + newGenes[5].ToString() + "\n");

            File.AppendAllText(filePathNew, "Lévy Tendency" + ":" + newGenes[6].ToString() + "\n");
                      
            File.AppendAllText(filePathNew, "Eating Green" + ":" + newGenes[7].ToString() + "\n");
            File.AppendAllText(filePathNew, "Eating Red" + ":" + newGenes[8].ToString() + "\n");
            File.AppendAllText(filePathNew, "Eating White" + ":" + newGenes[9].ToString() + "\n");
                       
            File.AppendAllText(filePathNew, "Hunger Rate" + ":" + newGenes[10].ToString() + "\n");
            File.AppendAllText(filePathNew, "Hunger Max" + ":" + newGenes[11].ToString() + "\n");
            File.AppendAllText(filePathNew, "Hunger Recovery" + ":" + newGenes[12].ToString() + "\n");
                             
            File.AppendAllText(filePathNew, "Aging Rate" + ":" + newGenes[13].ToString() + "\n");
            File.AppendAllText(filePathNew, "Aging Max" + ":" + newGenes[14].ToString() + "\n");

            //File.AppendAllText(filePathNew, "Mating Cost" + ":" + newGenes[15].ToString() + "\n");

            File.AppendAllText(filePathNew, "Gender" + ":" + Random.Range(0,100).ToString() + "\n");

            File.AppendAllText(filePathNew, "Parents:" + "\n");
            File.AppendAllText(filePathNew, parents[0] + "\n");
            File.AppendAllText(filePathNew, parents[1] + "\n");
        }

        prefab.GetComponent<Genes>().isChild = true;
        Instantiate(prefab, this.transform.position + (transform.up * 2), this.transform.rotation, transform.parent);
        //GetComponent<Survival>().ExpendEnergy();

        //Debug.Break();
    }
}
