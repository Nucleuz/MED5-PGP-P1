// Copyright (c) 2015 Two Big Ears Ltd.
// All Rights Reserved
// TwoBigEars.com

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

namespace TBE
{
	namespace Wwise
	{	
		/// <summary>
		/// Get and set room properties.
		/// </summary>
		public class TBE_RoomProperties : MonoBehaviour
		{
			
			[HideInInspector]
			[SerializeField]
			private float _absorption = 0.8f;
			
			[HideInInspector]
			[SerializeField]
			private float _dampening = 0.8f;
			
			[HideInInspector]
			[SerializeField]
			private float _reflectionRWall = 1f;
			
			[HideInInspector]
			[SerializeField]
			private float _reflectionFWall = 1f;
			
			[HideInInspector]
			[SerializeField]
			private float _reflectionLWall = 1f;
			
			[HideInInspector]
			[SerializeField]
			private float _reflectionBWall = 1f;
			
			[HideInInspector]
			[SerializeField]
			private float _reflectionFloor = 1f;
			
			[HideInInspector]
			[SerializeField]
			private float _reflectionCeiling = 1f;
			
			[HideInInspector]
			[SerializeField]
			private float _diffuseZone = 1;
			
			[HideInInspector]
			[SerializeField]
			private bool
				_showGuides = false;
			
			[HideInInspector]
			[SerializeField]
			private int _pivotSelect = 0;
			
			private Vector3 pivotVector = new Vector3(0.5f, 0.5f, 0.5f);
			private Vector3 _centerPosition;
			
			private Vector3 colliderScale;
			private Vector3 diffuseScale;
			private Vector3 bottomLeft;
			private Vector3 bottomRight;
			private Vector3 topLeft;
			private Vector3 topRight;
			
			BoxCollider thisBoxCollider;
			
			private int iRoomId = 0;
			
			private TBRoomProperties _RoomProperties;
			
			[HideInInspector]
			[SerializeField]
			private TBRoomPresets _Presets = TBRoomPresets.Default;
			
			void Awake()
			{	
				if (TBEngine.getLicenseStatus () == TBLicenceCheck.TBE_E_LICENSE_TIMEOUT) 
				{
					Debug.LogError("3Dception Wwise Evaluation License Expired. Please contact support@twobigears.com");
				}
				
				setPivot();
				
				iRoomId = GetInstanceID();
				TBRoom.addRoom(iRoomId);
				TBRoom.setRoomCentre(iRoomId, _centerPosition, transform.forward, transform.up, transform.localScale);
				roomPreset = _Presets;
				updateRoomSettings();
				TBRoom.setRoomDiffuseZone(iRoomId, _diffuseZone);
				
			}
			
			void OnDrawGizmosSelected()
			{
				if (!showGuides)
				{
					drawGizmos();
				}
			}
			
			void OnDrawGizmos()
			{
				if (showGuides)
				{
					drawGizmos();
				}	
			}
			
			void Update()
			{
				if (transform.hasChanged) {
					TBRoom.setRoomCentre(iRoomId, _centerPosition, transform.forward, transform.up, transform.localScale);
				}
			}
			
			void FixedUpdate()
			{
				TBRoom.update ();
			}
			
