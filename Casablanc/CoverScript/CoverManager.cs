using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : SingletonMono<CoverManager>
{
    /// <summary>
    /// 哈希生成机
    /// </summary>
    private static HashGenerator hashGenerator = new HashGenerator();

    /// <summary>
    /// 静态生成部分
    /// </summary>
    private static List<CoverGroup> coverGroups = new List<CoverGroup>();
    private static Dictionary<string, CoverGroup> CG = new Dictionary<string, CoverGroup>();

    /// <summary>
    /// 动态的分配过程
    /// </summary>
    private static Dictionary<int, LinkedListNode<CoverGroup>> Target = new Dictionary<int, LinkedListNode<CoverGroup>>();
    private static LinkedList<CoverGroup> CoversInCamera = new LinkedList<CoverGroup>();
    private static LinkedListNode<CoverGroup> tmp;

    public static void EnterGroup(GameObject gameObject) {
        if (CG.TryGetValue(gameObject.GetComponent<CoverComponent>().Groupname, out CoverGroup coverGroup)) {
            coverGroup.Enter(gameObject);
            gameObject.GetComponent<CoverComponent>().CoverGroup = coverGroup;
        }
        else {
            CoverGroup tmp = new CoverGroup();
            tmp.Enter(gameObject);
            gameObject.GetComponent<CoverComponent>().CoverGroup = tmp;
            CG.Add(gameObject.GetComponent<CoverComponent>().Groupname, tmp);
        }

    }
    public static void Enter(CoverGroup coverGroup) {
        if (coverGroup.coverRuntimePack.hash == -1) {
            coverGroup.coverRuntimePack.hash = hashGenerator.GetHash();
            CoversInCamera.AddLast(coverGroup);
            Target.Add(coverGroup.coverRuntimePack.hash, CoversInCamera.Last);
        }
        coverGroup.coverRuntimePack.count++;
    }
    public static void Quit(CoverGroup coverGroup) {
        coverGroup.coverRuntimePack.count--;
        if (coverGroup.coverRuntimePack.count == 0) {
            CoversInCamera.Remove(Target[coverGroup.coverRuntimePack.hash].Value);
            Target.Remove(coverGroup.coverRuntimePack.hash);
            hashGenerator.DisHash(coverGroup.coverRuntimePack.hash);
            coverGroup.coverRuntimePack.hash = -1;
        }
    }

    public static void Enable(string groupname) {
        CG[groupname].CoverOn((GameObject) => { return true; });
    }
    public static void Disable(string groupname) {
        CG[groupname].CoverOff((gameObject) => { return true; });
    }


    
}
