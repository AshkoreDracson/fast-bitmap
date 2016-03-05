internal static class Mathf
{
    public static int Clamp(int val, int min, int max)
    {
        if (val < min)
            return min;
        else if (val > max)
            return max;
        return val;
    }
    public static float Clamp01(float val)
    {
        if (val < 0f)
            return 0f;
        else if (val > 1f)
            return 1f;
        return val;
    }
    public static byte ClampByte(byte val)
    {
        if (val < 0)
            return 0;
        else if (val > 255)
            return 255;
        return val;
    }
}