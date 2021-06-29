using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogINNode : MonoBehaviour
{
    [SerializeField]
    private GameObject text;
    [SerializeField]
    private GameObject color;
    [SerializeField]
    private GameObject picture;
    [SerializeField]
    private GameObject back;
    [SerializeField]
    private Button button;

    [HideInInspector]
    public int hash = -1;

    bool active = false;
    private string Buffer = "";
    private Animator Animator {
        get {
            animator ??= this.gameObject.GetComponent<Animator>();
            return animator;
        }
    }
    private bool AnimationFinish {
        get {
            AnimatorStateInfo animatorInfo;
            animatorInfo = Animator.GetCurrentAnimatorStateInfo(0);  //要在update获取
            if (animatorInfo.normalizedTime > 1.0f) {
                return true;
            }
            return false;
        }
    }

    private Animator animator;
    public string Text {
        get {
            return text.GetComponent<TextMeshProUGUI>().text;
        }
        set {
            text.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
    public Color Color {
        get {
            return color.GetComponent<Image>().color;
        }
        set {
            color.GetComponent<Image>().color = value;
        }
    }
    public bool Active {
        get {
            return this.active;
        }
        set {
            this.text.SetActive(value);
            this.color.SetActive(value);
            this.picture.SetActive(value);
            this.back.SetActive(value);
            this.active = value;
        }
    }

    public void ContextSwitch() {
        Text = Buffer;
    }
    private void Setfalse() {
        this.Active = false;
    }
    private void Settrue() {
        this.Active = true;
    }
    public void Selected(bool select) {
        if (select) {
            this.Enable();
        }
        else {
            this.Disbale();
        }
    }

    public void SetNode(bool Show, string Str) {
        if (Show) {
            if (this.Active) {
                this.SwitchText(Str);                
            }
            else {
                this.Show(Str);
            }
        }
        else {
            if (this.Active) {
                this.DisShow();
            }
        }
    }
    private void Enable() {
        Animator.SetTrigger("Enable");
    }
    private void Disbale() {
        Animator.SetTrigger("Disable");
    }
    private void Show(string Str) {
        this.Text = Str;
        Animator.SetTrigger("Show");
    }
    private void SwitchText(string Str) {
        if (Str != Text) {
            Buffer = Str;
            Animator.SetTrigger("SwitchText");
        }
    }
    private void DisShow() {        
        Animator.SetTrigger("DisShow");
    }
    public void DisableButton() {
        button.interactable = false;
    }
    public void EnableButton() {
        button.interactable = true;
    }


    
}


