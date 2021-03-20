using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterOnTheGround : MonoBehaviour, ObjectOnTheGround 
{
    object ObjectOnTheGround.Object { get => characterOntheGround; }
    Object_Values_Handler ObjectOnTheGround.Object_Values_Handler { get => characterOntheGround.Character_Values_Handler; }
    public Character characterOntheGround;

    public CharacterInfoStore CharacterInfoStore;
    public Info_Type Info_Type = Info_Type.Properties;
    public int CharacterID = -1;

    public CharacterStore CharacterStore;



    public Info_Type Detail_Info_Type = Info_Type.Properties;
    public ItemInfoStore ItemInfoStore;




    public InputBase Input;
    public List<ItemNodeDynamic> Cotent = new List<ItemNodeDynamic>();







    [SerializeField]
    private CharacterPreInstanceProperties.CharacterPreInstanceProperties CharacterPreInstanceProperties = new CharacterPreInstanceProperties.CharacterPreInstanceProperties();
    [SerializeField]
    private bool InitInScene = false;
    private void OnEnable() {
        if (InitInScene) {
            CharacterOnTheGroundInit();
            this.characterOntheGround.Info_Handler.ReplaceInstance(this.gameObject);
        }
        characterOntheGround.OnEnable();
    }

    private void Update() {
        if (characterOntheGround != null) {
            ((ObjectOnGroundBase)characterOntheGround).UpdateBase();
        }
    }

    private void FixedUpdate() {
        if (characterOntheGround != null) {
            characterOntheGround.FixedUpdate();
        }
    }

    private void OnDisable() {
        if (this.Info_Type == Info_Type.InfoStore) {
            this.CharacterInfoStore.Save(characterOntheGround);
            this.CharacterInfoStore.characterNodeDynamic.CharacterRuntimeInfoPackage.CharacterRuntimeProperties.CharacterRuntimeValues.RuntimeValues_Bags.ItemInfoStore.StoreContainer(characterOntheGround);
        }
        else if (this.Info_Type== Info_Type.Properties){
            
        }
    }
    private void OnCollisionEnter(Collision collision) {
        ((ObjectOnGroundBase)characterOntheGround).CollisionEnterBase(collision);
    }
    private void OnCollisionExit(Collision collision) {
        ((ObjectOnGroundBase)characterOntheGround).CollisionExitBase(collision);
    }

   
    private void OnTriggerEnter(Collider other) {
        ((ObjectOnGroundBase)characterOntheGround).TriggerEnterBase(other);
        if (other.gameObject.TryGetComponent<CoverTriggerComponent>(out CoverTriggerComponent coverTriggerComponent)) {
            coverTriggerComponent.Disable();
        }
    }
    private void OnTriggerExit(Collider other) {
        ((ObjectOnGroundBase)characterOntheGround).TriggerExitBase(other);
        if (other.gameObject.TryGetComponent<CoverTriggerComponent>(out CoverTriggerComponent coverTriggerComponent)) {
            coverTriggerComponent.Enable();
        }
    }
    private void OnTriggerStay(Collider other) {
        ((ObjectOnGroundBase)characterOntheGround).TriggerStayBase(other);
    }
    private void CharacterOnTheGroundInit() {
        if (this.Info_Type == Info_Type.InfoStore) {
            characterOntheGround = this.CharacterInfoStore.GetCharacter();
        }
        else if (this.Info_Type == Info_Type.Properties) {
            characterOntheGround = Characters.GetCharacterByCharacterID_With_StaticProperties_And_PreInstanceProperties(this.CharacterID, this.CharacterPreInstanceProperties);

            if (this.Detail_Info_Type == Info_Type.InfoStore) {
                Debug.Log("似乎还没实现CharacterOntheGround中的Infostore");
            }
            else if (this.Detail_Info_Type == Info_Type.Properties) {
                Item bag = Items.GetItemByItemTypeAndItemIDStatic(ItemType.Container, 0);
                for (int i = 0; i < Cotent.Count; i++) {
                    if (Items.GetIsContainerByItemTypeAndItemID(Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemType, Cotent[i].ItemRuntimeInfoPackage.ItemRuntimeProperties.ItemID)) {
                        ((ScriptContainer)bag).SetItem(i, Cotent[i].GetContainer());
                    }
                    else {
                        ((ScriptContainer)bag).SetItem(i, Cotent[i].GetItem());
                    }
                }
            }
        }
    }
}


