/*
By KasperHdL
used to categories network messages

//@NOTE: cannot use enums because that would require non stop casting to byte and ushort
*/

public class Network{
	
	//used as the receiver 
	//TODO should then be called receiver..
	public class Tag{
		public const byte Manager 	= 1;
		public const byte Player 	= 2;
        public const byte Trigger   = 3;
        public const byte Mirror    = 4;
	}

    //what is the message about


    public class Subject
    {

        //NetworkManager
        public const ushort RequestServerLevel   = 0;
        public const ushort NewLevelManager      = 1;
		public const ushort ServerSentNetID      = 2;

        //Player Specific
		public const ushort HasJoined            = 3;
		public const ushort SpawnPlayer          = 4;
		public const ushort PlayerPositionUpdate = 5;
        public const ushort PlayerRotationUpdate = 6;
        public const ushort PlayerCartUpdate     = 14;

        //TriggerSystem
        public const ushort TriggerActivate      = 7;
        public const ushort TriggerDeactivate    = 8;
        public const ushort TriggerState         = 9;
        public const ushort PlayerSentTriggerState = 19;

        public const ushort ServerSentTriggerIDs = 10;
        public const ushort ServerSentTriggerStates = 18;
        public const ushort RequestTriggerIDs    = 11;

        public const ushort VoiceChat            = 12;

        public const ushort PlayerFocus          = 13;

        public const ushort SequenceFailed       = 15;

        public const ushort MirrorStarted        = 16;
        public const ushort MirrorEnded          = 17;

    }
}
