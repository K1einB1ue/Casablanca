using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class StoryChannelManager : ChannelManagerBase<StoryChannelManager, StoryInfoChannel>
{
    private static HashSet<DialogNode> Before = new HashSet<DialogNode>();
    private static HashSet<DialogNode> Current = new HashSet<DialogNode>();
    private static Dialog Dialog {
        get {
            return CharacterManager.Main.Dialog;
        }
    }
    private static bool IsNullBefore = true;
    private static bool DialogChange {
        get {
            if (Dialog != null) {
                if (IsNullBefore) {
                    IsNullBefore = false;
                    foreach(var node in Dialog.DialogGroupLize().dialogNodes) {
                        Current.Add(node);
                    }
                    return true;
                }
                else {
                    Current.Clear();
                    bool Change = false;
                    foreach (var node in Dialog.DialogGroupLize().dialogNodes) {
                        Change = Change || (!Before.Contains(node));
                        Current.Add(node);
                    }
                    Before.ExceptWith(Current);
                    if (Before.Count > 0) {
                        Change = true;
                    }
                    Before.Clear();
                    Before = Current;
                    Current = new HashSet<DialogNode>();
                    return Change;
                }
            }
            else {
                if (!IsNullBefore) {
                    Before.Clear();
                    Current.Clear();
                    IsNullBefore = true;
                    return true;
                }
                return false;               
            }
        }
    }
    public static void Refresh() {
        Instance.InfoChannel.Main.Update();
    }
    public StoryChannelManager() : base(StaticPath.StoryInfoChannel) { }

    protected override void OnChange() {
        base.OnChange();      
        EventManager.StoryChannelManager.OnStoryRefresh?.Invoke();
        
    }
    private void Update() {
        if (DialogChange) {
            EventManager.StoryChannelManager.OnMainCharacterDialogChange?.Invoke();
        }
    }

    public static void UpdateDialog(DialogMachineGroup Group) {
        if (Instance) {
            Instance.InfoChannel.UpdateDialog(Group);
        }
    }

    static StoryChannelManager() {
        EventManager.StoryChannelManager.OnStoryRefresh.AddListenerOnce(CharacterManager.Refresh);
        EventManager.StoryChannelManager.OnStoryRefresh.AddListenerOnce(StoryChannelManager.Refresh);
    }
}
