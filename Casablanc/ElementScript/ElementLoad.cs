using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementLoad : ScriptableObject
{
    [SerializeField]
    public List<ElementStore> elementlist;

    

    [HideInInspector]
    //public Dictionary<ElementType, ElementStore> ElementStatics;
    public Dictionary<string, ElementStore> ElementStatics;

    private void OnEnable() {
        //ElementStatics = new Dictionary<ElementType, ElementStore>();
        //foreach (var element in elementlist) {
        //    if (element != null) {
        //        if (ElementStatics.TryGetValue(element.ElementType, out var itemStore)) {
        //            Debug.LogError("物品静态加载有重复物品!请检查!");
        //        }
        //        else {
        //            ElementStatics[element.ElementType] = element;
        //        }
        //    }
        //}


        ElementStatics = new Dictionary<string, ElementStore>();
        foreach (var element in elementlist) {
            if (element != null) {
                if (ElementStatics.TryGetValue(element.name, out var itemStore)) {
                    Debug.LogError("物品静态加载有重复物品!请检查!");
                }
                else {
                    ElementStatics[element.name] = element;
                }
            }
        }

    }

    //public ElementStore this[ElementType Type]
    //{
    //    get {
    //        if (ElementStatics.TryGetValue(Type, out var elementStore)) {
    //            return elementStore;
    //        }
    //        else {
    //            Debug.LogError("错误的物品请求");
    //            return null;
    //        }
    //    }
    //}


    public ElementStore this[string type]
    {
        get {
            if (ElementStatics.TryGetValue(type, out var elementStore)) {
                return elementStore;
            }
            else {
                Debug.LogError("错误的物品请求");
                return null;
            }
        }
    }
}
