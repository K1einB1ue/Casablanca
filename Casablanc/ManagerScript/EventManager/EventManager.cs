using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : SingletonMono<EventManager>
{
    public partial class CharacterManager
    {
        /// <summary>
        /// ������ɫ�ı�ʱ����
        /// </summary>
        public static OneEvent OnMainCharacterChange = new OneEvent();
    }
    public partial class StoryChannelManager
    {
        /// <summary>
        /// �ڹ��¸���ʱ����,����˵��Ǳ�ڵ����˹��¸���.
        /// </summary>
        public static OneEvent OnStoryRefresh = new OneEvent();
        /// <summary>
        /// �ڶԻ�ѡ��ʱ����ѡ��string
        /// </summary>
        public static OneEvent<string> OnSelectDialog = new OneEvent<string>();
        /// <summary>
        /// ������ɫ�Ի����ı�ʱ����
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