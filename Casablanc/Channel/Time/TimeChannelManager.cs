using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChannelManager : ChannelManagerBase<TimeChannelManager,TimeInfoChannel>
{
    public TimeChannelManager() : base(StaticPath.TimeChannel) { }


    private int Cnt = 0;
    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (++Cnt == 50) {
            this.InfoChannel.Second++;
            Cnt = 0;
        }
    }
}
