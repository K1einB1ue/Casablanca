using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest :MonoBehaviour
{
    public InputBase InputBase;

    private void OnEnable() {
        if (InputBase != null) {
            InputBase.RegisteInput(Move, InputType.MoveRight);
        }
    }


    private void Move() {
        this.gameObject.transform.position += 0.5f * Vector3.up;
    }
}
