using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Moral_Axis {
    Good,
    Neutral,
    Evil,
}
public enum Order_Horizontal {
    Lawful,
    Neutral,
    Chaotic,
}
public class MoralStateMachine
{
    public Moral_Axis moral_Axis;
    public Order_Horizontal order_Horizontal;

    public void Doubt_Moral(float offset) {
    
    }
    public void Doubt_Order(float offset) {
    
    
    }
    public void Satisfy_Moral(float offset) {
    
    }
    public void Satisfy_Order(float offset) {
    
    }
    public int GetMoral_Order_Position() {
        switch (moral_Axis) {
            case Moral_Axis.Good:           switch (order_Horizontal) { case Order_Horizontal.Lawful: return 1; case Order_Horizontal.Neutral: return 2; case Order_Horizontal.Chaotic: return 3; } break;
            case Moral_Axis.Neutral:        switch (order_Horizontal) { case Order_Horizontal.Lawful: return 4; case Order_Horizontal.Neutral: return 5; case Order_Horizontal.Chaotic: return 6; } break;
            case Moral_Axis.Evil:           switch (order_Horizontal) { case Order_Horizontal.Lawful: return 7; case Order_Horizontal.Neutral: return 8; case Order_Horizontal.Chaotic: return 9; } break;
        }
        return 0;
    }
    public bool Suit(int num) {
        if (this.GetMoral_Order_Position() == num) {
            return true;
        }
        return false;
    }
}
