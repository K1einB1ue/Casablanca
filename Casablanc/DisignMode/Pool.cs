using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "物品池", menuName = "静态加载/物品池")]

public class Pool : ScriptableObject
{
    [SerializeField]
    private List<ItemStore> __ObjectPoolOrigin = new List<ItemStore>();
    [SerializeField]
    private int __PoolSize = 0;


    public GameObject[,] __BulletPool;
    public volatile bool[,] __EnableTable;
    private Dictionary<int, int> IDmapping = new Dictionary<int, int>();
    public LinkedList<GameObject> __BulletPoolExpansion;



    public void __SetUP__Pool() {
        __ObjectPoolOrigin.Sort((x, y) => x.ItemStaticProperties.ItemID.CompareTo(y.ItemStaticProperties.ItemID));
        __BulletPool = new GameObject[__ObjectPoolOrigin.Count, __PoolSize];
        __EnableTable = new bool[__ObjectPoolOrigin.Count, __PoolSize];
        __Mapping();
        __InstanceAll();
    }
    private void __Mapping() {
        if (IDmapping.Count == 0) {
            for (int i = 0; i < __ObjectPoolOrigin.Count; i++) {
                IDmapping[__ObjectPoolOrigin[i].ItemStaticProperties.ItemID] = i;
            }
        }
    }
    private void __InstanceAll() {
        if (!__BulletPool[__ObjectPoolOrigin.Count - 1, __PoolSize - 1]) {
            for (int i = 0; i < __ObjectPoolOrigin.Count; i++) {
                for (int j = 0; j < __PoolSize; j++) {
                    GameObject temp = GameObject.Instantiate(__ObjectPoolOrigin[i].ItemOrigin);
                    temp.SetActive(false);
                    __BulletPool[i, j] = temp;
                }
            }
        }
    }
    public void __TryReInit() {
        __Mapping();
        __InstanceAll();
    }
    public int __UsePoolByID(int Bullet_ID) { 
        for (int Mark = 0; Mark < __PoolSize; Mark++) {
            if (!__EnableTable[IDmapping[Bullet_ID], Mark]) {
                __EnableTable[IDmapping[Bullet_ID], Mark] = true;
                return Mark;
            }
        }
        return -1;
    }
    public GameObject _GetGameObjectRef(int Bullet_ID,int Mark) {
        return __BulletPool[IDmapping[Bullet_ID], Mark];
    }
    public void _GetGameObjectRef(int Bullet_ID,int Mark,out GameObject gameObject) {
        try {
            object.Equals(__BulletPool[IDmapping[Bullet_ID], Mark], null);
        }
        catch (IndexOutOfRangeException) {
            Debug.LogError("物品池过小,无法提供充足实例,请在物品池文件夹下对应Size调大");
        }
        finally {
            gameObject = __BulletPool[IDmapping[Bullet_ID], Mark];
        }
    }
    public void __DisablePoolByMap(int Bullet_ID,int Mark) {
        __BulletPool[IDmapping[Bullet_ID], Mark].SetActive(false);
        __EnableTable[IDmapping[Bullet_ID], Mark] = false;
    }
}
