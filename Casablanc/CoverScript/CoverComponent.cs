using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverComponent : MonoBehaviour
{
    public string Groupname = "建筑1";

    [HideInInspector]
    public CoverGroup CoverGroup;

    private void Awake() {
        CoverManager.EnterGroup(this.gameObject);
    }
   
}
