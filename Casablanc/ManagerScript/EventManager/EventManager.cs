using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : SingletonMono<EventManager>
{
    public partial class CharacterManager
    {
        public static OneEvent OnMainCharacterChange = new OneEvent();
    }

    public partial class UIManager
    {
        public static OneEvent OnGUIShouldChange = new OneEvent();
    }

    

}
public class OneEvent : UnityEvent
{
    private Dictionary<string, bool> UseTable = new Dictionary<string, bool>();
    public void AddListenerOnce(UnityAction unityAction) {
        if (UseTable.TryGetValue(unityAction.Method.Name, out bool var)) {
            if (!var) {
                this.AddListener(unityAction);
            }
        }
        else {
            this.AddListener(unityAction);
        }
    }
}
