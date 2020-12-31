using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Binaray_Tree_Entry<Component>
{
    Binary_Tree_Node<Component> Start;

    public Binaray_Tree_Entry(){
        this.Start = new Binary_Tree_Node<Component>() { Par = null };
    }

    public void Order_Insert(Binary_Tree_Node<Component> _Tree_Node) {
        this.Start.Order_Insert(_Tree_Node);
    }
}
public class Binary_Tree_Node<Component>
{
    int HashCode;
    Component Components;
    public Binary_Tree_Node<Component> Par;
    public Binary_Tree_Node<Component> L, R;


    public void Order_Insert(Binary_Tree_Node<Component> _Tree_Node) {
        Binary_Tree_Node<Component> temp = this;
        while (true) {
            if (_Tree_Node.HashCode < temp.HashCode) {
                if (temp.L != null) {
                    temp = temp.L;
                    
                }
                else {
                    temp.L = _Tree_Node;
                    return;
                }
            }
            else{
                if (temp.R != null) {
                    temp = temp.R;
                }
                else {
                    temp.R = _Tree_Node;
                    return;
                }
            }
        }
    }
    public int Get_TreeHash() {
        return this.HashCode;
    }

    //public bool Order_Remove_Match(Func<Binary_Tree_Node<Component>,bool> Action) { 
        
    //}
    //public bool Order_Remove(Binary_Tree_Node<Component> _Tree_Node,out Binary_Tree_Node<Component> _Tree_Node_Header) {
    //    Binary_Tree_Node<Component> temp = this;
    //    bool? flag = null;
    //    while (temp.HashCode != _Tree_Node.HashCode || temp.L != null || temp.R != null) {           
    //        if (temp.HashCode == _Tree_Node.HashCode) {
    //            if (flag == null) {
    //                _Tree_Node_Header
    //            }
    //            return true;
    //        }
    //        if (_Tree_Node.HashCode < temp.HashCode) {

    //        }
    //    }
    //}
}



public class Pair<T>
{
    public T first;
    public T Second;

    public Pair(T index1,T index2) {
        this.first = index1;
        this.Second = index2;
    }
    public Pair() {}
}

public class Pair<T,R>
{
    public T first;
    public R Second;

    public Pair(T index1, R index2) {
        this.first = index1;
        this.Second = index2;
    }
    public Pair() { }
}

