using UnityEngine;
using System.Collections.Generic;
using System;

/*
By KasperHdL

	Serialized version of Quaternion

*/
	
[Serializable]
public struct SQuaternion{
	public float x,y,z,w;
	public SQuaternion(Quaternion q){
		this.x = q.x;
		this.y = q.y;
		this.z = q.z;
		this.w = q.w;
	}
	public Quaternion get(){
		return new Quaternion(x,y,z,w);
	}
}