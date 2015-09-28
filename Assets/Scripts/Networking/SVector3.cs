using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct SVector3{
	public float x,y,z;
	public SVector3(Vector3 v){
		this.x = v.x;
		this.y = v.y;
		this.z = v.z;
	}
	public SVector3(float x,float y,float z){
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Vector3 get(){
		return new Vector3(x,y,z);
	}
}