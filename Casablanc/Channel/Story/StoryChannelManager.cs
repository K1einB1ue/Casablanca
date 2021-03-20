using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class StoryChannelManager : ChannelManagerBase<StoryChannelManager, StoryInfoChannel>
{
    public static UnityEvent<string> SelectDialogEvent = new UnityEvent<string>();

    public StoryChannelManager() : base(StaticPath.StoryInfoChannel) { }

    protected override void OnChange() {
        base.OnChange();
        this.InfoChannel.Main.Update();
    }

    public static void UpdateDialog(DialogMachineGroup Group) {
        Instance.InfoChannel.UpdateDialog(Group);
    }

}
