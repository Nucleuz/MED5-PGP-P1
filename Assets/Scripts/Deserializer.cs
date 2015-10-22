using UnityEngine;
using System.Collections;
using DarkRift;

public static class Deserializer{




    public static Vector3 Vector3(object data){
        if(data is DarkRiftReader){
            Vector3 v;
            using(DarkRiftReader reader = (DarkRiftReader)data){
                v   = new Vector3(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()
                );
            }
            return v;
        }else{
            Debug.LogError("data is not a darkriftreader");
            return new Vector3();
        }
    }

    public static Quaternion Quaternion(object data){
        if(data is DarkRiftReader){
            Quaternion q;
            using(DarkRiftReader reader = (DarkRiftReader)data){
                q   = new Quaternion(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle()
                );
            }
            return q;
        }else{
            Debug.LogError("data is not a darkriftreader");
            return new Quaternion();
        }
    }
}
