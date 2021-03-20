using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "天气接口", menuName = "固化接口/天气接口")]
public class WeatherInfoChannel : InfoChannel
{


    public float RainNum {
        get => rainNum;
        set {
            Change = true;
            rainNum = value;
        }
    }
    [ChannelMessage, Range(0.0f, 1.0f)]
    public float sunNum = 0.0f;
    [ChannelMessage, Range(0.0f, 1.0f)]
    public float rainNum = 0.0f;


}



