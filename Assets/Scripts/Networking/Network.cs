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
	}

	//what is the message about
	public class Subject{
		public const ushort HasJoined 			= 1;
		public const ushort SpawnPlayer 		= 2;
		public const ushort ServerSentSpawnPos 	= 3;
		public const ushort ServerSentNetID 	= 4;
		public const ushort PlayerUpdate 		= 5;
		public const ushort VoiceChat			= 6;
	}



}