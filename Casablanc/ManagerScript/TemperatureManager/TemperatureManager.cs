using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermodynamicsManager : SingletonMono<ThermodynamicsManager>
{
    public static float EnvironmentTemperature = 15f;



    private void FixedUpdate() {
        
    }
}

public class ThermodynamicsGroup
{
    LinkedList<IThermodynamics_Unit> Heat_Units = new LinkedList<IThermodynamics_Unit>();
    float Sum = 0;
    float Div = 0;
    void FixedUpdate() {
        
    }
     
    void TempratureSum() {
        Sum = 0;       
        foreach (var Unit in Heat_Units) {
            Sum += Unit.Temperature;
        }
        Div = Sum / Heat_Units.Count;
    }
    void TempraturePass() {      
        foreach (var Unit in Heat_Units) {
            float k = Div - Unit.Temperature;
            //Unit.ThermodynamicsAdjustment();
        }
    }
}
