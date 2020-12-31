using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyLoad : SingletonMono<KeyLoad>
{
    private void Awake() {
        KeyPress.__SetUp();
    }
    private void Update() {
        KeyPress.GetKeyLoad();
    }
}


public static class KeyPress
{
    public static bool K1;
    public static bool K2;
    public static bool K3;
    public static bool K4;
    public static bool K5;
    public static bool K6;
    public static bool K7;
    public static bool[] K;
    public static bool W;
    public static bool A;
    public static bool S;
    public static bool D;
    public static bool R;
    public static bool F;
    public static bool T;
    public static bool L_Down;
    public static bool R_Down;
    public static bool G;
    public static bool P;
    public static bool L;
    public static bool _Shift;
    public static bool _Ctrl;
    public static bool _Tab;
    public static bool _Esc;

    public static void __SetUp() {
        K = new bool[7];
    }
    public static void GetKeyLoad() {
        W = Input.GetKey(KeyCode.W);
        A = Input.GetKey(KeyCode.A);
        S = Input.GetKey(KeyCode.S);
        D = Input.GetKey(KeyCode.D);
        F = Input.GetKey(KeyCode.F);
        G = Input.GetKey(KeyCode.G);
        T = Input.GetKey(KeyCode.T);
        P = Input.GetKey(KeyCode.P);
        L = Input.GetKey(KeyCode.L);
        R = Input.GetKey(KeyCode.R);
        if (Input.GetKey(KeyCode.Alpha1)) { K1 = true; K[0] = true; }
        else { K1 = false; K[0] = false; }
        if (Input.GetKey(KeyCode.Alpha2)) { K2 = true; K[1] = true; }
        else { K2 = false; K[1] = false; }
        if (Input.GetKey(KeyCode.Alpha3)) { K3 = true; K[2] = true; }
        else { K3 = false; K[2] = false; }
        if (Input.GetKey(KeyCode.Alpha4)) { K4 = true; K[3] = true; }
        else { K4 = false; K[3] = false; }
        if (Input.GetKey(KeyCode.Alpha5)) { K5 = true; K[4] = true; }
        else { K5 = false; K[4] = false; }
        if (Input.GetKey(KeyCode.Alpha6)) { K6 = true; K[5] = true; }
        else { K6 = false; K[5] = false; }
        if (Input.GetKey(KeyCode.Alpha7)) { K7 = true; K[6] = true; }
        else { K7 = false; K[6] = false; }
        if (Input.GetMouseButton(0)) L_Down = true;
        else L_Down = false;
        if (Input.GetMouseButton(1)) R_Down = true;
        else R_Down = false;
        if (Input.GetKey(KeyCode.LeftShift)) _Shift = true;
        else _Shift = false;
        if (Input.GetKey(KeyCode.LeftControl)) _Ctrl = true;
        else _Ctrl = false;
        if (Input.GetKey(KeyCode.Tab)) _Tab = true;
        else _Tab = false;
        if (Input.GetKey(KeyCode.Escape)) _Esc = true;
        else _Esc = false;

    }

}
