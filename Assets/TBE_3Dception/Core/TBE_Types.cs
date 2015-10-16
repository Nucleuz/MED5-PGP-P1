// Copyright (c) 2015 Two Big Ears Ltd.
// All Rights Reserved
// TwoBigEars.com

using UnityEngine;
using System.Runtime.InteropServices;

namespace TBE 
{	
	namespace Wwise
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct TBVector 
		{
			public float x;
			public float y;
			public float z;
			
			public TBVector (float xValue, float yValue, float zValue) {
				
				x = xValue;
				y = yValue;
				z = zValue;
			}
		}

		public enum TBErrorCode
		{
			TBE_E_ALREADY_INIT = 1,             /*!< Engine is already initialised */
			TBE_E_SUCCESS = 0,                  /*!< Sucess, all is ok */
			TBE_E_FAIL = -1,                    /*!< Failure, all is not ok */
			TBE_E_NO_NEON = -2,                 /*!< ARM NEON is not supported on device */
			TBE_E_ENGINE_NOT_INIT = -3,         /*!< Engine isn't initialised */
			TBE_E_SOURCE_LIMIT = -4,            
			TBE_E_BAD_PTR = -5,					/*! input pointer not valid*/
			TBE_E_BAD_DATA = -6,				/*! problem with input data*/
			TBE_E_NOT_INITIALISED = -7	
		};
		
		public enum TBLicenceCheck
		{	
			TBE_E_LICENSE_PRO = 3,              /*!< Pro license type */
			TBE_E_LICENSE_EVAL = 2,             /*!< Timed evaluation license type */
			TBE_E_LICENSE_VALID = 0,            /*!< License is valid */
			TBE_E_LICENSE_INVALID = -1,         /*!< Invalid licensing details */
			TBE_E_LICENSE_TIMEOUT = -2,         /*!< Evaluation license has timed out */
		};
		
		[StructLayout(LayoutKind.Sequential)]
		public struct TBRoomProperties
		{
			public float fHfDampening;
			public float fAmpDampening;
			public float fAmpDampeningCeiling;
			public float fAmpDampeningFloor;
			public float fAmpDampeningRWall;
			public float fAmpDampeningFWall;
			public float fAmpDampeningLWall;
			public float fAmpDampeningBWall;
			public float fDiffuseZoneSize;
			
			public TBRoomProperties(float HfDampening,
			                        float AmpDampening,
			                        float AmpDampeningCeiling,
			                        float AmpDampeningFloor,
			                        float AmpDampeningRWall,
			                        float AmpDampeningFWall,
			                        float AmpDampeningLWall,
			                        float AmpDampeningBWall,
			                        float DiffuseZoneSize)
			{
				fHfDampening = HfDampening;
				fAmpDampening = AmpDampening;
				fAmpDampeningCeiling = AmpDampeningCeiling;
				fAmpDampeningFloor = AmpDampeningFloor;
				fAmpDampeningRWall = AmpDampeningRWall;
				fAmpDampeningFWall = AmpDampeningFWall;
				fAmpDampeningLWall = AmpDampeningLWall;
				fAmpDampeningBWall = AmpDampeningBWall;
				fDiffuseZoneSize = DiffuseZoneSize;
			}
			
		}
		
		
		[StructLayout(LayoutKind.Sequential)]
		public struct TBRoomSettings
		{
			public float fHfDampening;
			public float fAmpDampening;
			public float fAmpDampeningCeiling;
			public float fAmpDampeningFloor;
			public float fAmpDampeningRWall;
			public float fAmpDampeningFWall;
			public float fAmpDampeningLWall;
			public float fAmpDampeningBWall;
			
