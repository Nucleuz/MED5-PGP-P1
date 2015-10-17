// Copyright (c) 2015 Two Big Ears Ltd.
// All Rights Reserved
// TwoBigEars.com

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

namespace TBE
{
	namespace Wwise
	{
		public class TBRoom
		{
			#if UNITY_IOS && !UNITY_EDITOR
			const string DLL_NAME = "__Internal";
			#else
			const string DLL_NAME = "AkSoundEngine";
			#endif	
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_init();
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_addRoom(int iRoomId);
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_removeRoom(int iRoomId);
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_setRoomCentre(int iRoomId, TBVector CentrePosition, TBVector ForwardVector, TBVector UpVector, TBVector Scale);
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_setRoomProperties(int iRoomId, TBRoomProperties RoomProperties);
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_setRoomDiffuseZone(int iRoomId, float fDiffuseZoneSize);
			
			[DllImport(DLL_NAME)]
			static extern void TBRoomMan_update();
			
			public static void init ()
			{
				TBRoomMan_init ();
			}
			
			public static void addRoom (int iRoomId)
			{
				TBRoomMan_addRoom (iRoomId);
			}
			
			public static void removeRoom (int iRoomId)
			{
				TBRoomMan_removeRoom (iRoomId);
			}
			
			public static void setRoomCentre (int iRoomId, Vector3 CentrePosition, Vector3 ForwardVector, Vector3 UpVector, Vector3 Scale)
			{	
				TBVector tbPosition = Utils.convertVector(CentrePosition);
				TBVector tbForward = Utils.convertVector(ForwardVector);
				TBVector tbUp = Utils.convertVector(UpVector);
				TBVector tbScale = Utils.convertVector(Scale);
				TBRoomMan_setRoomCentre(iRoomId, tbPosition, tbForward, tbUp, tbScale);
			}
			
			public static void setRoomProperties (int iRoomId, TBRoomProperties RoomProperties)
			{
				TBRoomMan_setRoomProperties(iRoomId, RoomProperties);
			}
			
			public static void setRoomDiffuseZone (int iRoomId, float fDiffuseZoneSize)
			{
				TBRoomMan_setRoomDiffuseZone (iRoomId, fDiffuseZoneSize);
			}
			
			public static void update ()
			{
				TBRoomMan_update ();
			}
		}
	}
}

