using UnityEngine;

public static class MathV
{
    public static Vector3 ToAxisVector(this Quaternion quaternion)
    {
        float _y = quaternion.z * 0.0111111111111111f;
        float _x = 1 - _y;
        return new Vector3(_x, _y, 0);
    }

    public static float RandomAround(float value, int range = 4)
    {
        return value + Random.Range(-value / range, value / range);
    }

    public static float RoundTo(this float number, ushort decimalPlaces)
    {
        float x = 1;
        for (int i = 1; i <= decimalPlaces; i++) x *= 10.0f;
        float n = number * x;
        return (int)n / x;
    }
}
