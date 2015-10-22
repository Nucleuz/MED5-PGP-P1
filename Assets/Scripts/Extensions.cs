using UnityEngine;
using System.Collections;
using DarkRift;

public static class Extensions{

    //---------------------
    // Serialize
    //--------------------

    public static void Serialize(this Vector3 v,ref DarkRiftWriter writer){
        writer.Write(new float[3]{v.x,v.y,v.z});
    }
    

    public static void Serialize(this Quaternion q, ref DarkRiftWriter writer){
        writer.Write(new float[4]{q.x,q.y,q.z,q.w});
    }
}
