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

    [HideInInspector]
    public int hash = -1;


    private Animator Animator;

    private void OnEnable() {
        this.Animator = this.gameObject.GetComponent<Animator>();
    }
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
            return this.gameObject.activeSelf;
        }
        set {
            this.gameObject.SetActive(value);
        }
    }
    public void Setfalse() {
        this.gameObject.SetActive(false);
    }

    public void Selected(bool select) {
        Animator.SetBool("Enable", select);
        Animator.SetTrigger("Invoke");
    }

    
}


