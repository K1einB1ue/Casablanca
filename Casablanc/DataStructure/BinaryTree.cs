using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructureEnum;
namespace DataStructureEnum
{
    public enum OrderType
    {
        PreOrder,
        Infix,
        PostOrder
    }
    public enum DirectionType
    {
        forward,
        backward,
    }
    public enum LayerType
    {
        lower,
        higher,
    }
}

public class BigRootHeap<T> : BinaryTree<T> where T : IComparable
{
    public bool Empty {
        get {
            return !this.Root.InitValue;
        }
    }

    public BigRootHeap(List<T> List) : base(List) {
        this.BuildBigRoot();
    }
    public BigRootHeap(params T[] Index) : base(Index) {
        this.BuildBigRoot();
    }

    public override void PushInLayer(params T[] Index) {
        base.PushInLayer(Index);
        this.BuildBigRoot();
    }
    public void BuildBigRoot() {
        LayerInvoke((node) => { Build(node); }, LayerType.higher, DirectionType.forward);
    }

    public void Build(BinaryTreeNode<T> node) {
        bool left = false;
        if (node.Pair.Left.InitValue && node.Pair.Right.InitValue) {
            left = node.Pair.Left.Data.CompareTo(node.Pair.Right.Data) > 0;
        }
        else if (node.Pair.Left.InitValue && !node.Pair.Right.InitValue) {
            left = true;
        }
        else if (!node.Pair.Left.InitValue && node.Pair.Right.InitValue) {
            left = false;
        }
        else if (!node.Pair.Right.InitValue && !node.Pair.Left.InitValue) {
            return;
        }

        if (left) {
            if (node.Data.CompareTo(node.Pair.Left.Data) < 0) {
                node.Swap(node.Pair.Left);
                Build(node.Pair.Left);
            }
            return;
        }
        if (!left) {
            if (node.Data.CompareTo(node.Pair.Right.Data) < 0) {
                node.Swap(node.Pair.Right);
                Build(node.Pair.Right);
            }
            return;
        }
        return;
    }

    public T Pop() {
        if (Root.InitValue) {
            T Ret = Root.Data;
            int count = 1;
            BinaryTreeNode<T> temp = null;
            this.OrderInvoke((node) => { temp = node; }, ref count, OrderType.PostOrder);
            if (temp.InitValue) {
                temp.Swap(Root);
                temp.Restruct();
                LayerBuffer[LayerBuffer.Count - 1].Remove(temp);
                Build(Root);
            }

            return Ret;
        }
        else {
            return default;
        }
    }

    public void Push(List<T> List) {
        Push(List.ToArray());
    }
    public void Push(params T[] Index) {
        this.PushInLayer(Index);
    }
}



public class BinaryTree<T> : IEnumerable
{
    public BinaryTreeNode<T> Root;
    public Queue<BinaryTreeNode<T>> PushBuffer = new Queue<BinaryTreeNode<T>>();

    public Queue<BinaryTreeNode<T>> LayBuffer = new Queue<BinaryTreeNode<T>>();

    public LinkedList<BinaryTreeNode<T>> IEnumeratList = new LinkedList<BinaryTreeNode<T>>();

    public List<List<BinaryTreeNode<T>>> LayerBuffer = new List<List<BinaryTreeNode<T>>>();





    public BinaryTree(List<T> List) : this(List.ToArray()) { }
    public BinaryTree(params T[] Index) {
        if (Index.Length != 0) {
            Root = new BinaryTreeNode<T>(this, null);
            Root.PushInLayer(Index[0]);
        }
        for (int i = 1; i < Index.Length; i++) {
            var temp = PushBuffer.Dequeue();
            temp.PushInLayer(Index[i]);
        }
    }


