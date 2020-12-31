using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "玩家池", menuName = "玩家/玩家池",order =1)]
public class PlayerPool : ScriptableObject
{
    [SerializeField]
    private List<PlayerStore> __PlayerPoolOrigin = new List<PlayerStore>();
}
