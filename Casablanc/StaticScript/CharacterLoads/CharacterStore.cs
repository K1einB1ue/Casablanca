using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="新角色", menuName ="角色/新角色")]
public class CharacterStore : ScriptableObject
{
    [Header("静态角色配置")]
    public CharacterStaticProperties.CharacterStaticProperties CharacterStaticProperties;
    
}
