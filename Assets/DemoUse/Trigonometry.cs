using UnityEngine;
using System.Collections;

public class Trigonometry
{
    public Vector3 triSide;
    public Vector3 triAngel;

    public Trigonometry(Vector3 vect)
    {
        triAngel = GetAngelBySide(vect);
    }

    public static Vector3 GetAngelBySide(Vector3 side)
    {
        Vector3 result;
        float a = side.x;
        float b = side.y;
        float c = side.z;
        float a2 = Mathf.Pow(a, 2);
        float b2 = Mathf.Pow(b, 2);
        float c2 = Mathf.Pow(c, 2);
        result.x = Mathf.Acos((b2 + c2 - a2) / (2 * b * c));
        result.y = Mathf.Acos((c2 + a2 - b2) / (2 * c * a));
        result.z = Mathf.Acos((a2 + b2 - c2) / (2 * a * c));
        return (180 * result) / Mathf.PI;
    }

    public static Vector3 GetSideByPoint(Vector3 x, Vector3 y, Vector3 z)
    {
        Vector3 result;
        result.x = Vector3.Magnitude(y - z);
        result.y = Vector3.Magnitude(x - z);
        result.z = Vector3.Magnitude(x - y);
        return result;
    }
    public static bool IsTri(float side1, float side2, float side3)
    {
        if (side1 + side2 <= side3)
            return false;
        if (side1 + side3 <= side2)
            return false;
        if (side2 + side3 <= side1)
            return false;
        return true;
    }

    public static Vector2 GetTirSideRange(float side1, float side2)
    {
        return new Vector2(Mathf.Abs(side1 - side2), side2 + side1);
    }
}
