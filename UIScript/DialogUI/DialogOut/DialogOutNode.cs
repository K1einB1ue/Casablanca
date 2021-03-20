using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogOutNode : MonoBehaviour
{
    [SerializeField]
    private GameObject text;
    [SerializeField]
    private GameObject color;


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
}
