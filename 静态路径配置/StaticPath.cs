using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPath
{


    public static ItemLoad ItemLoad                 { get { return Resources.Load<ItemLoad>         ("__All_Item/物品静态加载"                ); } }
   
    public static UIPool UIPool                     { get { return Resources.Load<UIPool>           ("____初始化物品池/UIPool"                ); } }
    public static Pool BulletPool_Ram =                                     Resources.Load<Pool>("____初始化物品池/BulletPool");
    public static Pool BulletHolePool_Ram =                                 Resources.Load<Pool>("____初始化物品池/BulletHolePool");
    public static Pool BulletSmokePool_Ram =                                Resources.Load<Pool>("____初始化物品池/BulletSmokePool");

    public static StoryInfoDialogInfo StoryInfoDialogInfo =                 Resources.Load<StoryInfoDialogInfo>("静态路径加载区/剧情节点界面存储");
    public static GameObject CamGroup =                                     Resources.Load<GameObject>("____初始化预制体/摄像机组");

    //public static GameObject OriginGameObject { get { return Resources.Load<GameObject>("__PoolOrigin/Item预制体/Item_Origin"); } }


}


public static class PoolRegister {

    public static void UIPoolRegisterList() {
        StaticPath.UIPool.__SetUP__Pool();
    }
}
