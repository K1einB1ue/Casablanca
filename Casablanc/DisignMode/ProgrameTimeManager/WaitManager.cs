using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitManager
{ 
    public static IEnumerator WaitToEndOfFrame() {
        yield return new WaitForEndOfFrame();
    }
}
