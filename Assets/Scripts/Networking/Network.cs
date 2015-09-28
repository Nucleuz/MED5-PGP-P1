

/*
By KasperHdL
used to categories network messages

note:
cannot use enums because that would require non stop casting to byte
*/

public class Network{
	
	public class Tag{
		public const byte Manager 	= 1;
		public const byte Player 	= 2;
	}

	public class Subject{
		public const ushort HasJoined 			= 1;
		public const ushort SpawnPlayer 		= 2;
		public const ushort ServerSentSpawnPos 	= 3;
		public const ushort PlayerUpdate 		= 4;
	}



}