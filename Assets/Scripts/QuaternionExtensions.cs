using UnityEngine;
using System.Collections;
using DarkRift;

public static class QuaternionExtensions{


    public static DarkRiftWriter Serialize(this Quaternion q){
        using(DarkRiftWriter writer = new DarkRiftWriter()){
            writer.Write(q.x);
            writer.Write(q.y);
            writer.Write(q.z);
            writer.Write(q.w);

            return writer;
        }
    }

    public static Quaternion Deserialize(object data){
        if(data is DarkRiftReader){
            using(DarkRiftReader reader = (DarkRiftReader)data){
                Quaternion q = new Quaternion(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                );
                return q;
            }
        }else{
            Debug.LogError("data is not a darkriftreader");
            return Quaternion.identity;
        }
    }
}
