using UnityEngine;
using System.Collections;
using System.IO;
using DarkRift;

public static class Extensions{

    //---------------------
    // Serialize
    //--------------------

    public static byte[] Serialize(this Vector3 v){
        using (MemoryStream m = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(v.x);
                writer.Write(v.y);
                writer.Write(v.z);

                return m.ToArray();
            }
        }
    }
    

    public static byte[] Serialize(this Quaternion q){
         using (MemoryStream m = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(q.x);
                writer.Write(q.y);
                writer.Write(q.z);
                writer.Write(q.w);

                return m.ToArray();
            }
        }
    }
}
