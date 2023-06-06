using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //---------------------------------------------------------------------------------
    // List ---------------------------------------------------------------------------
    public static void Swap<T>(this List<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        T temp;
        int rnd1, rnd2;

        int times = list.Count < 50 ? list.Count / 2 : list.Count / 3;

        for(int i = 0; i < times; i++)
        {
            rnd1 = Random.Range(0, list.Count);
            rnd2 = Random.Range(0, list.Count);
            temp = list[rnd1];
            list[rnd1] = list[rnd2];
            list[rnd2] = temp;
        }
    }

    //---------------------------------------------------------------------------------
    // Vector3 ------------------------------------------------------------------------
    public static bool IsBehind(this Vector3 queried, Vector3 forward)
    {
        return Vector3.Dot(queried, forward) < 0;
    }

    //---------------------------------------------------------------------------------
    // Math ---------------------------------------------------------------------------
    public static int RoundToMultiples(this int number, int ofX)
    {
        if (number <= ofX)
        {
            return ofX;
        }
        
        int halfOfX = ofX / 2;
        int result;

        if (number % 50 < halfOfX)
        {
            result = number - (number % 50);
        }
        else
        {
            result = ((int)((number - 0.1f) / ofX) + 1) * ofX;
        }

        return result;
    }

    //---------------------------------------------------------------------------------
    // Color --------------------------------------------------------------------------
    public static bool IsApproximatelyEqualTo(this Color color1, Color color2, int maxDifference)
    {
        int r1 = Mathf.RoundToInt(color1.r * 1000);
        int r2 = Mathf.RoundToInt(color2.r * 1000);
        int g1 = Mathf.RoundToInt(color1.g * 1000);
        int g2 = Mathf.RoundToInt(color2.g * 1000);
        int b1 = Mathf.RoundToInt(color1.b * 1000);
        int b2 = Mathf.RoundToInt(color2.b * 1000);
        int a1 = Mathf.RoundToInt(color1.a * 1000);
        int a2 = Mathf.RoundToInt(color2.a * 1000);

        if (Mathf.Abs(r1 - r2) <= maxDifference) r1 = r2;
        if (Mathf.Abs(g1 - g2) <= maxDifference) g1 = g2;
        if (Mathf.Abs(b1 - b2) <= maxDifference) b1 = b2;
        if (Mathf.Abs(a1 - a2) <= maxDifference) a1 = a2;

        bool result = (r1 == r2) && (b1 == b2) && (g1 == g2) && (a1 == a2);

        return result;
    }

}
