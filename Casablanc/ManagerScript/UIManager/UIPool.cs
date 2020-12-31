using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "新界面池", menuName = "界面/界面池")]
public class UIPool : ScriptableObject
{
    [SerializeField]
    private List<UIObject> __ObjectPoolOrigin = new List<UIObject>();

    public Dictionary<int, List<GameObject>> __UIPool=new Dictionary<int, List<GameObject>>();
    public Dictionary<int, List<bool>> __EnableTable = new Dictionary<int, List<bool>>();
    public Dictionary<int, int> IDmapping = new Dictionary<int, int>();
    public Dictionary<int, int> IDBackMap = new Dictionary<int, int>();
    [HideInInspector]
    public int[] __Size;
    [HideInInspector]
    public int __MaxID = -1;
    [HideInInspector]
    public int __Count = -1;


    public void __SetUP__Pool() {
        __ObjectPoolOrigin.Sort((x, y) => x.ID.CompareTo(y.ID));
        __MaxID = __ObjectPoolOrigin[__ObjectPoolOrigin.Count - 1].ID + 1;
        __Size = new int[__MaxID];
        __Count = __ObjectPoolOrigin.Count;
        __Mapping();
        __InstanceAll();
    }


    void __Mapping() {
        for (int i = 0; i < __ObjectPoolOrigin.Count; i++) {
            IDmapping[__ObjectPoolOrigin[i].ID] = i;
            IDBackMap[i] = __ObjectPoolOrigin[i].ID;
        }
    }
    void __InstanceAll() {
        for (int i = 0; i < __ObjectPoolOrigin.Count; i++) {
            __Size[i] = __ObjectPoolOrigin[i].Size;
            for (int j = 0; j < __ObjectPoolOrigin[i].Size; j++) {
                if (__EnableTable.TryGetValue(i,out List<bool> value)){
                    __EnableTable[i].Add(false);
                }
                else {
                    __EnableTable.Add(i, new List<bool>());
                    __EnableTable[i].Add(false);
                }
                try {
                    GameObject temp;
                    temp = GameObject.Instantiate(__ObjectPoolOrigin[i].UI);
                    temp.SetActive(false);
                    if (__UIPool.TryGetValue(i, out List<GameObject> gameobj)) {
                        __UIPool[i].Add(temp);
                    }
                    else {
                        __UIPool.Add(i, new List<GameObject>());
                        __UIPool[i].Add(temp);

                    }
                }
                catch (UnassignedReferenceException) {
                    Debug.LogError("UIPool中UI个体并未设置实体,位置为第" + (i + 1).ToString() + "个");
                }
            }
        }
    }

    public int __UsePoolByID(int UI_ID) {
        for (int Mark = 0; Mark < __EnableTable[IDmapping[UI_ID]].Count; Mark++) {
            if (!__EnableTable[IDmapping[UI_ID]][Mark]) {
                __EnableTable[IDmapping[UI_ID]][Mark] = true;
                return Mark;
            }
        }
        return -1;
    }

    public GameObject _GetGameObjectRef(int UI_ID, int Mark) {
        return __UIPool[IDmapping[UI_ID]][Mark];
    }
    public void _GetGameObjectRef(int UI_ID, int Mark, out GameObject gameObject) {
        gameObject = __UIPool[IDmapping[UI_ID]][Mark];
    }
    public void __DisablePoolByMap(int UI_ID, int Mark) {
        __UIPool[IDmapping[UI_ID]][Mark].SetActive(false);
        __EnableTable[IDmapping[UI_ID]][Mark] = false;
    }
}