[CustomEditor(typeof(CharacterOnTheGround))]
public class CharacterOnTheGroundEditor : Editor
{
    SerializedProperty Info_Type;
    SerializedProperty CharacterID;
    SerializedProperty InitInScene;
    SerializedProperty CharacterInfoStore;
    SerializedProperty ItemInfoStore;
    SerializedProperty Input;
    SerializedProperty CharacterPreInstanceProperties;
    SerializedProperty Detail_Info_Type;
    SerializedProperty Cotent;
    SerializedProperty CharacterStore;
    public override void OnInspectorGUI() {
        serializedObject.Update();
        CharacterOnTheGround character = target as CharacterOnTheGround;


        Info_Type = serializedObject.FindProperty("Info_Type");
        InitInScene = serializedObject.FindProperty("InitInScene");
        CharacterInfoStore = serializedObject.FindProperty("CharacterInfoStore");
        ItemInfoStore = serializedObject.FindProperty("ItemInfoStore");
        CharacterID = serializedObject.FindProperty("CharacterID");
        Input = serializedObject.FindProperty("Input");
        CharacterStore = serializedObject.FindProperty("CharacterStore");
        CharacterPreInstanceProperties = serializedObject.FindProperty("CharacterPreInstanceProperties");
        Detail_Info_Type = serializedObject.FindProperty("Detail_Info_Type");
        Cotent = serializedObject.FindProperty("Cotent");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(Info_Type);


        if(character.Info_Type== global::Info_Type.InfoStore) {
            EditorGUILayout.PropertyField(CharacterInfoStore);
        }
        else if(character.Info_Type== global::Info_Type.Properties) {
            EditorGUILayout.PropertyField(Detail_Info_Type);
            EditorGUILayout.PropertyField(CharacterID);
            if (character.Detail_Info_Type == global::Info_Type.InfoStore) {
                EditorGUILayout.PropertyField(ItemInfoStore);
            }
            else if(character.Detail_Info_Type== global::Info_Type.Properties) {
                while (character.Cotent.Count < 7) {
                    character.Cotent.Add(new ItemNodeDynamic());
                }
                while (character.Cotent.Count > 7) {
                    character.Cotent.RemoveAt(character.Cotent.Count - 1);
                }
                EditorGUILayout.PropertyField(Cotent);
            }
            EditorGUILayout.PropertyField(Input);
            EditorGUILayout.PropertyField(CharacterPreInstanceProperties);
        }else if(character.Info_Type== global::Info_Type.Store) {
            EditorGUILayout.PropertyField(Detail_Info_Type);
            EditorGUILayout.PropertyField(CharacterStore);
            if (character.Detail_Info_Type == global::Info_Type.InfoStore) {
                EditorGUILayout.PropertyField(ItemInfoStore);
            }
            else if (character.Detail_Info_Type == global::Info_Type.Properties) {
                while (character.Cotent.Count < 7) {
                    character.Cotent.Add(new ItemNodeDynamic());
                }
                while (character.Cotent.Count > 7) {
                    character.Cotent.RemoveAt(character.Cotent.Count - 1);
                }
                EditorGUILayout.PropertyField(Cotent);
            }
            EditorGUILayout.PropertyField(Input);
            EditorGUILayout.PropertyField(CharacterPreInstanceProperties);
        }
        if (GUILayout.Button("生成部件")) {
            this.GeneratePart(character.gameObject.transform);
        }
        EditorGUILayout.PropertyField(InitInScene);

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
    }


    private void GeneratePart(Transform Tran) {
        if (Tran.FindSon("连接层") == null) {
            Tran.GenerateSon("连接层");
        }
        if (Tran.FindSon("连接层").FindSon("模型") == null) {
            Tran.FindSon("连接层").GenerateSon("模型");
        }
        if (Tran.FindSon("连接层").FindSon("头部") == null) {
            Tran.FindSon("连接层").GenerateSon("头部");
        }
        if (Tran.FindSon("连接层").FindSon("手部") == null) {
            Tran.FindSon("连接层").GenerateSon("手部");
        }

    }

}

