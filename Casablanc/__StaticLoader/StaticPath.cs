using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPath
{


    public static ItemLoad ItemLoad                                 = Resources.Load<ItemLoad>("静态路径加载区/物品静态加载");
    public static CharacterLoad CharacterLoad                       = Resources.Load<CharacterLoad>("静态路径加载区/角色静态加载");
    public static ElementLoad ElementLoad                           = Resources.Load<ElementLoad>("静态路径加载区/元素静态加载");


    public static UIPool UIPool                                     = Resources.Load<UIPool>("静态路径加载区/UI池");
    public static Pool BulletPool_Ram                               = Resources.Load<Pool>("静态路径加载区/子弹池");
    public static Pool BulletHolePool_Ram                           = Resources.Load<Pool>("静态路径加载区/弹痕池");
    public static Pool BulletSmokePool_Ram                          = Resources.Load<Pool>("静态路径加载区/烟雾池");
    public static GameObject CamGroup                               = Resources.Load<GameObject>("静态路径加载区/摄像机组");

    public static StoryInfoDialogInfo Story_DialogInfo              = Resources.Load<StoryInfoDialogInfo>("静态路径加载区/剧情节点界面存储");

    public static PlayerInput PlayerInput                           = Resources.Load<PlayerInput>("静态频道加载区/主机输入接口");


    public static WeatherInfoChannel WeatherChannel                 = Resources.Load<WeatherInfoChannel>("静态频道加载区/天气接口");
    public static TimeInfoChannel TimeChannel                       = Resources.Load<TimeInfoChannel>("静态频道加载区/时间接口");
    public static StoryInfoChannel StoryInfoChannel                 = Resources.Load<StoryInfoChannel>("静态频道加载区/剧情接口");

}


