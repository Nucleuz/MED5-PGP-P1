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
		public class TBEngine
		{
			#if UNITY_IOS && !UNITY_EDITOR
			const string DLL_NAME = "__Internal";
			#else
			const string DLL_NAME = "AkSoundEngine";
			#endif	
			
			[DllImport(DLL_NAME)]
			static extern void TBEngine_setSpeedOfSound(float speedOfSound);

			[DllImport(DLL_NAME)]
			static extern void TBEngine_setWorldScale(float worldScaleInMetres);

			[DllImport(DLL_NAME)]
			static extern TBLicenceCheck TBEngine_getLicenseStatus();
			
			[DllImport(DLL_NAME)]
			static extern TBLicenceCheck TBEngine_getLicenseType();

			public static void setSpeedOfSound (float speedOfSound)
			{
				TBEngine_setSpeedOfSound(speedOfSound);
			}
			
			public static void setWorldScale(float worldScaleInMetres)
			{
				TBEngine_setWorldScale(worldScaleInMetres);
			}

			public static TBLicenceCheck getLicenseStatus()
			{
				return TBEngine_getLicenseStatus ();
			}
			
			public static TBLicenceCheck getLicenseType()
			{
				return TBEngine_getLicenseType ();
			}
		}
	}
}

