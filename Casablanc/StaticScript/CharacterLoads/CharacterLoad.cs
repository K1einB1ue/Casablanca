using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoad : ScriptableObject
{
    [SerializeField]
    public List<CharacterStore> characterlist;
    private Dictionary<int, CharacterStore> CharacterStatics;



    private void OnEnable() {
        this.CharacterStatics = new Dictionary<int, CharacterStore>();
        foreach (var characterStore in characterlist) {
            if (characterStore != null) {
                if( this.CharacterStatics.TryGetValue(characterStore.CharacterStaticProperties.CharacterID,out var character)) {
                    Debug.LogError("ÖØ¸´µÄ½ÇÉ«ID!");
                }
                else {
                    this.CharacterStatics[characterStore.CharacterStaticProperties.CharacterID] = character;
                }
            }
        }
    }

    public CharacterStore this[int characterID] {
        get {
            if (this.CharacterStatics.TryGetValue(characterID, out var character)) {
                return character;
            }
            return null;         
        }
    }


}

