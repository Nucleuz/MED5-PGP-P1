using UnityEngine;
using System.Collections;

public class SoundTriggerZone : MonoBehaviour {

    public SoundCrystal soundCrystal;

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            soundCrystal.PlayerHitTriggerZone();
            Destroy(gameObject);
        }

    }

}
