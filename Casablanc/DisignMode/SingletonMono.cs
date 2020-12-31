using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T _instance = null;
    protected static object obj = new object();
    private static T Instance;


    public static void Singleton_Mono_initialize() {
        lock (obj) { 
            if (null == _instance) {
                GameObject go = GameObject.Find("(Singleton)" + typeof(T).ToString());
                if (null == go) {
                    go = new GameObject { name = "(Singleton) " + typeof(T).ToString() };
                    DontDestroyOnLoad(go);
                }
                _instance = go.GetComponent<T>();
                if (null == _instance) {
                    _instance = go.AddComponent<T>();
                }
            }
            Instance = _instance;


            RunTimeInit.EnableMap.Add(Instance.GetType().BaseType.ToString(), true);
        }
    }  
}  
public class Mono : SingletonMono<Mono>
{
    public static GameObject go;
    public static void Mono_initialize() {
        lock (obj) {
            go = GameObject.Find("MonoBase");
            if (!go) {
                go = new GameObject { name = "MonoBase" };
                DontDestroyOnLoad(go);
            }
        }
    }
    public static void AddComponent<T>() where T:MonoBehaviour {
        go.AddComponent<T>();
    }
}

public interface MonoItem
{
    int NumHash { get; set; }
    void awake();
    void start();
    void update();
    void fixedupdate();
}
public abstract class MonoStatic : MonoItem
{
    public int NumHash { get; set; }
    public MonoStatic() {
        this.NumHash = -1;
        HashMono.MonoAdd(this);
    }
    public virtual void awake() { }
    public virtual void start() { }
    public virtual void update() { }
    public virtual void fixedupdate() { }
}
public class MonoPackage:MonoStatic
{
    public List<MonoItem> monoItems = new List<MonoItem>();
    public override void awake() {
        for (int i = 0; i < monoItems.Count; i++) {
            monoItems[i].awake();
        }
    }
    public override void start() {
        for (int i = 0; i < monoItems.Count; i++) {
            monoItems[i].start();
        }
    }
    public override void update() {
        for (int i = 0; i < monoItems.Count; i++) {
            monoItems[i].update();
        }
    }
    public override void fixedupdate() {
        for(int i = 0; i < monoItems.Count; i++) {
            monoItems[i].fixedupdate();
        }
    }
}

public class HashMono:SingletonMono<HashMono>
{
    private static HashGenerator hashGenerator = new HashGenerator();
    public static Dictionary<int,MonoItem> monoItems = new Dictionary<int, MonoItem>();
    private static int tmp;
    private void Awake() {
        foreach (KeyValuePair<int, MonoItem> mono in monoItems) {
            mono.Value.awake();
        }
    }
    private void Start() {
        foreach (KeyValuePair<int, MonoItem> mono in monoItems) {
            mono.Value.start();
        }
    }
    private void Update() {
        foreach (KeyValuePair<int,MonoItem> mono in monoItems) {
            mono.Value.update();
        }
    }
    private void FixedUpdate() {
        foreach (KeyValuePair<int, MonoItem> mono in monoItems) {
            mono.Value.fixedupdate();
        }
    }

    public static void MonoAdd(MonoItem monoItem) {
        lock (obj) {
            if (monoItem.NumHash == -1) {
                tmp = hashGenerator.GetHash();
                monoItems.Add(tmp, monoItem);
                monoItem.NumHash = tmp;
            }
            else {
                monoItems.Add(monoItem.NumHash, monoItem);
            }

        }
    }
    public static void MonoRemove(MonoItem monoItem) {
        if(monoItems.TryGetValue(monoItem.NumHash,out MonoItem monoItem1)){
            hashGenerator.DisHash(monoItem.NumHash);
            monoItem1 = null;
        }
    }

}










