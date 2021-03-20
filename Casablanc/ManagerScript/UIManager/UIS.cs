using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[AttributeUsage(AttributeTargets.Class)]
public class UIAttribute : Attribute {
    public int UIID;
    public UI_TYPE UI_TYPE;
    public int ParID = -1;
    public bool enable = true;
    public UIAttribute(UI_TYPE uI_TYPE, int uiID) {
        this.UI_TYPE = uI_TYPE;
        this.UIID = uiID;
    }
    public UIAttribute(UI_TYPE uI_TYPE, int uiID, int ParID) {
        this.UI_TYPE = uI_TYPE;
        this.UIID = uiID;
        this.ParID = ParID;
    }
    public UIAttribute(UI_TYPE uI_TYPE, int uiID, int ParID, bool enable) {
        this.UI_TYPE = uI_TYPE;
        this.UIID = uiID;
        this.ParID = ParID;
        this.enable = enable;
    }

}

public static class UIS
{
    public static Dictionary<int, Func<UI>> Generators = new Dictionary<int, Func<UI>>();

    public static void AddGenerators(int UIID,UI_TYPE uI_TYPE,int ParID,Type type) {
        Generators.Add(UIID, () => {          
            UI tmp = (UI)type.Assembly.CreateInstance(type.FullName);
            ((UIInit)tmp).Kernel_Init(UIID, uI_TYPE, ParID);
            return tmp;
        });
    }
    private static bool Init = false;
    public static void INIT() {
        if (!Init) {
            if (StaticPath.UIPool != null) {
                for (int i = 0; i < StaticPath.UIPool.__Count; i++) {
                    for (int j = 0; j < StaticPath.UIPool.__Size[i]; j++) {
                        UIManager.ALLUI.Add(Generators[StaticPath.UIPool.IDBackMap[i]].Invoke());
                    }
                }
            }
            foreach (UI UIS in UIManager.ALLUI) {
                UIS.start();
            }
            UIManager.AwakeALL();
            Init = true;
        }
    }

    [UI(UI_TYPE.Static,0)]
    public class ToolBar : UIBase
    {

        private GameObject[] Frame, Num, Image;

        public ToolBar() { }
         

        public override void Awake() {
            string SonName = "Item";
            this.Frame = new GameObject[7];
            this.Num = new GameObject[7];
            this.Image = new GameObject[7];
            for (int i = 0; i < 7; i++) {
                this.Frame[i] = this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Frame").gameObject;
                this.Num[i] = this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Num").gameObject;
                this.Image[i] = this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Image").gameObject;
            }
        }
        public override void update() {
            this.Enable(true);
            base.update();
        }
        public override void Update() {
            DrawBagImages();
            DrawVIT();
            DrawHP();
        }

