using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static void Shuffle<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int r = Random.Range(0, array.Length);
            int r2 = r;
            
            while (r == r2)
            {
                r2 = Random.Range(0, array.Length);
            }

            var temp = array[r];

            array[r] = array[r2];
            array[r2] = temp;
        }
    }
}
