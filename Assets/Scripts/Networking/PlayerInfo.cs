using UnityEngine;
using System.Collections.Generic;
using System;

/*
By KasperHdL

Information being send for the players
//@TODO check if it is more efficient to send them individually
*/

//When using embedded server we need to serialize the that
[Serializable]
public class PlayerInfo{
	public SVector3 position;
	public SQuaternion rotation;
}