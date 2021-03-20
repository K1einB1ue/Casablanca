using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Bullet
{
    
    void Shoot(Gun gun);
}



public class BulletStatic : ItemBase, Bullet
{
    public BulletStatic() { }

    public virtual void Shoot(Gun gun) {
        int Mark = StaticPath.BulletPool_Ram.__UsePoolByID(gun.BulletKind);
        if(StaticPath.BulletPool_Ram._GetGameObjectRef(gun.BulletKind, Mark).TryGetComponent<BulletHitThings>(out BulletHitThings bulletHitThings)){
            bulletHitThings.ID = gun.BulletKind;
            bulletHitThings.Mark = Mark; ;
            bulletHitThings.Time = 1;
            bulletHitThings.Gun = (Item)gun;
        }
        this.NormalTransShoot(StaticPath.BulletPool_Ram._GetGameObjectRef(gun.BulletKind, Mark), gun);
    } 

    protected void NormalTransShoot(GameObject gameObject ,Gun gun) {
        gameObject.transform.position = gun.GetOutPointPos();
        gameObject.SetActive(true);
        gameObject.GetComponent<Rigidbody>().AddForce((gun.GetOutPointPos() - gun.GetLinePointPos() - gun.GetRandom() * 0.1f).normalized * gun.GunState.Firing_Force, ForceMode.Force);
    }
}
