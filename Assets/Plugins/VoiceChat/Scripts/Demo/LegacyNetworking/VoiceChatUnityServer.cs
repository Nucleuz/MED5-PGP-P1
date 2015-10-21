using UnityEngine;
using System.Collections;

namespace VoiceChat.Demo.LegacyNetworking
{
    public class VoiceChatUnityServer : MonoBehaviour
    {
        public int Port = 15000;
        public int MaxConnections = 8;

        void Start()
        {
			UnityEngine.Network.InitializeServer(MaxConnections, Port, false);
            MonoBehaviour.Destroy(GetComponent<VoiceChatUnityClient>());
        }

		void OnPlayerDisconnected(UnityEngine.NetworkPlayer player)
        {
            UnityEngine.Network.RemoveRPCs(player);
			UnityEngine.Network.DestroyPlayerObjects(player);
        }
    } 
}
