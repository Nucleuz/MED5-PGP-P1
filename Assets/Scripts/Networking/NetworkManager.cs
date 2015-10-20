using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DarkRift;

public class NetworkManager : MonoBehaviour{

    public static bool isServer = false;
    public static Text debugText;

    public virtual void OnLevelLoaded(int levelIndex){

    }

    public virtual void OnLevelCompleted(){

    }


    //for debugging 
    //@TODO - replace with console
    public static void Write(string mess)
    {
        if (debugText != null)
            debugText.text += mess + "\n";
    }




}
