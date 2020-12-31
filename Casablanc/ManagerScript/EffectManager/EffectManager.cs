using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMono<EffectManager>
{
    public static bool SlowMode = false;
    public float normal_Time_rate = 1;
    public float slow_Time_rate = 0.1f;

    private void Update() {
        TimeSlow();
    }

    private void TimeSlow() {
        if (SlowMode) {
            Time.timeScale = slow_Time_rate;
        }
        else {
            Time.timeScale = normal_Time_rate;
        }
    }
}