    public virtual void PushInLayer(params T[] Index) {
        RestructPushBuffer();
        if (Root == null) {
            if (Index.Length != 0) {
                Root = new BinaryTreeNode<T>(this, null);
                Root.PushInLayer(Index[0]);
            }
            for (int i = 1; i < Index.Length; i++) {
                var temp = PushBuffer.Dequeue();
                temp.PushInLayer(Index[i]); ;
            }
        }
        else {
            for (int i = 0; i < Index.Length; i++) {
                var temp = PushBuffer.Dequeue();
                temp.PushInLayer(Index[i]);
                if (!temp.Pair.Left.InitValue) {
                    this.PushBuffer.Enqueue(temp.Pair.Left);
                }
                if (!temp.Pair.Right.InitValue) {
                    this.PushBuffer.Enqueue(temp.Pair.Right);
                }

            }
        }
    }
    public List<T> ListLize() {
        List<T> Ret = new List<T>();
        LayBuffer.Clear();
        LayBuffer.Enqueue(Root);
        while (LayBuffer.Count != 0) {
            var temp = LayBuffer.Dequeue();
            Ret.Add(temp.Data);
            if (temp.Pair.Left.InitValue) {
                LayBuffer.Enqueue(temp.Pair.Left);
            }
            if (temp.Pair.Right.InitValue) {
                LayBuffer.Enqueue(temp.Pair.Right);
            }
        }
        return Ret;
    }


    public void LayerInvoke(Action<BinaryTreeNode<T>> action, LayerType layerType, DirectionType directionType) {
        RestructLayerList();
        if (layerType == LayerType.higher) {
            if (directionType == DirectionType.backward) {
                for (int i = LayerBuffer.Count - 1; i >= 0; i--) {
                    for (int j = 0; j < LayerBuffer[i].Count; j++) {
                        action.Invoke(LayerBuffer[i][j]);
                    }
                }
            }
            else if (directionType == DirectionType.forward) {
                for (int i = LayerBuffer.Count - 1; i >= 0; i--) {
                    for (int j = LayerBuffer[i].Count - 1; j >= 0; j--) {
                        action.Invoke(LayerBuffer[i][j]);
                    }
                }
            }
        }
        if (layerType == LayerType.lower) {
            if (directionType == DirectionType.backward) {
                for (int i = 0; i < LayerBuffer.Count; i++) {
                    for (int j = 0; j < LayerBuffer[i].Count; j++) {
                        action.Invoke(LayerBuffer[i][j]);
                    }
                }
            }
            else if (directionType == DirectionType.forward) {
                for (int i = 0; i < LayerBuffer.Count; i++) {
                    for (int j = LayerBuffer[i].Count - 1; j >= 0; j--) {
                        action.Invoke(LayerBuffer[i][j]);
                    }
                }
            }
        }
    }
    public void RestructLayerList() {
        this.LayerBuffer.Clear();
        this.OrderInvoke((node, var) => {
            while (this.LayerBuffer.Count <= var) {
                this.LayerBuffer.Add(new List<BinaryTreeNode<T>>());
            }
            this.LayerBuffer[var].Add(node);
        });
    }

    public void RestructPushBuffer() {
        PushBuffer.Clear();
        LayBuffer.Clear();
        LayBuffer.Enqueue(Root);
        while (LayBuffer.Count != 0) {
            var temp = LayBuffer.Dequeue();

            if (temp.Pair.Left.InitValue) {
                LayBuffer.Enqueue(temp.Pair.Left);
            }
            else {
                PushBuffer.Enqueue(temp.Pair.Left);
            }
            if (temp.Pair.Right.InitValue) {
                LayBuffer.Enqueue(temp.Pair.Right);
            }
            else {
                PushBuffer.Enqueue(temp.Pair.Right);
            }
        }
    }


