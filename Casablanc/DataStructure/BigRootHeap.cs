using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRootHeap<T> where T : IComparable
{
    BigRootHeapNode<T> Root;

    public void Push(T Data) {
         
    }



}



public class BigRootHeapNode<T> : TreeNode<T> where T : IComparable
{
    void Adjust() {
        if (this.Data.CompareTo(this.Left.Data) > 0 && this.Data.CompareTo(this.Right.Data) > 0) {
            return;
        }
        if (this.Right.Data.CompareTo(this.Left.Data) > 0) {
            T tmp = this.Data;
            
        }
    }
}

public class TreeNode<T> where T : IComparable
{
    public T Data;
    public TreeNode<T> Left;
    public TreeNode<T> Right;
}