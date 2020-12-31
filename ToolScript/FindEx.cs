using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindEx 
{
    public static IEnumerable<Transform> Find(Transform par, Func<string, bool> match) {
        for (int i =0; i < par.childCount; i++) {
            if (match.Invoke(par.GetChild(i).name)) {
                yield return par.GetChild(i);
            }
            foreach (var son in Find(par.GetChild(i), match)) {
                yield return son;
            }
        }
    }
    public static Transform Find(Transform par, string name) {
        for (int i = 0; i < par.childCount; i++) {
            if (par.GetChild(i).name==name) {
                return par.GetChild(i);
            }
            return Find(par, name);
        }
        return null;
    }

}
