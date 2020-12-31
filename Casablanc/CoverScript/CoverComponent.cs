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
    private void OnBecameVisible() {
        CoverManager.Enter(this.CoverGroup);
        EventRegister.CoverEvent.Call();
    }
    private void OnBecameInvisible() {
        CoverManager.Quit(this.CoverGroup);
        EventRegister.CoverEvent.Call();
    }
}
