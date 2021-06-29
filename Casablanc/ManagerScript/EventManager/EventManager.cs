using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : SingletonMono<EventManager>
{
    public partial class CharacterManager
    {
        /// <summary>
        /// 在主角色改变时触发
        /// </summary>
        public static OneEvent OnMainCharacterChange = new OneEvent();
    }
    public partial class StoryChannelManager
    {
        /// <summary>
        /// 在故事更新时触发,或者说它潜在调用了故事更新.
        /// </summary>
        public static OneEvent OnStoryRefresh = new OneEvent();
        /// <summary>
        /// 在对话选择时返回选项string
        /// </summary>
        public static OneEvent<string> OnSelectDialog = new OneEvent<string>();
        /// <summary>
        /// 在主角色对话机改变时触发
        /// </summary>
        public static UnityEvent OnMainCharacterDialogChange = new UnityEvent();
    }
    public partial class UIManager
    {
        public static OneEvent OnGUIShouldChange = new OneEvent();
    }


}









































public class OneEvent : UnityEvent
{
    private HashSet<UnityAction> EnableTable = new HashSet<UnityAction>();
    public void AddListenerOnce(UnityAction unityAction) {
        if (EnableTable.Add(unityAction)) {
            this.AddListener(unityAction);
        }
    }
}

public class OneEvent<T> : UnityEvent<T>
{
    private HashSet<UnityAction<T>> EnableTable = new HashSet<UnityAction<T>>();
    public void AddListenerOnce(UnityAction<T> unityAction) {
        if (EnableTable.Add(unityAction)) {
            this.AddListener(unityAction);
        }
    }
}