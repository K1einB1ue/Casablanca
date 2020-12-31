using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
//using System.Reflection.Emit;

public static class EventRegister
{
    public static EventBody CoverEvent = new EventBody("Cover-Change");
    //public static EventBody SoundEvent = new EventBody("Cover-Change");
    //public static EventBody DoorEvent = new EventBody("Cover-Change");
    //public static EventBody CoverEvent = new EventBody("Cover-Change");





}
public static class UIRegister {
    public static void UIRegisterList() {
        Init_Search();
    }
    private static void Init_Search() {
        Type type = typeof(UIStatic);
        Assembly assembly = Assembly.GetAssembly(type);
        foreach (Type Child in assembly.GetTypes()) {
            object[] vs = Child.GetCustomAttributes(typeof(UIAttribute), true);
            if (vs.Length > 0) {
                foreach (Attribute Att in vs) {
                    UIAttribute uIAttribute = Att as UIAttribute;
                    if (uIAttribute.enable) {
                        UIS.AddGenerators(uIAttribute.UIID, uIAttribute.UI_TYPE, uIAttribute.ParID, Child);             
                    }
                }
            }
        } 
    }
}

public static class ItemRegister
{
    public static void ItemRegisterList() {
        Init_Search();
    }
    private static void Init_Search() {
        Type type = typeof(ItemStatic);
        Assembly assembly = Assembly.GetAssembly(type);
        foreach (Type Child in assembly.GetTypes()) {
            object[] vs = Child.GetCustomAttributes(typeof(ItemAttribute), true);
            if (vs.Length > 0) {
                foreach (Attribute Att in vs) {
                    ItemAttribute itemAttribute = Att as ItemAttribute;
                    if (itemAttribute.enable) {
                        Items.AddGenerators(itemAttribute.ItemType, itemAttribute.ItemID, Child);
                        if (Child.GetInterfaces().Contains(typeof(Container))) {
                            Items.AddIsContainer(itemAttribute.ItemType, itemAttribute.ItemID, Child);
                        }
                    }
                }
            }
        }
        Items.FillIsContainerDictionary();
        return;
    }
}