			void drawGizmos()
			{	
				calculateDiffuseScale();
				thisBoxCollider = GetComponent<BoxCollider>();
				Gizmos.matrix = thisBoxCollider.transform.localToWorldMatrix;
				
				Vector3 gizmoPosition = thisBoxCollider.center;
				Gizmos.color = new Color(0.9f, 0.1f, 0.1f, 0.9f);
				Gizmos.DrawWireCube(gizmoPosition, colliderScale);
				
				const float r = 0.1f;
				const float g = 0.5f;
				const float b = 0.3f;
				
				Gizmos.color = new Color(r, g, b, 0.9f);
				Gizmos.DrawWireCube(gizmoPosition, new Vector3(1, 1, 1));
				
				const float limit = 0.6f;
				
				Gizmos.color = new Color(r, g, b, Mathf.Min(_reflectionLWall, limit));
				Vector3 cube1Pos = gizmoPosition;
				Vector3 cube1Scale = new Vector3(1, 1, 1);
				cube1Scale.x = 0.01f;
				cube1Pos.x = -thisBoxCollider.size.x * 0.5f + gizmoPosition.x;
				Gizmos.DrawCube(cube1Pos, cube1Scale);
				
				Gizmos.color = new Color(r, g, b, Mathf.Min (_reflectionRWall, limit));
				Vector3 cube2Pos = gizmoPosition;
				Vector3 cube2Scale = new Vector3(1, 1, 1);
				cube2Scale.x = 0.01f;
				cube2Pos.x = thisBoxCollider.size.x * 0.5f + gizmoPosition.x;
				Gizmos.DrawCube(cube2Pos, cube2Scale);
				
				Gizmos.color = new Color(r, g, b, Mathf.Min (_reflectionFWall, limit));
				Vector3 cube3Pos = gizmoPosition;
				Vector3 cube3Scale = new Vector3(1, 1, 1);
				cube3Scale.z = 0.01f;
				cube3Pos.z = thisBoxCollider.size.z * 0.5f + gizmoPosition.z;
				Gizmos.DrawCube(cube3Pos, cube3Scale);
				
				Gizmos.color = new Color(r, g, b, Mathf.Min (_reflectionBWall, limit));
				Vector3 cube4Pos = gizmoPosition;
				Vector3 cube4Scale = new Vector3(1, 1, 1);
				cube4Scale.z = 0.01f;
				cube4Pos.z = -thisBoxCollider.size.z * 0.5f + gizmoPosition.z;
				Gizmos.DrawCube(cube4Pos, cube4Scale);
				
				Gizmos.color = new Color(r, g, b, Mathf.Min (_reflectionCeiling, limit));
				Vector3 cube5Pos = gizmoPosition;
				Vector3 cube5Scale = new Vector3(1, 1, 1);
				cube5Scale.y = 0.05f;
				cube5Pos.y = thisBoxCollider.size.y * 0.5f + gizmoPosition.y;
				Gizmos.DrawCube(cube5Pos, cube5Scale);
				
				Gizmos.color = new Color(r, g, b, Mathf.Min (_reflectionFloor, limit));
				Vector3 cube6Pos = gizmoPosition;
				Vector3 cube6Scale = new Vector3(1, 1, 1);
				cube6Scale.y = 0.05f;
				cube6Pos.y = -thisBoxCollider.size.y * 0.5f + gizmoPosition.y;
				Gizmos.DrawCube(cube6Pos, cube5Scale);
				
			}
			
			
			void calculateDiffuseScale()
			{
				diffuseScale = transform.localScale + new Vector3(2 * _diffuseZone, 2 * _diffuseZone, 2 * _diffuseZone);
				colliderScale.x = diffuseScale.x / transform.localScale.x; 
				colliderScale.y = diffuseScale.y / transform.localScale.y; 
				colliderScale.z = diffuseScale.z / transform.localScale.z;
				TBRoom.setRoomProperties(iRoomId, _RoomProperties);
			}
			
			void setPivot()
			{
				if (_pivotSelect == 0)
				{	
					pivotVector = new Vector3(0.5f, 0.5f, 0.5f);
					GetComponent<BoxCollider>().center = pivotVector;
				}
				else if (_pivotSelect == 1)
				{
					pivotVector = new Vector3(0, 0, 0);
					GetComponent<BoxCollider>().center = pivotVector;
					
				}
				
				_centerPosition = transform.TransformPoint(pivotVector);
			}
			
