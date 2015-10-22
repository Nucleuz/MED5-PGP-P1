using UnityEngine;
using System.Collections;
using System.IO;
using DarkRift;

    /* By KasperHdL 
     * Currently theere is some duplicative code but cant see an easy way to minimize this without a performance hit
     *
     */

public static class Deserializer{
    public static Vector3 Vector3(byte[] data){
        Vector3 v = new Vector3();

        using (MemoryStream m = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(m))
            {
                Debug.Log(reader.BaseStream.Length);
                int i = 0;
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    v[i++] = reader.ReadSingle();
                }
            }
        }
        return v;
    }

    public static Quaternion Quaternion(byte[] data){
        Quaternion q = new Quaternion();

        using (MemoryStream m = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(m))
            {
                Debug.Log(reader.BaseStream.Length);
                int i = 0;
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    q[i++] = reader.ReadSingle();
                }
            }
        }
        return q;
    }
}
