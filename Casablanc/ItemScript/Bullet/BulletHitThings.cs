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
            if (collision.collider.gameObject.TryGetComponent<ItemOnTheGround>(out ItemOnTheGround itemOnTheGround)) {
                if (itemOnTheGround.itemOntheGround != Gun) {
                    Time--;
                    ((Item_Detail)itemOnTheGround.itemOntheGround).Info_Handler.BeDmged(((Gun)Gun).GetGunState().Firing_Dmg);
                }
            }
            else if (collision.collider.gameObject.TryGetComponent<Player_Instance>(out Player_Instance player_Instance)) {
                ((ValuePlayer)player_Instance.Instance).DecHP(((Gun)Gun).GetGunState().Firing_Dmg);
                Time--;
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
        int Mark = PoolManager.BulletHolePool.__UsePoolByID(/*this.ID*/1);
        PoolManager.BulletHolePool._GetGameObjectRef(1, Mark, out GameObject gameObject);
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
            PoolManager.BulletPool._GetGameObjectRef(ID, Mark).GetComponent<Rigidbody>().velocity = new Vector3();
            PoolManager.BulletPool._GetGameObjectRef(ID, Mark).GetComponent<TrailRenderer>().Clear();
            PoolManager.BulletPool.__DisablePoolByMap(ID, Mark);
            Recycled = true;
        }
    }
}
