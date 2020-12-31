using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "新玩家", menuName = "玩家/新玩家")]
public class PlayerStore : ScriptableObject
{
    [SerializeField]
    private GameObject Player;
}
