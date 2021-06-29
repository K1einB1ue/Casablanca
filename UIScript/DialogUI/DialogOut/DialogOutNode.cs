using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogOutNode : MonoBehaviour
{
    [SerializeField]
    private float MinHeight = 57;
    [SerializeField]
    private GameObject text;
    [SerializeField]
    private GameObject color;
    [SerializeField]
    private LayoutElement layoutElement;

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
    private LayoutElement LayoutElement {
        get {
            if (!layoutElement) {
                layoutElement = this.GetComponent<LayoutElement>();
            }
            return layoutElement;
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
    private float Height {
        get {
            try {
                return this.text.GetComponent<TextMeshPro>().preferredHeight > this.MinHeight ? this.GetComponent<TextMeshPro>().preferredHeight : this.MinHeight;
            }
            catch {
                Debug.Log("");
            }
            return 0;
        }
    }

    private void Awake() {   
        this.LayoutElement.minHeight = this.Height;
        Rect rect = this.color.GetComponent<RectTransform>().rect;
        this.color.GetComponent<RectTransform>().rect.Set(rect.x, rect.y, rect.width, this.Height);
    }
}