        void DrawBagImages() {
            for (int i = 0; i < 7; i++) {
                Frame[i].SetActive(CharacterManager.Main.Character_UI_Handler.Heldnum == i);
                Num[i].SetActive(((ItemUI)CharacterManager.Main.Bag[i]).Displaycount());
                Num[i].GetComponent<Text>().text = ((ItemUI)CharacterManager.Main.Bag[i]).GetUIheld().ToString();
                if (CharacterManager.Main.Bag[i] != null) {
                    if (CharacterManager.Main.Bag[i].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid == null) {
                        Image[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                    else {
                        Image[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        if (CharacterManager.Main.Bag[i].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid != null) {
                            Image[i].GetComponent<Image>().sprite = CharacterManager.Main.Bag[i].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;
                        }
                    }
                }
            }
        }

        void DrawVIT() {
            this.UIgraph.Instance.transform.Find("体力条").Find("Image").GetComponent<Image>().fillAmount = (int)(CharacterManager.Main.Info_Handler.PPrate * 50) / 50f;
        }
        void DrawHP() {
            this.UIgraph.Instance.transform.Find("血条").Find("Image").GetComponent<Image>().fillAmount = (int)(CharacterManager.Main.Info_Handler.HPrate * 50) / 50f;
        }
    }

    //[UI(UI_TYPE.StaticSon,3,0)]
    //public class GunBar : UIBase
    //{
    //    public GunBar() { }

    //    public override void update() {
    //        this.Enable(CharacterManager.Main.Held.Type == ItemType.Gun);
    //        base.update();
    //    }
    //    public override void Update() {
    //        this.DrawMagazine();
    //    }
    //    void DrawMagazine() {
    //        List<Item> Magazines = ((Gun)CharacterManager.Main.Held).FindMarchMagazine();
    //        if (Magazines != null) {
    //            int Count = Magazines.Count >= 2 ? 2 : Magazines.Count;
    //            for (int i = 1; i < 3; i++) {
    //                this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).gameObject.SetActive(i <= Count);
    //            }
    //            for (int i = 0; i <= Count; i++) {
    //                if (i == 0) {
    //                    if (((Gun)CharacterManager.Main.Held).magazine != Items.Empty) {
    //                        this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").gameObject.SetActive(true);
    //                        this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").GetComponent<Image>().sprite = ((Gun)CharacterManager.Main.Held).magazine.Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;
    //                        this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").Find("Rate").GetComponent<Image>().fillAmount = ((UIMagazine)((Gun)CharacterManager.Main.Held).magazine).GetBulletRate();
    //                    }
    //                    else {
    //                        this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").gameObject.SetActive(false);
    //                    }
    //                }
    //                else {
    //                    this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").GetComponent<Image>().sprite = Magazines[i - 1].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;
    //                    this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").Find("Rate").GetComponent<Image>().fillAmount = ((UIMagazine)Magazines[i - 1]).GetBulletRate();
    //                }
    //            }
    //        }
    //    }           
    //}
    [UI(UI_TYPE.Static,5)]
    public class DialogBar : UIBase
    {
        public DialogBar() { }

        public static TextAsset Dialog;
        public override void update() {
            this.Enable(false);
            base.update();
        }
        public override void Update() {
            DrawDialog();
        }

        void DrawDialog() {
            if (Dialog != null) {

            }
        }
    }
    [UI(UI_TYPE.Static,6)]
    public class ItemIntro : UIBase
    {
        public ItemIntro() { }

        public override void update() {
            this.Enable(CharacterManager.Main.Character_UI_Handler.ObjectSelect != null);
            base.update();
        }
        public override void Update() {
            DrawFrame();
        }

        public void DrawFrame() {
            if (CharacterManager.Main.Character_UI_Handler.ObjectSelect.Object is Item) {
                this.UIgraph.Instance.transform.Find("偏移层").GetComponent<RectTransform>().anchoredPosition3D = ((Item)CharacterManager.Main.Character_UI_Handler.ObjectSelect.Object).Item_UI_Handler.CenterInScreen;
                this.UIgraph.Instance.transform.Find("偏移层").Find("主框架").Find("图像").GetComponent<Image>().sprite = ((Item)CharacterManager.Main.Character_UI_Handler.ObjectSelect.Object).Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;

                this.UIgraph.Instance.transform.Find("偏移层").Find("主框架").Find("物品名").GetComponent<Text>().text = ((Item)CharacterManager.Main.Character_UI_Handler.ObjectSelect.Object).GetType().ToString();
                this.UIgraph.Instance.transform.Find("偏移层").Find("主框架").Find("描述").GetComponent<Text>().text = ((ItemUI)CharacterManager.Main.Character_UI_Handler.ObjectSelect.Object).GetItemIntro().GetString();
            }
        }
    }


    
}

[UI(UI_TYPE.Static,100)]
public class DebugWindow : UIBase
{
    private static StringBuilder stringBuilder = new StringBuilder();
    private Text Text;
    private static int UpdateCount = 0;
    private const int CountDisplay = 10;
    public static void ItemContainInfo(Item item) {
        if (UpdateCount == CountDisplay) {
            stringBuilder.Append(item.GetType().ToString());
            stringBuilder.Append('\n');
            if (item.IsContainer && item != Items.Empty) {
                for (int i = 0; i < item.GetContainerState().size; i++) {
                    ItemContainInfo(item.GetContainerState().Contents[i]);
                }
            }
        }
    }


  
    public static void DebugInfo(string Info) {
        if (UpdateCount == CountDisplay) {
            stringBuilder.Append(Info);
            stringBuilder.Append('\n');
        }
    }
    public DebugWindow() { }

    public override void Awake() {
        this.Text = this.UIgraph.Instance.transform.Find("Text").GetComponent<Text>();
    }
    public override void update() {
        this.Enable(SystemStateManager.systemState == SystemState.Debug);
        base.update();
    }
    public override void Update() {
        if (UpdateCount == CountDisplay) {
            WaitManager.WaitToEndOfFrame();
            Text.text = stringBuilder.ToString();
            stringBuilder.Clear();
            UpdateCount = 0;
        }
        else {
            UpdateCount++;
        }
    }


}
public abstract class UIBase : UI, UIInit
{
    public int ID { get { return this.id; } }
    private int id = -1;
    public int ParID = -1;
    public List<UI> SonNode = new List<UI>();
    public UISetting UISetting = new UISetting();
    public UIGraph UIgraph;

    public UIBase() { }

    public void Enable(bool index) {
        this.UIgraph.enable(index);//临时的
    }
    public virtual void Awake() { }
    public virtual void update() { if (this.UIgraph.enabled) { this.Update(); foreach (UI uis in SonNode) { uis.update(); } } }
    public virtual void Update() { }
    bool UI.disable(LinkedListNode<UI> thisnode, out LinkedListNode<UI> NEXT) {
        if (!this.UIgraph.enabled) {
            if (thisnode.Next != null) {
                NEXT = thisnode.Next;
            }
            else {
                NEXT = null;
            }
            UIManager.UIEnable.Remove(thisnode);
            return true;
        }
        NEXT = null;
        return false;
    }
    List<UI> UI.GetSonNode() {
        return this.SonNode;
    }
    void UIInit.Kernel_Init(int ID, UI_TYPE uI_TYPE, int ParID) {
        this.id = ID;
        this.UIgraph=new UIGraph();// = new UIGraph(ID);
        this.UISetting.UI_TYPE = uI_TYPE;
        this.ParID = ParID;
        this.UIgraph.Kernel_Init(ID);
    }
    public virtual void start() {
        if (this.UISetting.UI_TYPE == UI_TYPE.Static) {
            UIManager.StaticUI.Add(this);
        }
        else if(this.UISetting.UI_TYPE==UI_TYPE.Dynamics) {
            UIManager.UIEnable.AddLast(this);
        }
        else if (this.UISetting.UI_TYPE == UI_TYPE.StaticSon){
            if (this.ParID == -1) {
                Debug.LogError("UI未设置UI_Par");
            }
            UIManager.GetStaticUIAttach(this.ParID, this);
        }
    }

}
public enum UI_TYPE
{
    Static,
    Dynamics,
    StaticSon,
}
public class UISetting
{
    public UI_TYPE UI_TYPE = UI_TYPE.Dynamics;
}
public class UIGraph
{
    public bool enabled = true;
    private int Mark;
    private int ID;
    public GameObject Instance;

    public UIGraph(int ID) {
        this.ID = ID;
        this.Mark = StaticPath.UIPool.__UsePoolByID(ID);
        if (this.Mark == -1) {
            Debug.LogError("UI类的ID声明无法在UI池中找到对应/或者未在UIS Init中添加新类");
        }
        Instance = StaticPath.UIPool._GetGameObjectRef(ID, Mark);
        Instance.SetActive(true);
    }
    public UIGraph() { }
    public void Kernel_Init(int ID) {
        this.ID = ID;
        this.Mark = StaticPath.UIPool.__UsePoolByID(ID);
        if (this.Mark == -1) {
            Debug.LogError("UI类的ID声明无法在UI池中找到对应/或者未在UIS Init中添加新类");
        }
        Instance = StaticPath.UIPool._GetGameObjectRef(ID, Mark);
        Instance.SetActive(true);
    }

    public void enable(bool index) {
        this.Instance.SetActive(index);
        this.enabled = index;
    }
}
public interface UIInit {
    void Kernel_Init(int ID, UI_TYPE uI_TYPE, int ParID);
}
public interface UI
{
    int ID { get; }
    void Enable(bool index);
    void Awake();
    void update();
    void start();
    bool disable(LinkedListNode<UI> thisnode, out LinkedListNode<UI> NEXT);
    List<UI> GetSonNode();
    
}
