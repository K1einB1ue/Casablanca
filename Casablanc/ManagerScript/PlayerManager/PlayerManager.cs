using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMono<PlayerManager>
{
    public static Character MainCharacter;


    





















































    #region 旧版本
    public static Player Main;
    private static HashGenerator HashGenerator = new HashGenerator();

    private static List<GameObject> PlayersGameobj=new List<GameObject>();

    private static bool inited = false;
    private static bool Mained = false;

    



    private void Awake() {
        
    }
    private void Start() {
        Init();
    }


    private void Update() {
        if (!inited) {
            update();
        }
        PlayerSwitch(KeyPress.P);
    }



    public class Key_Input
    {
        public int HashCode;
        public InputInterface Mask;
        public InputInterface Origin;
        public Player Player;
        public Key_Input(int HashCode) { this.HashCode = HashCode; this.Mask = null; this.Origin = null; this.Player = null; }

        public void swapInfo() {
            this.Origin.StateStore.Facing = this.Mask.StateStore.Facing;
            this.Origin.StateStore.Handing = this.Mask.StateStore.Handing;
        }
    }
    private static LinkedList<Key_Input> MaskAndOrigin_Inputs = new LinkedList<Key_Input>();
    private static Dictionary<int, LinkedListNode<Key_Input>> Inputs_Map = new Dictionary<int, LinkedListNode<Key_Input>>();
    private static void update() {
        LinkedListNode<Key_Input> tmp = null;
        if (MaskAndOrigin_Inputs.First != null) {
            tmp = MaskAndOrigin_Inputs.First;
        }
        while (tmp != null) {
            if (tmp.Value.Player.__PlayerStateSetting.Main) {
                Main = tmp.Value.Player;
                tmp.Value.Mask = new AllInput.PlayerInput(tmp.Value.Player);
                inited = true;
                CameraManager.CamBind(Main.GetMainTransform());
            }
            else {
                tmp.Value.Mask = AllInput.FalseInput;
            }
            tmp = tmp.Next;
        }
    }
    private static void Init() {
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player")) {
            if (Player.TryGetComponent<Player_Instance>(out Player_Instance player_Instance)) {
                if (player_Instance != null) {
                    int Hash= HashGenerator.GetHash();

                    ((PlayerManage)player_Instance.Instance).HashCode = Hash;
                    Key_Input tmp = new Key_Input(Hash);
                    tmp.Player = player_Instance.Instance;
                    //在未有主玩家时 Main一个带Main角色
                    if (player_Instance.Instance.__PlayerStateSetting.Main && !Mained) {
                        tmp.Mask = new AllInput.PlayerInput(tmp.Player);
                        tmp.Origin = new AllInput.AIInput(tmp.Player);
                        Main = player_Instance.Instance;
                        Mained = true;
                    }
                    //在有主玩家时 放弃Main一个带Main的角色
                    else if (player_Instance.Instance.__PlayerStateSetting.Main && Mained) {
                        tmp.Mask = new AllInput.FalsesInput();
                        tmp.Origin = new AllInput.AIInput(tmp.Player);
                        player_Instance.Instance.__PlayerStateSetting.Main = false;
                        Debug.Log("场景中主角色冲突,已修正.");
                    }
                    //憨憨机器人
                    else {
                        tmp.Mask = new AllInput.FalsesInput();
                        tmp.Origin = new AllInput.AIInput(tmp.Player);
                    }
                    
                    MaskAndOrigin_Inputs.AddLast(tmp);
                    Inputs_Map.Add(tmp.HashCode, MaskAndOrigin_Inputs.Last);
                }
            }
        }

        if (!Mained) {
            Debug.LogWarning("内存中失去主角色");
        }


    }

    public static InputInterface GetInputInterface(int Hash) {
        if (!object.Equals(Inputs_Map[Hash].Value.Mask, AllInput.FalseInput)) {
            return Inputs_Map[Hash].Value.Mask;
        }
        else {
            return Inputs_Map[Hash].Value.Origin;
        }
    }

    private static void PlayerSwitch(bool Trigger) {
        if (Trigger) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 11;
            layerMask = ~layerMask;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask)) {
                if (hit.transform.TryGetComponent<Player_Instance>(out Player_Instance player_Instance)) {
                    SetMain(player_Instance.Instance);
                }
            }
        }
    }
    public static void SetMain(Player player) {         
        if (Main != null) {
            if(Inputs_Map.TryGetValue(((PlayerManage)Main).HashCode,out LinkedListNode<Key_Input> value)){
                value.Value.swapInfo();
            }
            Main.__PlayerStateSetting.__OFF();
        }
        player.__PlayerStateSetting.__SetMainPlayer();
        update();
    }

    
    



    public static bool MainPlayerHeldTypeBool(ItemType itemType) {
        return Main.Held.Type == itemType;
    }
    #endregion
}
