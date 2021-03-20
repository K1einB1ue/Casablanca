using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherVFX : ChannelListenerUnitBase<WeatherChannelManager, WeatherInfoChannel>
{
    private VisualEffect VisualEffect;
    public override bool IsEnable => !VisualEffect.culled;
    
    private void Awake() {
        VisualEffect ??= this.GetComponent<VisualEffect>();
    }

    public override void ChannelMassage(object Channel, Func<string, Func<object, object>> FieldGets) {
        for (int i = 0; i < AdjustMents.Count; i++) {
            switch (AdjustMents[i].ParameterType) {
                case ParameterType.INT: this.VisualEffect.SetInt(AdjustMents[i].ObjectParameter, AdjustMents[i].GetIntValue(Channel, FieldGets)); break;
                case ParameterType.FLOAT: this.VisualEffect.SetFloat(AdjustMents[i].ObjectParameter, AdjustMents[i].GetFloatValue(Channel, FieldGets)); break;
            }
        }
    }
    void Update()
    {
        if (IsEnable) {
            this.Enter();
        }
    }
}