    public void OrderInvoke(Action<BinaryTreeNode<T>> action, OrderType orderType = OrderType.PreOrder) {
        this.Root.Invoke(action, orderType);
    }
    public void OrderInvoke(Action<BinaryTreeNode<T>, int> action, OrderType orderType = OrderType.PreOrder) {
        this.Root.Invoke(action, orderType, -1);
    }
    public void OrderInvoke(Action<BinaryTreeNode<T>> action, ref int num, OrderType orderType = OrderType.PreOrder) {
        this.Root.Invoke(action, orderType, ref num);
    }
    public IEnumerator GetEnumerator() {
        return IEnumeratList.GetEnumerator();
    }

    public IEnumerable<T> Find(Func<T, bool> func) {
        foreach (BinaryTreeNode<T> temp in this) {
            if (func.Invoke(temp.Data)) {
                yield return temp.Data;
            }
        }
    }
}

public class BinaryTreePair<T>
{
    public BinaryTreeNode<T> Left;
    public BinaryTreeNode<T> Right;
}


public class BinaryTreeNode<T>
{
    private BinaryTree<T> Root;
    public BinaryTreePair<T> Pair;
    private T data;
    private bool init = false;
    public bool InitValue {
        get {
            return init;
        }
    }
    public T Data {
        get {
            return data;
        }
        set {
            if (!this.init) {
                this.Root.IEnumeratList.AddLast(this);
                this.init = true;
            }
            data = value;
        }
    }

    public BinaryTreeNode(BinaryTree<T> Root, BinaryTreeNode<T> Par) {
        this.Root = Root;
        this.Pair = new BinaryTreePair<T>();
    }
    public void PushInLayer(T Index) {
        this.Data = Index;
        this.Pair.Left = new BinaryTreeNode<T>(this.Root, this);
        this.Pair.Right = new BinaryTreeNode<T>(this.Root, this);
        Root.PushBuffer.Enqueue(this.Pair.Left);
        Root.PushBuffer.Enqueue(this.Pair.Right);
    }
    public void Swap(BinaryTreeNode<T> node) {
        T temp;
        temp = this.Data;
        this.Data = node.Data;
        node.Data = temp;
    }
    public void Restruct() {
        this.init = false;
        this.data = default;
        this.Root.IEnumeratList.Remove(this);
    }


    public void Invoke(Action<BinaryTreeNode<T>> action, OrderType orderType) {
        switch (orderType) {
            case OrderType.PreOrder:
                action.Invoke(this);
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType);
                }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType);
                }
                break;
            case OrderType.Infix:
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType);
                }
                action.Invoke(this);
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType);
                }
                break;
            case OrderType.PostOrder:
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType);
                }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType);
                }
                action.Invoke(this);
                break;
        }
    }
    public void Invoke(Action<BinaryTreeNode<T>, int> action, OrderType orderType, int Layer) {
        Layer++;
        switch (orderType) {
            case OrderType.PreOrder:
                action.Invoke(this, Layer);
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType, Layer);
                }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType, Layer);
                }
                break;
            case OrderType.Infix:
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType, Layer);
                }
                action.Invoke(this, Layer);
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType, Layer);
                }
                break;
            case OrderType.PostOrder:
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType, Layer);
                }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType, Layer);
                }
                action.Invoke(this, Layer);
                break;
        }
    }

    public void Invoke(Action<BinaryTreeNode<T>> action, OrderType orderType, ref int countDown) {
        switch (orderType) {
            case OrderType.PreOrder:
                action.Invoke(this);
                if (countDown-- == 0) { return; }
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType);
                }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType);
                }
                break;
            case OrderType.Infix:
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType);
                }
                action.Invoke(this);
                if (countDown-- == 0) { return; }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType);
                }
                break;
            case OrderType.PostOrder:
                if (this.Pair.Left.InitValue) {
                    this.Pair.Left.Invoke(action, orderType);
                }
                if (this.Pair.Right.InitValue) {
                    this.Pair.Right.Invoke(action, orderType);
                }
                action.Invoke(this);
                if (countDown-- == 0) { return; }
                break;
        }
    }
}

