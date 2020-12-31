using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunFireSmoke : MonoBehaviour
{
    private Timer timer = new Timer(5);
    private Timer Timer = new Timer(0.5f);
    public int ID = -1;
    public int Mark = -1;
    private void OnEnable() {
        StartCoroutine(Timer.TimingOnce(this.GetComponent<VisualEffect>().Stop));
        StartCoroutine(timer.TimingOnce(Recycle));
    }

    void Recycle() {
        PoolManager.BulletSmokePool.__DisablePoolByMap(ID, Mark);
    }
}
