using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class ZhengScripts : MonoBehaviour
{
    public static XmlDocument doc = null;
    public static bool max = true, ssmallest = true, smallest = true, skateboard = true;
    public static bool initialize = false;
    /*
    Max3 
    */
    // "UpArmSleeve", "UpArmBar", "Max1", "Max2", "Max3", "MMove", "SSmallest1", "SSmallest2", "SSmallest3", "SSMove", "Smallest1", "Smallest2", "Smallest3", "SMove"
    static bool inverse = true;
    public static void BindingZhengMachineMAX(string Max1name, string Max2name, string Max3name, string MMovename)
    {
        GameObject Max1 = GameObject.Find(Max1name);
        GameObject Max2 = GameObject.Find(Max2name);
        GameObject Max3 = GameObject.Find(Max3name);
        GameObject MMove = GameObject.Find(MMovename);
        if (Max1 == null || Max2 == null || Max3 == null || MMove == null)
            return;
        if (max && initialize)
        {
            max = false;
            AddXML("Max1", Max1.transform.localPosition.ToString("F10"), Max1.transform.localEulerAngles.ToString("F10"), null);
            AddXML("Max2", Max2.transform.localPosition.ToString("F10"), Max2.transform.localEulerAngles.ToString("F10"), null);
            AddXML("Max3", Max3.transform.localPosition.ToString("F10"), Max3.transform.localEulerAngles.ToString("F10"), null);
            AddXML("MMove", MMove.transform.localPosition.ToString("F10"), MMove.transform.localEulerAngles.ToString("F10"), null);
        }
        else
        {
            if (doc == null)
                AddXML();
            XmlNode root = doc.SelectSingleNode("IniGameTrans").SelectSingleNode("IniGameTransAll");
            Initilize((XmlElement)root.SelectSingleNode("Max1"), Max1);
            Initilize((XmlElement)root.SelectSingleNode("Max2"), Max2);
            Initilize((XmlElement)root.SelectSingleNode("Max3"), Max3);
            Initilize((XmlElement)root.SelectSingleNode("MMove"), MMove);
        }
        MechanMetaComponent Max1_MMove = new MechanMetaComponent(Max1.transform, MMove.transform);
        Max1_MMove.SetFixedPosition(true);
        MechanMetaComponent MMove_Max1 = new MechanMetaComponent(MMove.transform, Max1.transform);
        MMove_Max1.SetFixedPosition(true);
        MechanMetaComponent Max3_Max2 = new MechanMetaComponent(Max3.transform, Max2.transform);
        Max3_Max2.SetFixedLength(true);

        Machine.singleton.AddMecBindings(Max1name + "_" + MMovename/*"Max1_MMove"*/, new MechanMetaComponentBinding(Max1_MMove));
        Machine.singleton.AddMecBindings(MMovename + "_" + Max1name /*"MMove_Max1"*/, new MechanMetaComponentBinding(MMove_Max1));
        Machine.singleton.AddMecBindings(Max3name + "_" + Max2name/*"Max3_Max2"*/, new MechanMetaComponentBinding(Max3_Max2));
    }

	public static void Initilize(int index){
		GameObject Max1 = GameObject.Find ("Max1" + index);
		GameObject Max2 = GameObject.Find ("Max2" + index);
		GameObject Max3 = GameObject.Find ("Max3" + index);
		GameObject MMove = GameObject.Find ("MMove" + index);
		GameObject SSmallest1 = GameObject.Find ("SSmallest1" + index);
		GameObject SSmallest2 = GameObject.Find ("SSmallest2" + index);
		GameObject SSmallest3 = GameObject.Find ("SSmallest3" + index);
		GameObject SSMove = GameObject.Find ("SSMove" + index);
		GameObject Smallest1 = GameObject.Find ("Smallest1" + index);
		GameObject Smallest2 = GameObject.Find ("Smallest2" + index);
		GameObject Smallest3 = GameObject.Find ("Smallest3" + index);
		GameObject SMove = GameObject.Find ("SMove" + index);
		if (doc == null)
			AddXML ();
		XmlNode root = doc.SelectSingleNode ("IniGameTrans").SelectSingleNode ("IniGameTransAll");
		Initilize ((XmlElement)root.SelectSingleNode ("Max1"), Max1);
		Initilize ((XmlElement)root.SelectSingleNode ("Max2"), Max2);
		Initilize ((XmlElement)root.SelectSingleNode ("Max3"), Max3);
		Initilize ((XmlElement)root.SelectSingleNode ("MMove"), MMove);
		Initilize ((XmlElement)root.SelectSingleNode ("SSmallest1"), SSmallest1);
		Initilize ((XmlElement)root.SelectSingleNode ("SSmallest2"), SSmallest2);
		Initilize ((XmlElement)root.SelectSingleNode ("SSmallest3"), SSmallest3);
		Initilize ((XmlElement)root.SelectSingleNode ("SSMove"), SSMove);
		Initilize ((XmlElement)root.SelectSingleNode ("Smallest1"), Smallest1);
		Initilize ((XmlElement)root.SelectSingleNode ("Smallest2"), Smallest2);
		Initilize ((XmlElement)root.SelectSingleNode ("Smallest3"), Smallest3);
		Initilize ((XmlElement)root.SelectSingleNode ("SMove"), SMove);

	}

    public static void BindingZhengMachineSSMALLEST(string SSmallest1name, string SSmallest2name, string SSmallest3name, string SSMovename)
    {
        GameObject SSmallest1 = GameObject.Find(SSmallest1name);
        GameObject SSmallest2 = GameObject.Find(SSmallest2name);
        GameObject SSmallest3 = GameObject.Find(SSmallest3name);
        GameObject SSMove = GameObject.Find(SSMovename);
        if (SSmallest1 == null || SSmallest2 == null || SSmallest3 == null || SSMove == null)
            return;
        if (ssmallest && initialize)
        {
            ssmallest = false;
            AddXML("SSmallest1", SSmallest1.transform.localPosition.ToString("F10"), SSmallest1.transform.localEulerAngles.ToString("F10"), null);
            AddXML("SSmallest2", SSmallest2.transform.localPosition.ToString("F10"), SSmallest2.transform.localEulerAngles.ToString("F10"), null);
            AddXML("SSmallest3", SSmallest3.transform.localPosition.ToString("F10"), SSmallest3.transform.localEulerAngles.ToString("F10"), null);
            AddXML("SSMove", SSMove.transform.localPosition.ToString("F10"), SSMove.transform.localEulerAngles.ToString("F10"), null);
        }
        else
        {
            if (doc == null)
                AddXML();
            XmlNode root = doc.SelectSingleNode("IniGameTrans").SelectSingleNode("IniGameTransAll");
            Initilize((XmlElement)root.SelectSingleNode("SSmallest1"), SSmallest1);
            Initilize((XmlElement)root.SelectSingleNode("SSmallest2"), SSmallest2);
            Initilize((XmlElement)root.SelectSingleNode("SSmallest3"), SSmallest3);
            Initilize((XmlElement)root.SelectSingleNode("SSMove"), SSMove);
        }
        MechanMetaComponent SSmallest1_SSMove = new MechanMetaComponent(SSmallest1.transform, SSMove.transform);
        SSmallest1_SSMove.SetFixedPosition(true);
        MechanMetaComponent SSMove_SSmallest1 = new MechanMetaComponent(SSMove.transform, SSmallest1.transform);
        SSMove_SSmallest1.SetFixedPosition(true);
        MechanMetaComponent SSmallest3_SSmallest2 = new MechanMetaComponent(SSmallest3.transform, SSmallest2.transform);
        SSmallest3_SSmallest2.SetFixedLength(true);
        MechanMetaComponent SSmallest2_SSmallest3 = new MechanMetaComponent(SSmallest2.transform, SSmallest3.transform);

        Machine.singleton.AddMecBindings(SSmallest1name + "_" + SSMovename/*"SSmallest1_SSMove"*/, new MechanMetaComponentBinding(SSmallest1_SSMove));
        Machine.singleton.AddMecBindings(SSMovename + "_" + SSmallest1name/*"SSMove_SSmallest1"*/, new MechanMetaComponentBinding(SSMove_SSmallest1));
        Machine.singleton.AddMecBindings(SSmallest3name + "_" + SSmallest2name/*"SSmallest3_SSmallest2"*/, new MechanMetaComponentBinding(SSmallest3_SSmallest2));
        Machine.singleton.AddMecBindings(SSmallest2name + "_" + SSmallest3name/*"SSmallest2_SSmallest3"*/, new MechanMetaComponentBinding(SSmallest2_SSmallest3));
    }

    public static void BindingZhengMachineSMALLEST(string Smallest1name, string Smallest2name, string Smallest3name, string SMovename)
    {
        GameObject Smallest1 = GameObject.Find(Smallest1name);
        GameObject Smallest2 = GameObject.Find(Smallest2name);
        GameObject Smallest3 = GameObject.Find(Smallest3name);
        GameObject SMove = GameObject.Find(SMovename);
        if (Smallest1 == null || Smallest2 == null || Smallest3 == null || SMove == null)
            return;

        if (smallest && initialize)
        {
            smallest = false;
            AddXML("Smallest1", Smallest1.transform.localPosition.ToString("F10"), Smallest1.transform.localEulerAngles.ToString("F10"), null);
            AddXML("Smallest2", Smallest2.transform.localPosition.ToString("F10"), Smallest2.transform.localEulerAngles.ToString("F10"), null);
            AddXML("Smallest3", Smallest3.transform.localPosition.ToString("F10"), Smallest3.transform.localEulerAngles.ToString("F10"), null);
            AddXML("SMove", SMove.transform.localPosition.ToString("F10"), SMove.transform.localEulerAngles.ToString("F10"), null);
        }
        else
        {
            if (doc == null)
                AddXML();
            XmlNode root = doc.SelectSingleNode("IniGameTrans").SelectSingleNode("IniGameTransAll");
            Initilize((XmlElement)root.SelectSingleNode("Smallest1"), Smallest1);
            Initilize((XmlElement)root.SelectSingleNode("Smallest2"), Smallest2);
            Initilize((XmlElement)root.SelectSingleNode("Smallest3"), Smallest3);
            Initilize((XmlElement)root.SelectSingleNode("SMove"), SMove);
        }
        MechanMetaComponent Smallest1_SMove = new MechanMetaComponent(Smallest1.transform, SMove.transform);
        Smallest1_SMove.SetFixedPosition(true);
        MechanMetaComponent SMove_Smallest1 = new MechanMetaComponent(SMove.transform, Smallest1.transform);
        SMove_Smallest1.SetFixedPosition(true);
        MechanMetaComponent Smallest3_Smallest2 = new MechanMetaComponent(Smallest3.transform, Smallest2.transform);
        Smallest3_Smallest2.SetFixedLength(true);

        Machine.singleton.AddMecBindings(Smallest1name + "_" + SMovename/*"Smallest1_SMove"*/, new MechanMetaComponentBinding(Smallest1_SMove));
        Machine.singleton.AddMecBindings(SMovename + "_" + Smallest1name/*"SMove_Smallest1"*/, new MechanMetaComponentBinding(SMove_Smallest1));
        Machine.singleton.AddMecBindings(Smallest3name + "_" + Smallest2name/*"Smallest3_Smallest2"*/, new MechanMetaComponentBinding(Smallest3_Smallest2));
    }

    public static void BindingZhengMachineSkateboard(string SkateboardForwardname, string SkateboardBackname)
    {
        GameObject SkateboardForward = GameObject.Find(SkateboardForwardname);
        GameObject SkateboardBack = GameObject.Find(SkateboardBackname);

        if (SkateboardForward == null || SkateboardBack == null)
            return;

        if (skateboard && initialize)
        {
            skateboard = false;
            AddXML("Ban", SkateboardForward.transform.localPosition.ToString("F10"), SkateboardForward.transform.localEulerAngles.ToString("F10"), null);
            if (doc == null)
                AddXML();
            XmlNode root = doc.SelectSingleNode("IniGameTrans").SelectSingleNode("IniGameTransAll");
        }
        else
        {
            if (doc == null)
                AddXML();
            XmlNode root = doc.SelectSingleNode("IniGameTrans").SelectSingleNode("IniGameTransAll");
            Initilize((XmlElement)root.SelectSingleNode("Ban"), SkateboardForward);
            //Vector3 p = SkateboardForward.transform.transform.localPosition;
            //if (SkateboardForwardname == "Ban0")
            //    SkateboardForward.transform.localPosition = new Vector3(p.x, p.y, 0.972f);
        }
        MechanMetaComponent SkateboardForwardname_SkateboardBackname = new MechanMetaComponent(SkateboardForward.transform, SkateboardBack.transform);
        SkateboardForwardname_SkateboardBackname.SetFixedPosition(true);

        Machine.singleton.AddMecBindings(SkateboardForwardname + "_" + SkateboardBackname/*"Smallest1_SMove"*/, new MechanMetaComponentBinding(SkateboardForwardname_SkateboardBackname));
    }

    public static void InitilizeSketatorAnimation(float extent = 0)
    {
        for (int i = 0; i < Fuzhi.mounts; i++)
        {
            GameObject game = GameObject.Find("Sketator" + i);
            if (game == null)
                continue;
            Animation ani = game.GetComponent<Animation>();
            if (ani == null)
                continue;
            ani.Play();
            ani[ani.clip.name].speed = 0;
            ani[ani.clip.name].normalizedTime = extent;
        }
        //     ZhuAnimation.offAll = true;
    }

    public static void InitilizeZhiJiaAnimation()
	{
		Animation[] animations = GameObject.FindObjectsOfType<Animation> ();
		for (int i = 0; i < animations.Length; i++) {
			if (animations [i].gameObject.name == "BracketAll" || animations[i].gameObject.name == "UpArm") {
				try{
				animations [i].Play ();
				animations [i] [animations [i].clip.name].normalizedTime = 0;
				animations [i] [animations [i].clip.name].speed = 0;
				}catch(System.Exception e){
					Debug.LogError ("尝试播放默认动画片段为Null的动画");
				}
			}
		}
	}

    public static void InitilizeSketator(Vector2 targetindex, float[] extent)
    {
        //float ntime = 0;
        int start = (int)Mathf.Min(targetindex.x, targetindex.y);
        int end = (int)Mathf.Max(targetindex.x, targetindex.y);
        //int count = end - start + 1;
        int k = 0;
       
        for (int i = start; i <= end; i++)
        {
            GameObject game = GameObject.Find("Sketator" + i);
            if (game == null)
                continue;
            Animation ani = game.GetComponent<Animation>();
            if (ani == null)
                continue;
            ani.Play();
            ani[ani.clip.name].speed = 0;
            if (k < extent.Length)
                ani[ani.clip.name].normalizedTime = extent[k];
            else
                ani[ani.clip.name].normalizedTime = 0;
            k++;
        }
    }

    public static void InitilizeChildAnimation(Vector2 targetindex, float[] extent, string childname)
    {
        //float ntime = 0;
        int start = (int)Mathf.Min(targetindex.x, targetindex.y);
        int end = (int)Mathf.Max(targetindex.x, targetindex.y);
        //int count = end - start + 1;
        int k = 0;

        for (int i = start; i <= end; i++)
        {
            GameObject game = GameObject.Find("ZhiJia" + i);
            game = Fuzhi.FindChildAll(game, childname);
            if (game == null)
                continue;
            Animation ani = game.GetComponent<Animation>();
            if (ani == null)
                continue;
            ani.Play();
            ani[ani.clip.name].speed = 0;
            if (k < extent.Length)
                ani[ani.clip.name].normalizedTime = extent[k];
            else
                ani[ani.clip.name].normalizedTime = 0;
            k++;
        }
    }

    public static void InitilizeBracketAll(Vector2 targetindex, float[] extent)
    {
        InitilizeChildAnimation(targetindex, extent, "BracketAll");
    }

    public static void InitilizeUpArm(Vector2 targetindex, float[] extent)
    {
        InitilizeChildAnimation(targetindex, extent, "UpArm");
    }

    public static void Binding()
    {
        for (int i = 0; i < Fuzhi.mounts; i++)
        {
            BindingZhengMachineMAX("Max1" + i, "Max2" + i, "Max3" + i, "MMove" + i);
            BindingZhengMachineSSMALLEST("SSmallest1" + i, "SSmallest2" + i, "SSmallest3" + i, "SSMove" + i);
            BindingZhengMachineSMALLEST("Smallest1" + i, "Smallest2" + i, "Smallest3" + i, "SMove" + i);
            BindingZhengMachineSkateboard("Ban" + i, "Ban" + (i + 1));
        }

     //   Animation a;
        

        if (initialize)
            SaveXML();
        ZhengScripts.InitilizeSketatorAnimation();
        ZhengScripts.InitilizeZhiJiaAnimation();
        AnimationManager.singion.playing = false;
    }

    static void Initilize(XmlElement elements, GameObject game)
    {
        if (game != null)
        {
            game.transform.localPosition = PluginEvent.Common.Vector3Parse(elements.GetAttribute("Position"));
            game.transform.localEulerAngles = PluginEvent.Common.Vector3Parse(elements.GetAttribute("Roration"));
        }
    }

    public static void PlayAnimation(Vector3 time, Vector3 step, Vector3 duration, int startindex, bool order, int triggerinverse, int inversestart, int endindex, int inverseend, int remberstartindex, bool attach = false)
    {

        if (startindex <50 /*Fuzhi.mounts*/ && startindex >= 0)
            Machine.singleton.MecBindings["Smallest3" + startindex + "_" + "Smallest2" + startindex].PlayAnimatin(time.x, step.x).AnimationAction(() =>
            {
                Machine.singleton.MecBindings["SSmallest3" + startindex + "_" + "SSmallest2" + startindex].PlayAnimatin(time.y, step.y).AnimationAction(() =>
                {
                    MechanMetaComponentBinding binding = Machine.singleton.MecBindings["Max3" + startindex + "_" + "Max2" + startindex].PlayAnimatin(time.z, step.z);
                    if (attach)
                    {
                        print(startindex + "   " + inverseend);
                        if (startindex == inverseend)
                        {
                            binding.AnimationAction(() =>
                            {
                                PlayAnimation(time, -step, duration, inverseend, !order, inverseend, endindex, inversestart, remberstartindex, inverseend);
                                inverse = true;
                                MonoBehaviour.print(inverseend + " " + !order + "  " + inverseend + "  " + endindex + "  " + inversestart + "  " + remberstartindex + "  " + inverseend);
                            }
                            );
                            return;
                        }
                    }
                    binding.AnimationAction(() =>
                    {
                        if (startindex != endindex)
                        {
                            if (startindex == triggerinverse && inverse)//待定
                            {
                                inverse = false;
                                PlayAnimation(time, -step, duration, inversestart, order, triggerinverse, inversestart, endindex, inverseend, remberstartindex, true);
                            }
                            if (order)
                                startindex++;
                            else
                                startindex--;
                            PlayAnimation(time, step, duration, startindex, order, triggerinverse, inversestart, endindex, inverseend, remberstartindex, attach);
                        }
                    }, duration.z);
                }, duration.y);
            }, duration.x);
        else
            MonoBehaviour.print("xsadsadsa");
    }

    public static void SetLenght(Vector2 index, Vector3 step)
    {
        int min = (int)Mathf.Min(index.x, index.y);
        int max = (int)Mathf.Max(index.x, index.y);
        if ((max > Fuzhi.mounts || max < 0) && max != -1)
            return;
        if (max != -1)
            for (int i = min; i <= max; i++)
            {
                Machine.singleton.MecBindings["Smallest3" + i + "_" + "Smallest2" + i].SetLenght(step.x);
                Machine.singleton.MecBindings["SSmallest3" + i + "_" + "SSmallest2" + i].SetLenght(step.y);
                Machine.singleton.MecBindings["Max3" + i + "_" + "Max2" + i].SetLenght(step.z);
            }
        else
        {
            try
            {
                Machine.singleton.MecBindings["Smallest3_Smallest2"].SetLenght(step.x);
                Machine.singleton.MecBindings["SSmallest3_SSmallest2"].SetLenght(step.y);
                Machine.singleton.MecBindings["Max3_Max2"].SetLenght(step.z);
            }
            catch (System.Exception e)
            {
                Debug.LogError("没有发现节点Smallest3_Smallest2、SSmallest3_SSmallest2、Max3_Max2");
            }
        }
    }

    public static void StopCurrentAnimation()
    {
        for (int i = 0; i < Fuzhi.mounts; i++)
        {
            Machine.singleton.MecBindings["Smallest3" + i + "_" + "Smallest2" + i].Stop();
            Machine.singleton.MecBindings["SSmallest3" + i + "_" + "SSmallest2" + i].Stop();
            Machine.singleton.MecBindings["Max3" + i + "_" + "Max2" + i].Stop();
        }
    }

    public static void PauseCurrentAnimation()
    {
        for (int i = 0; i < Fuzhi.mounts; i++)
        {
            PauseCurrentAnimation(i);
            print(i + "   hhhh");
        }
    }

    public static void PauseCurrentAnimation(int index)
    {
        Machine.singleton.MecBindings["Smallest3" + index + "_" + "Smallest2" + index].Pause();
        Machine.singleton.MecBindings["SSmallest3" + index + "_" + "SSmallest2" + index].Pause();
        Machine.singleton.MecBindings["Max3" + index + "_" + "Max2" + index].Pause();
    }

    public static void PlayCurrentAnimation()
    {
        for (int i = 0; i < Fuzhi.mounts; i++)
            PlayCurrentAnimation(i);
    }

    public static void PlayCurrentAnimation(int index)
    {
        Machine.singleton.MecBindings["Smallest3" + index + "_" + "Smallest2" + index].PlayAnimatin();
        Machine.singleton.MecBindings["SSmallest3" + index + "_" + "SSmallest2" + index].PlayAnimatin();
        Machine.singleton.MecBindings["Max3" + index + "_" + "Max2" + index].PlayAnimatin();
    }

    public static void AddXML(string name, string position, string roration, string path)
    {
        XmlNode root;
        XmlNode node;
        if (doc == null)
        {
            doc = new XmlDocument();

            try
            {
                doc.Load(path);
            }
            catch (Exception e)
            {
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(dec);
            }
            root = doc.CreateElement("IniGameTrans");
            doc.AppendChild(root);
            node = doc.CreateElement("IniGameTransAll");
            root.AppendChild(node);
        }

        root = doc.SelectSingleNode("IniGameTrans");
        doc.AppendChild(root);

        node = root.SelectSingleNode("IniGameTransAll");
        root.AppendChild(node);

        XmlElement element1 = doc.CreateElement(name);
        element1.SetAttribute("Position", position);
        element1.SetAttribute("Roration", roration);
        node.AppendChild(element1);
    }

    public static void AddXML()
    {
        if (doc == null)
        {
            doc = new XmlDocument();
            doc.Load(PluginEvent.Common.WindowsPath(TPResourceManager.GetFilePath("ZMJIni.xml")));
        }
    }

    public static void SaveXML()
    {
        if (doc != null)
            doc.Save(@"d:\ZMJIni.xml");
    }

    //XmlNode xn = xmlDoc.SelectSingleNode("bookstore");
    // 1: XmlDocument doc = new XmlDocument();
    //2: doc.Load(@"..\..\Book.xml");
}
