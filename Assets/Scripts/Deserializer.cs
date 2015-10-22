using UnityEngine;
using System.Collections;
using DarkRift;

public static class Deserializer{




    public static Vector3 Vector3(object data){
        Vector3 v = new Vector3();
        if(data is DarkRiftReader){
            using(DarkRiftReader reader = (DarkRiftReader)data){
                float[] singles = reader.ReadSingles();

                for(int i = 0;i<singles.Length;i++)
                    v[i] = singles[i];
            }
        }else{
            Debug.LogError("data is not a darkriftreader");
        }
        return v;
    }

    public static Quaternion Quaternion(object data){
        Quaternion q = new Quaternion();
        if(data is DarkRiftReader){
            using(DarkRiftReader reader = (DarkRiftReader)data){
                float[] singles = reader.ReadSingles();

                for(int i = 0;i<singles.Length;i++)
                    q[i] = singles[i];

           }
        }else{
            Debug.LogError("data is not a darkriftreader");
        }
        return q;
    }
}
