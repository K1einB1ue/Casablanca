using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NormalPlayer_Instance : MonoBehaviour,Player_Instance
{
    public InputBase Input;
    [SerializeField]
    ItemInfoStore ItemInfoStore;
    public Player Instance { get { return this.Player; } set { this.Player = value; } }
    public Player Player;


    private void Start() {       





        //下划线事件为必要事件
        Instance = new NormalPlayer(GetComponent<Rigidbody>());
        Instance.__SetReborn(/*GameObject.FindGameObjectWithTag("RebornPoint").transform.position*/ new Vector3(0, 0, 0));                              //复活点
        Instance.__PlayerStateSetting.Rebornable = true;                                                                       //可复活的
        Instance.__PlayerStateSetting.__SetMainPlayer();                                                                       //玩家操作
        Instance.__ExploreSwitch(true);                                                                                        //可否上下操作
        Instance.__SetBackPack(ItemInfoStore.GetContainer());                                                                  //设置背包
        Instance.__SetVelocity(0.03f,3.0f);                                                                                    //速度(行走,奔跑倍率)
        Instance.__SetHP(100, 100);                                                                                            //生命(当前,上限)
        Instance.__SetVIT(100, 100);                                                                                           //体力(当前,上限)
        Instance.Heldupdate();                                                                                                 //手持物品图像更新
    }
    
    void Update(){
        Instance.__update();
    }
    private void FixedUpdate() {
        Instance.__fixedupdate();
    }
    private void OnDisable() {
        ItemInfoStore.StoreContainer(Instance);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<CoverTriggerComponent>(out CoverTriggerComponent coverTriggerComponent)) {
            coverTriggerComponent.Disable();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.TryGetComponent<CoverTriggerComponent>(out CoverTriggerComponent coverTriggerComponent)) {
            coverTriggerComponent.Enable();
        }
    }


}


