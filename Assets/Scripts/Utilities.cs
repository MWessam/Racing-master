
using System;
public static class Utilities
{
    public static int LoopIndex(int index, int length)
    {
        if (index < 0)
        {
            return length - 1;
        }
        if (index < length)
        {
            return index;
        }
        else if (index >= length)
        {
            return 0;
        }
        return 0;
    }
}
static class RandomExtensions
{
    static private Random s_rng = new Random();
    public static T[] Shuffle<T>(this T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = s_rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
        return array;
    }
}