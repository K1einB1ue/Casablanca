using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalAI_Instance : MonoBehaviour, Player_Instance                                                            //Player_Instance 用以调用多态泛型UI.
{
    public Player Instance { get { return this.Player; } set { this.Player = value; } }
    public Player Player;
    private void Start() {                                                                                                     //下划线事件为必要事件
        Instance = new NormalPlayer(GetComponent<Rigidbody>());
        Instance.__SetBackPack((Container)Items.GetItemByItemTypeAndItemIDWithoutItemProperty(ItemType.Container, 0));
        Instance.__PlayerStateSetting.DropItem = true;
        Instance.__PlayerStateSetting.FalseWhenDeath = true;
        Instance.__PlayerStateSetting.Rebornable = false;
        Instance.__SetReborn(this.gameObject.transform.position);
        Instance.__SetVelocity(0.03f, 3.0f);
        Instance.__SetHP(100, 100);
        Instance.__SetVIT(100, 100);

        //((Container)Instance.GetStaticBag()).AddItem(Items.GetItemByItemTypeAndItemID(ItemType.Container, 1));                                        //标准背包   
        //((Container)Instance.GetStaticBag()).AddItem(Items.GetItemByItemTypeAndItemID(ItemType.Container, 1));                                        //弹夹
        //((Container)((Container)Instance.GetStaticBag()).FindItem(ItemType.Container, 1)[0]).AddItem(Items.GetItemByItemTypeAndItemID(ItemType.Container, 1));          //背包套娃
        //((Container)((Container)((Container)Instance.GetStaticBag()).FindItem(ItemType.Container, 1)[0]).FindItem(ItemType.Container, 1)[0]).AddItem(Items.GetItemByItemTypeAndItemID(ItemType.Container, 1));
        //((Container)Instance.GetStaticBag()).AddItem(Items.GetItemByItemTypeAndItemID(ItemType.Container, 1));                                        //AK-47
        //Instance.Gain(Items.GetItemByItemTypeAndItemID(ItemType.Gun, 1));
        Instance.Heldupdate();

    }
    

    void Update() {
        Instance.__update();
    }
    private void FixedUpdate() {
        Instance.__fixedupdate();
    }


}
