using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogUI : MonoBehaviour
{
    public static bool Dialoging = false;
    public float Interval_time = 0.5f;
    public DialogIN DialogIN;
    public DialogOut DialogOut;

    private Animator Animator {
        get {
            animator ??= this.GetComponent<Animator>();
            return animator;
        }
    }
    private Animator animator;

    public static ActionTimer ActionTimer = new ActionTimer();

    private void OnEnable() {
        ActionTimer.Register(DialogModeSwitch);
        StaticPath.PlayerInput.RegisteInput(ActionTimer.Invoke, InputType.O, true);
    }

    private void DialogModeSwitch() {
        if (Dialoging) {
            Animator.SetTrigger("Switch");
        }
        else {
            Animator.SetTrigger("Switch");
        }
    }

}
