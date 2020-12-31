using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverTriggerComponent : MonoBehaviour
{
    public string groupname;
    [HideInInspector]
    public int Count = 0;
    bool disable = false;
    public void Disable(){
        Count++;
        if (Count > 0 && !disable) {
            CoverManager.Disable(this.groupname);
            disable = true;
        }        
    }


    public void Enable() {
        Count--;
        if (Count <= 0) {
            CoverManager.Enable(this.groupname);
            disable = false;
        }
    }
    
}
