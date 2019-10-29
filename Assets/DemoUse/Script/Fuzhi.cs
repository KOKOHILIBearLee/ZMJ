using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Fuzhi : MonoBehaviour
{
    public Vector3 vec;
    public static string[] names = new string[] { "UpArmSleeve", "UpArmBar", "Max1", "Max2", "Max3", "MMove", "SSmallest1", "SSmallest2", "SSmallest3", "SSMove", "Smallest1", "Smallest2", "Smallest3", "SMove", "Sketator", "Zhu", "Ban", "SLKEY" };
    public static int mounts = 54;
#if UNITY_EDITOR
    [MenuItem("Tool/Copy")]
#endif
    public static void dosomething()
    {
        //Max1
        //Max2
        //Max3
        //MMove
        //SSmallest1
        //SSmallest2
        //SSmallest3
        //SSMove
        //Smallest1
        //Smallest2
        //Smallest3
        //SMove
       
        GameObject gamefather = new GameObject("Father");
        GameObject game = GameObject.Find("SSLL"/*"ZhiJia"*/);
        GameObject gameo = null;
        GameObject child;
        for (int i = 0; i < mounts*6; i++)
        {
            gameo = Object.Instantiate<GameObject>(game);
            gameo.name = "SSLL" + i;
            gameo.transform.parent = gamefather.transform;

            //for (int k = 0; k < names.Length; k++)
            //{
            //    child = FindChildAll(gameo, names[k]);
            //    if (child != null)
            //        child.name = names[k] + i;
            //}
        }
    }

    public static GameObject FindChildAll(GameObject father, string childname)
    {
        Transform resulttrans = father.transform.Find(childname);
        GameObject result = null;
        if (resulttrans != null)
            result = resulttrans.gameObject;
        if (father.transform.childCount == 0 || result != null)
            return result;
        foreach (Transform trans in father.transform)
        {
            result = FindChildAll(trans.gameObject, childname);
            if (result != null)
                return result;
        }
        return null;
    }
}
