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
	}

    //what is the message about


    public class Subject
    {

        //NetworkManager
        public const ushort NewLevelManager      = 1;
		public const ushort ServerSentNetID      = 2;

        //Player Specific
		public const ushort HasJoined            = 3;
		public const ushort SpawnPlayer          = 4;
		public const ushort PlayerUpdate         = 5;

        //TriggerSystem
        public const ushort TriggerActivate      = 6;
        public const ushort TriggerDeactivate    = 7;
        public const ushort TriggerState         = 8;

        public const ushort ServerSentTriggerIDs = 9;
        public const ushort RequestTriggerIDs    = 10;

        public const ushort VoiceChat            = 11;


    }
}
