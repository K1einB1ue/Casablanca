using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "物品池", menuName = "静态加载/物品池")]
public class Pool : ScriptableObject
{
    //[SerializeField]
    //private List<ItemStore> __ObjectPoolOrigin = new List<ItemStore>();
    [SerializeField]
    public List<PoolStore> Origins = new List<PoolStore>();
    [SerializeField]
    private int PoolSize = 0;


    public GameObject[,] PoolItems;
    public volatile bool[,] EnableTable;
    private Dictionary<int, int> IDmapping = new Dictionary<int, int>();
    public LinkedList<GameObject> BulletPoolExpansion;



    public void __SetUP__Pool() {
        Origins.Sort((x, y) => x.ID.CompareTo(y.ID));
        PoolItems = new GameObject[Origins.Count, PoolSize];
        EnableTable = new bool[Origins.Count, PoolSize];
        Mapping();
        InstanceAll();
    }
    private void Mapping() {
        if (IDmapping.Count == 0) {
            for (int i = 0; i < Origins.Count; i++) {
                IDmapping[Origins[i].ID] = i;
            }
        }
    }
    private void InstanceAll() {
        if (!PoolItems[Origins.Count - 1, PoolSize - 1]) {
            for (int i = 0; i < Origins.Count; i++) {
                for (int j = 0; j < PoolSize; j++) {
                    GameObject temp = GameObject.Instantiate(Origins[i].Origin);
                    temp.SetActive(false);
                    PoolItems[i, j] = temp;
                }
            }
        }
    }

    public int __UsePoolByID(int Bullet_ID) { 
        for (int Mark = 0; Mark < PoolSize; Mark++) {
            if (!EnableTable[IDmapping[Bullet_ID], Mark]) {
                EnableTable[IDmapping[Bullet_ID], Mark] = true;
                return Mark;
            }
        }
        return -1;
    }
    public GameObject _GetGameObjectRef(int Bullet_ID,int Mark) {
        return PoolItems[IDmapping[Bullet_ID], Mark];
    }
    public void _GetGameObjectRef(int Bullet_ID,int Mark,out GameObject gameObject) {
        try {
            object.Equals(PoolItems[IDmapping[Bullet_ID], Mark], null);
        }
        catch (IndexOutOfRangeException) {
            Debug.LogError("物品池过小,无法提供充足实例,请在物品池文件夹下对应Size调大");
        }
        finally {
            gameObject = PoolItems[IDmapping[Bullet_ID], Mark];
        }
    }
    public void __DisablePoolByMap(int Bullet_ID,int Mark) {
        PoolItems[IDmapping[Bullet_ID], Mark].SetActive(false);
        EnableTable[IDmapping[Bullet_ID], Mark] = false;
    }
}
