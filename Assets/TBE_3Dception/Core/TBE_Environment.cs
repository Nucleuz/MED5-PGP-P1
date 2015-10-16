// Copyright (c) 2015 Two Big Ears Ltd.
// All Rights Reserved
// TwoBigEars.com

using UnityEngine;
using System.Collections;

namespace TBE
{
	namespace Wwise
	{	
		/// <summary>
		/// Set global environment properties: world scale and speed of sound
		/// </summary>
		public class TBE_Environment : MonoBehaviour {
			
			[HideInInspector]
			[SerializeField]
			private float _worldScale = 1.0f;
			
			[HideInInspector]
			[SerializeField]
			private float _speedOfSound = 340.29f;

			void Awake()
			{	
				if (TBEngine.getLicenseStatus () == TBLicenceCheck.TBE_E_LICENSE_TIMEOUT) 
				{
					Debug.LogError("3Dception Wwise Evaluation License Expired. Please contact support@twobigears.com");
				}

				TBEngine.setWorldScale(_worldScale);
				TBEngine.setSpeedOfSound(_speedOfSound);
			}

			/// <summary>
			/// Gets or set the scale of the world: 1 unit in the game world translated to metres.
			/// </summary>
			/// <value>The world scale.</value>
			public float worldScale
			{
				set
				{
					_worldScale = value;
					TBEngine.setWorldScale(_worldScale);
				}
				
				get
				{
					return _worldScale;
				}
			}

			/// <summary>
			/// Gets or sets the speed of sound in metres per second. Affects doppler and room modelling.
			/// </summary>
			/// <value>The speed of sound.</value>
			public float speedOfSound
			{
				set
				{
					_speedOfSound = value;
					TBEngine.setSpeedOfSound(_speedOfSound);
				}
				
				get
				{
					return _speedOfSound;
				}
			}
		}
	}
}