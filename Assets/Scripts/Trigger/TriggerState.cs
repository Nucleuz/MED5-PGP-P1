using System;

[Serializable]
public class TriggerState{

    public ushort id;
    public bool isTriggered;
    public bool isReadyToBeTriggered;
    public bool canReset;

    public TriggerState(ushort id, bool isTriggered, bool isReadyToBeTriggered, bool canReset){
        this.id                   = id;
        this.isTriggered          = isTriggered;
        this.isReadyToBeTriggered = isReadyToBeTriggered;
        this.canReset             = canReset;
    }
    public TriggerState(Trigger trigger){
        this.id                   = trigger.triggerID;
        this.isTriggered          = trigger.isTriggered;
        this.isReadyToBeTriggered = trigger.isReadyToBeTriggered;
        this.canReset             = trigger.canReset;
    }

    public override string ToString(){
        return "TriggerState(id: " + id + ",triggered: " + isTriggered + ",ready: " + isReadyToBeTriggered + ",canReset: " + canReset + ")";
    }
}
