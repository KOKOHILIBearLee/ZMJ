using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchPadChangUI : MonoBehaviour {
    public struct UIPair
    {
        public GameObject normal;
        public GameObject action;

        public void ActionState()
        {
            normal.SetActive(false);
            action.SetActive(true);
        }

        public void NormalState()
        {
            normal.SetActive(true);
            action.SetActive(false);
        }
    }



	//ui列表
	public List<UIPair> m_uiList = new List<UIPair>();
	public List<Vector2> m_UIangel = new List<Vector2>();
	public List<GameObject> ui = new List<GameObject>();

	public delegate void PressAction();
	public List<PressAction> m_eventList = new List<PressAction> ();


	// Use this for initialization
	void Start () {
		if(ui.Count > 1)
		{
			for(int i = 0; i < ui.Count; )
			{
				UIPair p = new UIPair();
				p.normal = ui[i];
				p.action = ui[i+1];
				p.NormalState();
				m_uiList.Add(p);

				i += 2;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeUI(uint angle)
    {
        if (SOManager.singleton.currSceneName == "Demo001")
        {
            bool bChange = false;

            for (int i = 0; i < m_UIangel.Count; i++)
            {
                try
                {
                    Vector2 rang = m_UIangel[i];
                    m_uiList[i].NormalState();
                    if (angle > rang.x && angle < rang.y)
                    {
                        m_uiList[i].ActionState();
                        bChange = true;
                    }
                }
                catch { }
            }

            if (!bChange)
                NormalState();
        }
    }

	public void OnTouchPadPress(uint angle)
	{
        if (SOManager.singleton.currSceneName == "Demo001")
        {
            for (int i = 0; i < m_UIangel.Count; i++)
            {
                Vector2 rang = m_UIangel[i];
                try
                {
                    m_uiList[i].NormalState();
                }
                catch { }
                if (angle > rang.x && angle < rang.y && i < m_eventList.Count)
                {
                    m_eventList[i].Invoke();
                }
            }
        }
    }

	public void NormalState()
	{
		foreach(UIPair p in m_uiList)
		{
            try
            {
                p.NormalState();
            }
            catch { }
		}
	}
}
