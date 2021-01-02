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
    public class ToolBar : UIStatic
    {
        public ToolBar() { }

        public override void update() {
            this.Enable(true);
            base.update();
        }
        public override void Update() {
            DrawBagImages();
        }

        void DrawBagImages() {
            string SonName = "Item";
            for (int i = 0; i < 7; i++) {
                this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Frame").gameObject.SetActive(PlayerManager.Main.HeldOrder == i);
                this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Num").gameObject.SetActive(((ItemUI)((Container)PlayerManager.Main.GetStaticBag()).GetContainerState().Contents[i]).Displaycount());
                this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Num").GetComponent<Text>().text = ((ItemUI)((Container)PlayerManager.Main.GetStaticBag()).GetContainerState().Contents[i]).GetUIheld().ToString();
                if (((Container)PlayerManager.Main.GetStaticBag()).GetContainerState().Contents[i] != null) {
                    if (((Container)PlayerManager.Main.GetStaticBag()).GetContainerState().Contents[i].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid == null) {
                        this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Image").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                    else {
                        this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Image").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        if (((Container)PlayerManager.Main.GetStaticBag()).GetContainerState().Contents[i].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid != null) {
                            this.UIgraph.Instance.transform.Find("工具栏").Find(SonName + (i + 1).ToString()).Find("Image").GetComponent<Image>().sprite = ((Container)PlayerManager.Main.GetStaticBag()).GetContainerState().Contents[i].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;
                        }
                    }
                }
            }
        }
    }
    [UI(UI_TYPE.StaticSon,1,0)]
    public class VITBar : UIStatic
    {
        public VITBar() { }

        public override void update() {
            this.Enable(true);
            base.update();
        }
        public override void Update() {
            DrawVIT();
        }

        void DrawVIT() {
            this.UIgraph.Instance.transform.Find("体力条").Find("Image").GetComponent<Image>().fillAmount = (int)(((ValuePlayer)PlayerManager.Main).GetVITrate() * 50) / 50f;
            if (((UIPlayer)PlayerManager.Main).GetRunable()) {
                this.UIgraph.Instance.transform.Find("体力条").Find("Image").GetComponent<Image>().color = new Color(7 / 255, 255 / 255, 0);
            }
            else if (!((UIPlayer)PlayerManager.Main).GetRunable()) {
                this.UIgraph.Instance.transform.Find("体力条").Find("Image").GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 0);
            }
        }
    }
    [UI(UI_TYPE.StaticSon,2,0)]
    public class HPBar : UIStatic
    {
        public HPBar() { }

        public override void update() {
            this.Enable(true);
            base.update();
        }
        public override void Update() {
            DrawHP();
        }

        void DrawHP() {
            this.UIgraph.Instance.transform.Find("血条").Find("Image").GetComponent<Image>().fillAmount = (int)(((ValuePlayer)PlayerManager.Main).GetHPrate() * 50) / 50f;
        }
    }
    [UI(UI_TYPE.StaticSon,3,0)]
    public class GunBar : UIStatic
    {
        public GunBar() { }

        public override void update() {
            this.Enable(PlayerManager.MainPlayerHeldTypeBool(ItemType.Gun));
            base.update();
        }
        public override void Update() {
            this.DrawMagazine();
        }
        void DrawMagazine() {
            List<Item> Magazines = ((Gun)PlayerManager.Main.Held).FindMarchMagazine();
            if (Magazines != null) {
                int Count = Magazines.Count >= 2 ? 2 : Magazines.Count;
                for (int i = 1; i < 3; i++) {
                    this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).gameObject.SetActive(i <= Count);
                }
                for (int i = 0; i <= Count; i++) {
                    if (i == 0) {
                        if (((Gun)PlayerManager.Main.Held).magazine != Items.Empty) {
                            this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").gameObject.SetActive(true);
                            this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").GetComponent<Image>().sprite = ((Gun)PlayerManager.Main.Held).magazine.Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;
                            this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").Find("Rate").GetComponent<Image>().fillAmount = ((UIMagazine)((Gun)PlayerManager.Main.Held).magazine).GetBulletRate();
                        }
                        else {
                            this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").gameObject.SetActive(false);
                        }
                    }
                    else {
                        this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").GetComponent<Image>().sprite = Magazines[i - 1].Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;
                        this.UIgraph.Instance.transform.Find("弹夹条").Find((i + 1).ToString()).Find("Image").Find("Rate").GetComponent<Image>().fillAmount = ((UIMagazine)Magazines[i - 1]).GetBulletRate();
                    }
                }
            }
        }           
        


    }
    [UI(UI_TYPE.StaticSon,4,0)]
    public class SkillBar : UIStatic
    {
        public SkillBar() { }

        public override void update() {
            this.Enable(true);
            base.update();
        }
        public override void Update() {
            DrawSkill();
        }

        void DrawSkill() {
            //this.UIgraph.Instance.transform.Find("血条").Find("Image").GetComponent<Image>().fillAmount = (int)(((ValuePlayer)PlayerManager.Main).GetHPrate() * 50) / 50f;
        }
    }
    [UI(UI_TYPE.Static,5)]
    public class DialogBar : UIStatic
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
    public class ItemIntro : UIStatic
    {
        public ItemIntro() { }

        public override void update() {
            this.Enable(PlayerManager.Main.PlayerBoolLock.Origin != null);
            base.update();
        }
        public override void Update() {
            DrawFrame();
        }

        public void DrawFrame() {
            this.UIgraph.Instance.transform.Find("偏移层").GetComponent<RectTransform>().anchoredPosition3D = PlayerManager.Main.PlayerBoolLock.Origin.itemOntheGround.Item_UI_Handler.CenterInScreen;
            this.UIgraph.Instance.transform.Find("偏移层").Find("主框架").Find("图像").GetComponent<Image>().sprite = PlayerManager.Main.PlayerBoolLock.Origin.itemOntheGround.Item_UI_Handler.Graph.StaticGraphs_Sprite.UI_Ingrid;

            this.UIgraph.Instance.transform.Find("偏移层").Find("主框架").Find("物品名").GetComponent<Text>().text = PlayerManager.Main.PlayerBoolLock.Origin.itemOntheGround.GetType().ToString();
            this.UIgraph.Instance.transform.Find("偏移层").Find("主框架").Find("描述").GetComponent<Text>().text = ((ItemUI)PlayerManager.Main.PlayerBoolLock.Origin.itemOntheGround).GetItemIntro().GetString();
        }
    }


    
}

[UI(UI_TYPE.Static,100)]
public class DebugWindow : UIStatic
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

    public static void InputInterfaceInfo(InputInterface inputInterface) {
        if (UpdateCount == CountDisplay) {
            stringBuilder.Append("Input.Use1:" + inputInterface.InputState.Use1.ToString() + '\n');
            stringBuilder.Append("Input.Use2:" + inputInterface.InputState.Use2.ToString() + '\n');
            stringBuilder.Append("Input.Use3:" + inputInterface.InputState.Use3.ToString() + '\n');
            stringBuilder.Append("Input.Use4:" + inputInterface.InputState.Use4.ToString() + '\n');
            stringBuilder.Append("Input.Use5:" + inputInterface.InputState.Use5.ToString() + '\n');
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
public abstract class UIStatic : UI, UIInit
{
    public int ID { get { return this.id; } }
    private int id = -1;
    public int ParID = -1;
    public List<UI> SonNode = new List<UI>();
    public UISetting UISetting = new UISetting();
    public UIGraph UIgraph;

    public UIStatic() { }

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
