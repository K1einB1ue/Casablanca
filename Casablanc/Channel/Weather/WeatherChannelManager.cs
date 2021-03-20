using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChannelManager : ChannelManagerBase<WeatherChannelManager,WeatherInfoChannel>
{
    public WeatherChannelManager() : base(StaticPath.WeatherChannel) { }
}

