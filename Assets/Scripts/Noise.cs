using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{

	int[] P;

    void Awake()
    {
		P = new int[512];
		for (int i = 0; i < 255; i++)
		{
			P[i] = i;
		}
		Shuffle(P);
		for (int i = 256; i < 512; i++)
		{
			P[i] = P[i- 256];
		}
    }

	private void Shuffle(int[] Permutation)
	{
		for (int e = Permutation.Length - 1; e > 0; e--)
		{
			int index = (int)Mathf.Round(Random.Range(0.0f, 1.0f) * (e - 1)),
				temp = Permutation[e];

			Permutation[e] = Permutation[index];
			Permutation[index] = temp;
		}
	}

	private Vector2 GetConstantVector(int val)
	{
		if (val % 4 == 0)
			return new Vector2(1.0f, 1.0f);
		else if (val % 4 == 1)
			return new Vector2(-1.0f, 1.0f);
		else if (val % 4 == 2)
			return new Vector2(-1.0f, -1.0f);
		else
			return new Vector2(1.0f, -1.0f);
	}

	private float Fade(float t)
	{
		return ((6 * t - 15) * t + 10) * t * t * t; //
	}

	private float Lerp(float t, float a1, float a2)
	{
		return a1 + t * (a2 - a1);
	}

	public float Noise2D(float x, float y)
	{
		int iX = (int)Mathf.Floor(x) & 255;
		int iY = (int)Mathf.Floor(y) & 255;

		//Get the decimal position of the point withiin the cell
		//This takes away the cell's position within the grid
		float xf = x - Mathf.Floor(x);
		float yf = y - Mathf.Floor(y);

		//Find the difference vector
		Vector2 topRight = new Vector2(xf - 1.0f, yf - 1.0f);
		Vector2 topLeft = new Vector2(xf, yf - 1.0f);
		Vector2 lowerRight = new Vector2(xf - 1.0f, yf);
		Vector2 lowerLeft = new Vector2(xf, yf);

		int valueTopRight = P[P[iX + 1] + iY + 1];
		int valueTopLeft = P[P[iX] + iY + 1];
		int valueLowerRight = P[P[iX + 1] + iY];
		int valueLowerLeft = P[P[iX] + iY];

		float dotTopRight = Vector3.Dot(topRight, GetConstantVector(valueTopRight));
		float dotTopLeft = Vector3.Dot(topLeft, GetConstantVector(valueTopLeft));
		float dotLowerRight = Vector3.Dot(lowerRight, GetConstantVector(valueLowerRight));
		float dotLowerLeft = Vector3.Dot(lowerLeft, GetConstantVector(valueLowerLeft));


		return Lerp(Fade(xf),
			Lerp(Fade(yf), dotLowerLeft, dotTopLeft),
			Lerp(Fade(yf), dotLowerRight, dotTopRight)
		);
	}
}