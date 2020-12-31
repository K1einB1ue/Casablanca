using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLeader : MonoBehaviour
{
    public GameObject Target;

    public bool ITryGetComponent<T>(out T component) {
        if(this.Target.TryGetComponent<T>(out T c)) {
            component = c;
            return true;
        }
        component = default(T);
        return false;
    }
}
