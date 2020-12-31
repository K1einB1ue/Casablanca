using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    public static Pool BulletPool { get { return StaticPath.BulletPool_Ram; } }
    public static Pool BulletHolePool { get { return StaticPath.BulletHolePool_Ram; } }
    public static Pool BulletSmokePool { get { return StaticPath.BulletSmokePool_Ram; } }
    
    private void Awake() {
        BulletPool.__SetUP__Pool();
        BulletHolePool.__SetUP__Pool();
        BulletSmokePool.__SetUP__Pool();
    }
    /*
    private void Update() {
        BulletPool.__TryReInit();
        BulletHolePool.__TryReInit();
        BulletSmokePool.__TryReInit();
    }
    */

}
