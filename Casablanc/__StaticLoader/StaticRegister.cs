using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

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


public static class CharacterRegister
{
    public static void CharacterRegisterList() {
        Init_Search();
    }
    private static void Init_Search() {
        Type type = typeof(CharacterBase);
        Assembly assembly = Assembly.GetAssembly(type);
        foreach (Type Child in assembly.GetTypes()) {
            object[] vs = Child.GetCustomAttributes(typeof(CharacterAttribute), true);
            if (vs.Length > 0) {
                foreach (Attribute Att in vs) {
                    CharacterAttribute itemAttribute = Att as CharacterAttribute;
                    if (itemAttribute.enable) {
                        Characters.AddGenerators(itemAttribute.CharacterID, Child);
                    }
                }
            }
        }
        return;
    }
}

public static class PoolRegister
{

    public static void UIPoolRegisterList() {
        StaticPath.UIPool.__SetUP__Pool();
        StaticPath.BulletPool_Ram.__SetUP__Pool();
        StaticPath.BulletHolePool_Ram.__SetUP__Pool();
        StaticPath.BulletSmokePool_Ram.__SetUP__Pool();
    }
}