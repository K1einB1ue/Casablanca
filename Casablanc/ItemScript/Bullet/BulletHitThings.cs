using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitThings : MonoBehaviour
{
    private Timer timer = new Timer(2);
    public int Time = 1;
    public int ID = -1;
    public int Mark = -1;
    public Item Gun;
    volatile bool Recycled = true;

    private void OnCollisionEnter(Collision collision) {
        if (Time != 0) {
            if (collision.collider.gameObject.TryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
                if (objectOnTheGround.Object != Gun) {
                    Time--;
                    objectOnTheGround.Object_Values_Handler.BeDmged(((Gun)Gun).GunState.Firing_Dmg);
                }
            }
            else {
                Time--;
            }
            if (Time == 0) {
                this.Spark(collision);
                this.Recycle(Recycled);
            }
        }
    }
    private void OnEnable() {
        Recycled = false;
        StartCoroutine(timer.TimingOnce(Recycle,Recycled));
    }

    private void Spark(Collision collision) {
        int Mark = StaticPath.BulletHolePool_Ram.__UsePoolByID(/*this.ID*/1);
        StaticPath.BulletHolePool_Ram._GetGameObjectRef(1, Mark, out GameObject gameObject);
        if (gameObject.TryGetComponent<BulletHitSpark>(out BulletHitSpark bulletHitSpark)) {
            bulletHitSpark.ID = 1;
            bulletHitSpark.Mark = Mark;
        }
        gameObject.transform.Find("Spark").rotation= Quaternion.Euler(collision.contacts[0].normal * 90);
        //gameObject.transform.rotation = Quaternion.Euler(collision.contacts[0].normal*90);
        gameObject.transform.position = collision.contacts[0].point + collision.contacts[0].normal.normalized * 0.02f;
        gameObject.SetActive(true);
    }

    private void Recycle(bool flag) {
        if (!flag) {
            StaticPath.BulletPool_Ram._GetGameObjectRef(ID, Mark).GetComponent<Rigidbody>().velocity = new Vector3();
            StaticPath.BulletPool_Ram._GetGameObjectRef(ID, Mark).GetComponent<TrailRenderer>().Clear();
            StaticPath.BulletPool_Ram.__DisablePoolByMap(ID, Mark);
            Recycled = true;
        }
    }
}
