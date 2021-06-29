using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MotionStore : ScriptableObject
{
    Vector3 Prediction;
}
public class MotionFields
{
    Vector3 Prediction;
    List<Animation> Animations;

    

    Animation FindMatch() {
        AnimationClip clip = this.Animations[0].clip;
        clip.
    }
}
