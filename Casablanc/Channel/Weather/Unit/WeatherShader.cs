using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MaterialType
{
    Shadred,
    Unit,
}

public class WeatherShader : ChannelListenerUnitBase<WeatherChannelManager,WeatherInfoChannel>
{
    private Material material; 
    private bool InSight = false;
    public MaterialType MaterialType = MaterialType.Shadred;
    
    public override bool IsEnable => InSight;

    private void Awake() {
        if (this.MaterialType == MaterialType.Shadred) {
            this.material = this.GetComponent<Renderer>().sharedMaterial;
        }else if(this.MaterialType== MaterialType.Unit) {
            this.material = this.GetComponent<Renderer>().material;
        }
    }
    public override void ChannelMassage(object Channel, Func<string, Func<object, object>> FieldGets) {
        for(int i = 0; i < AdjustMents.Count; i++) {
            switch (AdjustMents[i].ParameterType) {
                case ParameterType.INT: this.material.SetInt(AdjustMents[i].ObjectParameter, AdjustMents[i].GetIntValue(Channel, FieldGets));break;
                case ParameterType.FLOAT: this.material.SetFloat(AdjustMents[i].ObjectParameter, AdjustMents[i].GetFloatValue(Channel, FieldGets)); break;
            }
        }
    }

    private void OnBecameVisible() {
        InSight = true;
        this.Enter();
    }
    private void OnBecameInvisible() {
        InSight = false;
    }
}
