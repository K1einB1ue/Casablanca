using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ÐÂ´æ´¢", menuName = "½ÇÉ«/¶¯¾²Ì¬»ìºÏ´æ´¢")]
public class CharacterInfoStore : ScriptableObject
{
    public CharacterNodeStatic CharacterNodeStatic;
    public CharacterNodeDynamic characterNodeDynamic;


    public ItemInfoStore ItemInfoStore;
    public InputBase Input;


    public bool init = false;


    public Character GetCharacter() {
        if (!this.init) {
            this.init = true;
            return this.CharacterNodeStatic.GetCharacter();
        }
        else {
            return this.characterNodeDynamic.GetCharacter();
        }
    }

    public Character GetNonSavaCharacter() {
        Character character = this.CharacterNodeStatic.GetCharacter();
        character.Info_Handler.Binds(Input);
        character.Info_Handler.Binds(ItemInfoStore.GetItem());
        return character;
    }

    public void SaveItem(Character character) {
        ItemNodeDynamic content = new ItemNodeDynamic(character.Info_Handler.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Bag.PlayerBag);
        content.ItemContain = ((ScriptContainer)character.Info_Handler.Character_Property.CharacterRuntimeProperties.CharacterRuntimeBinds.RuntimeBinds_Bag.PlayerBag).GetItemNodes();
        this.ItemInfoStore.itemNodesD = content;
    }
}