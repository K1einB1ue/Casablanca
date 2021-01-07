using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class StaticRun
{
    public static Dictionary<string, bool> EnableMap = new Dictionary<string, bool>();


    public static bool Enable(string TypeName) {
        if(EnableMap.TryGetValue(TypeName, out bool flag)) {
            if (flag) {
                return true;
            }
            return false;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// 反射初始化,构造函数字典,静态属性映射.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void StaticInit() {
        //物品系统注册表初始化
        ItemRegister.ItemRegisterList();
        //界面系统注册表初始化
        UIRegister.UIRegisterList();
        //UI物品池系统初始化
        PoolRegister.UIPoolRegisterList();
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init() {
        SingletonMono<ScriptobjectManager>  .Singleton_Mono_initialize();   //解决输入端口唯一性_前置于所有具有输入端口的脚本
        SingletonMono<HashMono>             .Singleton_Mono_initialize();   //得到可以删除的Update队列;
        SingletonMono<PlayerManager>        .Singleton_Mono_initialize();   //具有输入接口
        SingletonMono<CameraManager>        .Singleton_Mono_initialize();   //将会有输入接口

        SingletonMono<PoolManager>          .Singleton_Mono_initialize();
        SingletonMono<UIManager>            .Singleton_Mono_initialize();

        SingletonMono<EffectManager>        .Singleton_Mono_initialize();
        SingletonMono<EventManager>         .Singleton_Mono_initialize();
        SingletonMono<CoverManager>         .Singleton_Mono_initialize();
        
        SingletonMono<ThermodynamicsManager>   .Singleton_Mono_initialize();
        //事件系统注册表 需要实例化事件系统
        EventRegister.CoverEvent.Eventlize(CoverManager.Onchange);




        //在需要debug状态时请实例化该脚本
        SingletonMono<SystemStateManager>.Singleton_Mono_initialize();



        //以下为测试中脚本
        SingletonMono<MeshLoad>.Singleton_Mono_initialize();
        SingletonMono<TimeManager>.Singleton_Mono_initialize();







        //以上



        //Mono系统注册表 需要实例化Mono
        Mono.Mono_initialize();
        Mono.AddComponent<Item_Instance>();
        Mono.AddComponent<KeyLoad>();
        Mono.AddComponent<MouseLoad>();


        //动态Mono 需要实例化HashMono 且加载包需要继承自MonoPackage/MonoStatic

        //HashMono.MonoAdd();
        //HashMono.MonoRemove();
    }

}

