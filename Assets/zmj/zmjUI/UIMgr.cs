using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour {

	public GameObject mainUI;
	public GameObject secUI;
	public GameObject sbczUI;

	static public UIMgr mgr;
	public TouchPadChangUI m_ui;
	// Use this for initialization
	void Start () {
		mgr = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
}
