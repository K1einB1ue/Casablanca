using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitSpark : MonoBehaviour
{
    private Timer timer = new Timer(5);
    public int ID = -1;
    public int Mark = -1;

    private void OnEnable() {
        StartCoroutine(timer.TimingOnce(Recycle));
    }

    void Recycle() {
        StaticPath.BulletHolePool_Ram.__DisablePoolByMap(ID, Mark);
    }
}


