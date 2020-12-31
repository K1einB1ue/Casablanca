using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashGenerator : MonoBehaviour
{
    Dictionary<int, bool> VisTable = new Dictionary<int, bool>();
    private int Mark = -1;



    public int GetHash() {
        for(int i= this.Mark; i< 2147483647; i++) {
            if(VisTable.TryGetValue(i,out bool trigger)) {
                if (!trigger) {
                    Mark = i;
                    return i;
                }
            }
            else {
                VisTable.Add(i, true);
                Mark = i;
                return i;
            }
        }
        for(int i = 0; i < this.Mark; i++) {
            if (VisTable.TryGetValue(i, out bool trigger)) {
                if (!trigger) {
                    Mark = i;
                    return i;
                }
            }
            else {
                VisTable.Add(i, true);
                Mark = i;
                return i;
            }
        }
        Debug.LogError("Hash生成器超出限制");
        Mark = -1;
        return -1;
    }
    public void DisHash(int index) {
        if(VisTable.TryGetValue(index,out bool trigger)) {
            if (trigger) {
                trigger = false;
            }
            else {
                Debug.LogWarning("HashGenerator 注销了错误的Hash值!");
            }
        }
        else {
            Debug.LogError("HashGenerator 注销了不存在的Hash值!");
        }
    }




}
