using UnityEngine;
using System.Collections;
public static class VectorExtensions {
     
    public static Vector2 Rotate(this Vector2 v, float radians) {
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
         
        float tx = v.x;
        float ty = v.y;

        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
    }
}
