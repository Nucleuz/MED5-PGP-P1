using UnityEngine;
using System.Collections;

using DarkRift;
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

    public static DarkRiftWriter Serialize(this Vector3 v){
        using(DarkRiftWriter writer = new DarkRiftWriter()){
            writer.Write(v.x);
            writer.Write(v.y);
            writer.Write(v.z);

            return writer;
        }
    }

    public static Vector3 Deserialize(object data){
        if(data is DarkRiftReader){
            using(DarkRiftReader reader = (DarkRiftReader)data){
                Vector3 v = new Vector3(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()
                );
                return v;
            }
        }else{
            Debug.LogError("data is not a darkriftreader");
            return Vector3.zero;
        }
    }
}