			public TBRoomSettings(float HfDampening,
			                      float AmpDampening,
			                      float AmpDampeningCeiling,
			                      float AmpDampeningFloor,
			                      float AmpDampeningRWall,
			                      float AmpDampeningFWall,
			                      float AmpDampeningLWall,
			                      float AmpDampeningBWall)
			{
				fHfDampening = HfDampening;
				fAmpDampening = AmpDampening;
				fAmpDampeningCeiling = AmpDampeningCeiling;
				fAmpDampeningFloor = AmpDampeningFloor;
				fAmpDampeningRWall = AmpDampeningRWall;
				fAmpDampeningFWall = AmpDampeningFWall;
				fAmpDampeningLWall = AmpDampeningLWall;
				fAmpDampeningBWall = AmpDampeningBWall;
			}
			
		}
		
		public class PresetSettings
		{
			public static TBRoomSettings Glass() 
			{	
				TBRoomSettings result;
				result.fHfDampening = 0.8f;
				result.fAmpDampening = 0.5f;
				result.fAmpDampeningCeiling = 0.97f;
				result.fAmpDampeningFloor = 0.97f;
				result.fAmpDampeningRWall = 0.97f;
				result.fAmpDampeningFWall = 0.97f;
				result.fAmpDampeningLWall = 0.97f;
				result.fAmpDampeningBWall = 0.97f;
				return result;
			}
			
			public static TBRoomSettings Default() 
			{	
				TBRoomSettings result;
				result.fHfDampening = 1;
				result.fAmpDampening = 1;
				result.fAmpDampeningCeiling = 1;
				result.fAmpDampeningFloor = 1;
				result.fAmpDampeningRWall = 1;
				result.fAmpDampeningFWall = 1;
				result.fAmpDampeningLWall = 1;
				result.fAmpDampeningBWall = 1;
				return result;
			}
			
			public static TBRoomSettings ConcreteRoom() 
			{	
				TBRoomSettings result;
				result.fHfDampening = 1;
				result.fAmpDampening = 1;
				result.fAmpDampeningRWall = 0.94f;
				result.fAmpDampeningFWall = 0.94f;
				result.fAmpDampeningLWall = 0.94f;
				result.fAmpDampeningBWall = 0.94f;
				result.fAmpDampeningCeiling = 0.94f;
				result.fAmpDampeningFloor = 0.94f;;
				return result;
			}
			
			public static TBRoomSettings RecordingStudio() 
			{	
				TBRoomSettings result;
				result.fHfDampening = 1;
				result.fAmpDampening = 1;
				result.fAmpDampeningRWall = 0.42f;
				result.fAmpDampeningFWall = 0.25f;
				result.fAmpDampeningLWall = 0.42f;
				result.fAmpDampeningBWall = 0.42f;
				result.fAmpDampeningCeiling = 0.23f;
				result.fAmpDampeningFloor = 0.75f;
				return result;
			}
			
			public static TBRoomSettings LivingRoom() 
			{	
				TBRoomSettings result;
				result.fHfDampening = 1;
				result.fAmpDampening = 1;
				result.fAmpDampeningRWall = 0.78f;
				result.fAmpDampeningFWall = 0.78f;
				result.fAmpDampeningLWall = 0.78f;
				result.fAmpDampeningBWall = 0.78f;
				result.fAmpDampeningCeiling = 0.96f;
				result.fAmpDampeningFloor = 0.93f;
				return result;
			}
			
			
			public static TBRoomSettings Outdoor() 
			{	
				TBRoomSettings result;
				result.fHfDampening = 1;
				result.fAmpDampening = 1;
				result.fAmpDampeningRWall = 0.01f;
				result.fAmpDampeningFWall = 0.01f;
				result.fAmpDampeningLWall = 0.01f;
				result.fAmpDampeningBWall = 0.01f;
				result.fAmpDampeningCeiling = 0.01f;
				result.fAmpDampeningFloor = 0.94f;
				return result;
			}
		}
		
		public enum TBRoomPresets
		{
			Custom = 0,
			Default = 1,
			Glass = 2,
			ConcreteRoom = 3,
			RecordingStudio = 4,
			LivingRoom = 5,
			Outdoor = 6
		}
		
		public class Utils
		{
			
			public static TBVector convertVector(Vector3 vector)
			{
				TBVector result;
				result.x = vector.x;
				result.y = vector.y;
				result.z = vector.z;
				return result;
			}
			
		}
	}
}