			void updateRoomSettings()
			{ 	
				_RoomProperties.fHfDampening = _absorption;
				_RoomProperties.fAmpDampening = _dampening;
				_RoomProperties.fAmpDampeningCeiling = _reflectionCeiling;
				_RoomProperties.fAmpDampeningFloor = _reflectionFloor;
				_RoomProperties.fAmpDampeningRWall = _reflectionRWall;
				_RoomProperties.fAmpDampeningFWall = _reflectionFWall;
				_RoomProperties.fAmpDampeningLWall = _reflectionLWall;
				_RoomProperties.fAmpDampeningBWall = _reflectionBWall;
				_RoomProperties.fDiffuseZoneSize = _diffuseZone;
				
				TBRoom.setRoomProperties(iRoomId, _RoomProperties);
			}
			
			void OnDestroy()
			{
				TBRoom.removeRoom(iRoomId);
			}
			
			/// <summary>
			/// Get and set the intensity of high frequency reflections, clamped between 0 and 1. A greater value results in more high frequency reflections.
			/// </summary>
			/// <value>The intensity of high frequency reflections</value>
			public float hfReflections
			{
				get
				{
					return _absorption;
				}
				set
				{
					_absorption = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Get and set the amplitude of reflections. A greater value results in more reflections.
			/// </summary>
			/// <value>The amplitude of early reflections.</value>
			public float erLevel
			{
				get
				{
					return _dampening;
				}
				set
				{
					_dampening = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Gets or sets the amplitude of reflections off the right wall
			/// </summary>
			/// <value>The amplitude of reflections off the right wall</value>
			public float reflectionRWall
			{
				get
				{
					return _reflectionRWall;
				}
				set
				{
					_reflectionRWall = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Gets or sets the amplitude of reflections off the front wall
			/// </summary>
			/// <value>The amplitude of reflections off the front wall</value>
			public float reflectionFWall
			{
				get
				{
					return _reflectionFWall;
				}
				set
				{
					_reflectionFWall = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Gets or sets the amplitude of reflections off the left wall
			/// </summary>
			/// <value>The amplitude of reflections off the left wall</value>
			public float reflectionLWall
			{
				get
				{
					return _reflectionLWall;
				}
				set
				{
					_reflectionLWall = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Gets or sets the amplitude of reflections off the back wall
			/// </summary>
			/// <value>The amplitude of reflections off the back wall</value>
			public float reflectionBWall
			{
				get
				{
					return _reflectionBWall;
				}
				set
				{
					_reflectionBWall = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Gets or sets the amplitude of reflections off the floor
			/// </summary>
			/// <value>The amplitude of reflections off the floor</value>
			public float reflectionFloor
			{
				get
				{
					return _reflectionFloor;
				}
				set
				{
					_reflectionFloor = value;
					updateRoomSettings();
				}
			}
			
			/// <summary>
			/// Gets or sets the amplitude of reflections off the ceiling
			/// </summary>
			/// <value>The amplitude of reflections off the ceiling</value>
			public float reflectionCeiling
			{
				get
				{
					return _reflectionCeiling;
				}
				set
				{
					_reflectionCeiling = value;
					updateRoomSettings();
				}
			}

			/// <summary>
			/// Gets or sets the size of the diffuse zone (in game units). The diffuse zone is the area around which all reflections gradually die out.
			/// </summary>
			/// <value>Sets the size of the diffuse zone (in game units)</value>
			public float diffuseZone
			{
				get
				{
					return _diffuseZone;
				}
				set
				{
					_diffuseZone = value;
					calculateDiffuseScale();
				}
			}
			
			/// <summary>
			/// Show the room guides guides 
			/// </summary>
			/// <value>Shows the room guides if <c>true</c></value>
			public bool showGuides
			{
				get
				{
					return _showGuides;
				}
				set
				{
					_showGuides = value;
				}
			}
			
			/// <summary>
			/// Gets or sets the pivot point. (0 = corner, 1 = centre)
			/// </summary>
			/// <value>The pivot point. (0 = corner, 1 = centre)</value>
			public int pivotPoint
			{
				get
				{
					return _pivotSelect;
				}
				set
				{
					_pivotSelect = value;
					setPivot();
				}
			}
			
			/// <summary>
			/// Gets or sets room presets.
			/// </summary>
			/// <value>The room preset.</value>
			public TBRoomPresets roomPreset
			{
				get
				{
					return _Presets;
				}
				set
				{	
					_Presets = value;
					TBRoomSettings _RoomSettings;
					switch(_Presets)
					{
					case TBRoomPresets.Default:
						_RoomSettings = PresetSettings.Default();
						_reflectionCeiling = _RoomSettings.fAmpDampeningCeiling;
						_reflectionFloor = _RoomSettings.fAmpDampeningFloor;
						_reflectionRWall = _RoomSettings.fAmpDampeningRWall;
						_reflectionFWall = _RoomSettings.fAmpDampeningFWall;
						_reflectionLWall = _RoomSettings.fAmpDampeningLWall;
						_reflectionBWall = _RoomSettings.fAmpDampeningBWall;
						break;
					case TBRoomPresets.ConcreteRoom:
						_RoomSettings = PresetSettings.ConcreteRoom();
						_reflectionCeiling = _RoomSettings.fAmpDampeningCeiling;
						_reflectionFloor = _RoomSettings.fAmpDampeningFloor;
						_reflectionRWall = _RoomSettings.fAmpDampeningRWall;
						_reflectionFWall = _RoomSettings.fAmpDampeningFWall;
						_reflectionLWall = _RoomSettings.fAmpDampeningLWall;
						_reflectionBWall = _RoomSettings.fAmpDampeningBWall;
						break;
					case TBRoomPresets.Glass:
						_RoomSettings = PresetSettings.Glass();
						_reflectionCeiling = _RoomSettings.fAmpDampeningCeiling;
						_reflectionFloor = _RoomSettings.fAmpDampeningFloor;
						_reflectionRWall = _RoomSettings.fAmpDampeningRWall;
						_reflectionFWall = _RoomSettings.fAmpDampeningFWall;
						_reflectionLWall = _RoomSettings.fAmpDampeningLWall;
						_reflectionBWall = _RoomSettings.fAmpDampeningBWall;
						break;
					case TBRoomPresets.Outdoor:
						_RoomSettings = PresetSettings.Outdoor();
						_reflectionCeiling = _RoomSettings.fAmpDampeningCeiling;
						_reflectionFloor = _RoomSettings.fAmpDampeningFloor;
						_reflectionRWall = _RoomSettings.fAmpDampeningRWall;
						_reflectionFWall = _RoomSettings.fAmpDampeningFWall;
						_reflectionLWall = _RoomSettings.fAmpDampeningLWall;
						_reflectionBWall = _RoomSettings.fAmpDampeningBWall;
						break;
					case TBRoomPresets.RecordingStudio:
						_RoomSettings = PresetSettings.RecordingStudio();
						_reflectionCeiling = _RoomSettings.fAmpDampeningCeiling;
						_reflectionFloor = _RoomSettings.fAmpDampeningFloor;
						_reflectionRWall = _RoomSettings.fAmpDampeningRWall;
						_reflectionFWall = _RoomSettings.fAmpDampeningFWall;
						_reflectionLWall = _RoomSettings.fAmpDampeningLWall;
						_reflectionBWall = _RoomSettings.fAmpDampeningBWall;
						break;
					case TBRoomPresets.LivingRoom:
						_RoomSettings = PresetSettings.LivingRoom();
						_reflectionCeiling = _RoomSettings.fAmpDampeningCeiling;
						_reflectionFloor = _RoomSettings.fAmpDampeningFloor;
						_reflectionRWall = _RoomSettings.fAmpDampeningRWall;
						_reflectionFWall = _RoomSettings.fAmpDampeningFWall;
						_reflectionLWall = _RoomSettings.fAmpDampeningLWall;
						_reflectionBWall = _RoomSettings.fAmpDampeningBWall;
						break;
					}
					
					updateRoomSettings();
				}
			}
		}
	}
}

