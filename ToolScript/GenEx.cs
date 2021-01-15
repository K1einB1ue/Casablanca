using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenEx
{
    public static void Gen(Transform Tran,string name) {
        GameObject tmp = new GameObject();
        tmp.transform.parent = Tran;
        tmp.transform.position = Tran.position;
        tmp.name = name;
    }
    public static void GenerateSon(this Transform Tran,string name) {
        GameObject tmp = new GameObject();
        tmp.transform.parent = Tran;
        tmp.transform.position = Tran.position;
        tmp.name = name;
    }
}
