using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class MakeAnimation : MonoBehaviour
{
    public GameObject exsample;
    List<Vector3> posList = new List<Vector3>();
    public List<GameObject> handKey = new List<GameObject>();
    public bool MeiSL = false;
    #region ByTime
    public float moveSpeed;
    public float createSpeed;
    #endregion

    #region ByFrame
    public int createSpeedByFrame;
    int currentFrame = 0;
	Plane pl;
	public GameObject game;
    #endregion

    public string elementName = null;

    List<GameObject> objs = new List<GameObject>();
    Vector3 Normal;
    //
    //public Vector2 index;
    //public float vec;
    //public float time;
    //List<float> starttime;
    //public float distancetime;
    //int playindex;
    //public Vector3 dir;
    //public bool isplaying = false;

    public enum AnimationState
    {
        play,
        stop
    }

    public enum AnimationMode
    {
        ByTime,
        ByFrame,
    }

    public AnimationState state = AnimationState.stop;
    public AnimationMode mode = AnimationMode.ByTime;


	public class Pair
    {
        public int id;
        public GameObject obj;
        public Pair(int i, GameObject ob)
        {
            id = i;
            obj = ob;
        }
    }

    List<Pair> objList = new List<Pair>();
    float tCreat;
    float tSpeed;
    List<int> removeList = new List<int>();
    List<int> incrementList = new List<int>();

    GameObject parentGoMi;
    // Use this for initialization
    void Start()
    {
        tCreat = 0;
        tSpeed = 0;
        if (MeiSL)
        {
            for (int i = 0; i < Fuzhi.mounts; i++)
            {
                GameObject game = GameObject.Find("SLKEY" + i);
                if (game != null)
                    objs.Add(game);
            }
        }

        for (int i = 0; i < handKey.Count; i++)
            if (handKey[i] != null)
                objs.Add(handKey[i]);

        if (objs.Count > 1)
            Normal = objs[1].transform.position - objs[0].transform.position;
        else
            Normal = Vector3.zero;
        foreach (GameObject ob in objs)
        {
            posList.Add(ob.transform.position);
        }
        if (handKey.Count == 0)
            Initilize();
        if (objs.Count > 1)
        {
            try
            {
                pl = new Plane(objs[0].transform.position - objs[1].transform.position, game.transform.position);
            }
            catch (System.Exception e)
            {
            }
        }

        parentGoMi = GameObject.Find("obj_Parent");
    }

    //void putforward()
    //{
    //    if (isplaying)
    //    {
    //        for (int i = 0; i < playindex; i++)
    //        {
    //            playforward(i);
    //        }
    //        if (Time.time - starttime[playindex] < distancetime)
    //        {
    //            playindex++;
    //            starttime.Add(Time.time);
    //        }
    //    }
    //}

    //void playforward(int sindex)
    //{
    //    if (isplaying)
    //    {
    //        float vecl = vec * Time.deltaTime;
    //        GameObject gametem = objs[sindex];
    //        if (Time.time - starttime[sindex] < time)
    //        {
    //            gametem.transform.position = gametem.transform.position + dir * vecl;
    //        }
    //        else
    //        {

    //        }
    //    }
    //}
    // Update is called once per frame
	void UpdateByFrame(){
		if (mode == AnimationMode.ByFrame) {

			if (currentFrame > createSpeedByFrame) {
				CreateObjByFrame ();
				currentFrame = 0;
			}
			currentFrame++;
		}
	}

    void Update()
    {
		//print (this.gameObject.name);
        if (state == AnimationState.play && posList.Count > 1)
        {
            //putforward();
            if (mode == AnimationMode.ByTime)
            {
                tCreat += Time.deltaTime;
                if (tCreat > createSpeed)
                {
                    CreateObj();
                    tCreat = 0;
                }
            }
			//UpdateByFrame ();
       /*     if (mode == AnimationMode.ByFrame)
            {

                if (currentFrame > createSpeedByFrame)
                {
                    CreateObjByFrame();
                    currentFrame = 0;
                }
                currentFrame++;
            }*/
            UpdateObjPos();
            DestroyObj();
            //if (mode == AnimationMode.ByFrame)
            //{
            //    if (currentFrame > createSpeedByFrame)
            //    {

            //        createSpeedByFrame = 0;
            //    }
            //    currentFrame++;
            //}
        }
        if (state == AnimationState.stop)
        {
            while (objList.Count > 0)
            {
                try
                {
                    Destroy(objList[0].obj);
                    objList.RemoveAt(0);
                }
                catch (System.Exception e) { }
            }
            objList = new List<Pair>();
            removeList = new List<int>();
            incrementList = new List<int>();
        }
    }

    void CreateObjByFrame(bool dbfe = false)
    {
        if (exsample)
        {
            GameObject obj = null;
            if (objList.Count > 0)
            {
                float distance = moveSpeed * createSpeedByFrame;
                Transform trans = objList[objList.Count - 1].obj.transform;
                float origin = (objs[0].transform.position - objs[1].transform.position).magnitude;
                float enter = (objs[1].transform.position - trans.position).magnitude;
                Vector3 posi;
                //if (enter + distance < origin)
                // {
                obj = GameObject.Instantiate(exsample);
                obj.name = "obj_clone";
                posi = new Ray(objs[1].transform.position, objs[0].transform.position - objs[1].transform.position).GetPoint(enter + distance);
                obj.transform.position = posi;
                // }
            }
            else
            {
                obj = GameObject.Instantiate(exsample);
                obj.name = "obj_clone";
                obj.transform.position = objs[0].transform.position;

            }

            if (obj != null && objs.Count > 2)
            {
                Pair pair = new Pair(1, obj);
                if (Vector3.Distance(obj.transform.position, objs[1].transform.position /*posList[objList[i].id]*/) < (dbfe ? moveSpeed * createSpeedByFrame : moveSpeed))
                {
                    obj.transform.forward = objs[2].transform.position - objs[1].transform.position/*posList[1] - posList[0]*/;
                    pair.id += 1;
                }
                else
                    obj.transform.forward = objs[1].transform.position - objs[0].transform.position;
                objList.Add(pair);
            }
        }
    }

    void CreateObj()
    {
        if (exsample)
        {
            GameObject obj = GameObject.Instantiate(exsample);
            obj.name = "obj_clone";
            
            if (parentGoMi != null)
            {
                obj.transform.parent = parentGoMi.transform;
            }
            //			obj.transform.parent = rootObj.transform;
            obj.transform.forward = objs[1].transform.position - objs[0].transform.position/*posList[1] - posList[0]*/;
            obj.transform.position = objs[0].transform.position /*posList[0]*/;
            Pair pair = new Pair(1, obj);
            objList.Add(pair);
        }
    }

    void Initilize()
    {
        //for (int i = 0; i < Fuzhi.mounts * 5; i++)
        //{
        //    CreateObjByFrame(true);
        //    UpdateObjPos(true);
        //}

        int ladder = 0;
        if (elementName == null || elementName == "")
            return;
        for (int i = 0; i < Fuzhi.mounts * 6; i++)
        {
            GameObject game = GameObject.Find(elementName + i);
            if (game == null)
                continue;
            Vector3 gameposition = game.transform.position;
            for (int k = ladder; k < objs.Count - 1; k++)
            {
                Vector3 position1 = objs[k].transform.position;
                Vector3 position2 = objs[k + 1].transform.position;

                if (Vector3.Dot(position1 - gameposition, position2 - gameposition) < 0.1)
                {
                    Pair pair = new Pair(k + 1, game);
                    objList.Add(pair);
                    ladder = k;
                    break;
                }
            }
        }
        List<Pair> list = new List<Pair>();
        for (int i = objList.Count - 1; i >= 0; i--)
            list.Add(objList[i]);
        objList = list;

    }

    //void UpdateByFrame()
    //{
    //    if (objList.Count >= objs.Count)
    //        objList.RemoveAt(0);
    //    for (int i = 1; i < objList.Count; i++)
    //    {
    //        Vector3 vec = objList[i - 1].obj.transform.position;
    //        objList[i].obj.transform.position = new Vector3(vec.x, vec.y, vec.z - distance);
    //        objList[i].obj.transform.LookAt(objs[objList[i + 1].id].transform);
    //    }
    //}

	void UpdateObjPos(bool defb=false)
    {
		if (mode == AnimationMode.ByFrame) {
			List<Pair> llp = new List<Pair> ();
			for (int i = 0; i < objList.Count; i++) 
				if (!pl.GetSide (objList [i].obj.transform.position)) 
					llp.Add (objList [i]);
			
			while (llp.Count != 0) {
				Destroy (llp [0].obj);
				objList.Remove (llp [0]);
				llp.RemoveAt (0);
                CreateObjByFrame(false);
			}
				
			objList [0].obj.transform.position += new Vector3 (-moveSpeed, 0, 0);//new Vector3 (0, 0, moveSpeed);
			for (int i = objList.Count - 1; i >= 0; i--) {
				UpdateDistance (i);
			}
			/*
			List<int> destroy = new List<int> ();
			for (int i = 0; i < objs.Count; i++) {
				if (objList [i].id >= objs.Count - 1)
					destroy.Add (i);
			}

			for (int i = 0; i < destroy.Count; i++) {
				Destroy (objList [i].obj);
				objList.RemoveAt (i);
			}*/
		}
		if (mode == AnimationMode.ByTime) {
			for (int i = objList.Count - 1; i >= 0; i--) {
				//if (Normal != Vector3.zero && i - 1 >= 0)
				//{
				//    Plane pl = new Plane(Normal, objList[i].obj.transform.position);
				//    Ray ray = new Ray(objs[objList[i].id].transform.position, objs[objList[i].id].transform.position - objs[objList[i-1].id].transform.position);
				//    float enter;
				//    pl.Raycast(ray, out enter);
				//    //objList[i].obj.transform.position = ray.GetPoint(enter);
				//    print(ray.GetPoint(enter));
				//}
				float dis = 0;
				if (mode == AnimationMode.ByTime)
					dis = moveSpeed * Time.deltaTime;
				if (mode == AnimationMode.ByFrame) {
					if (defb)
						dis = moveSpeed * createSpeedByFrame;
					else
						dis = moveSpeed;
				}
				objList [i].obj.transform.position = Vector3.MoveTowards (objList [i].obj.transform.position, objs [objList [i].id].transform.position/* posList[objList[i].id]*/, dis);
				//objList [i].obj.transform.LookAt (objs [objList [i].id].transform);
				objList [i].obj.transform.forward = objs [objList [i].id].transform.position - objs [objList [i].id - 1].transform.position;
				if (Vector3.Distance (objList [i].obj.transform.position, objs [objList [i].id].transform.position /*posList[objList[i].id]*/) < (dis + 0.3f)) {//Vector3.Distance(objList[i].obj.transform.position, objs[objList[i].id].transform.position /*posList[objList[i].id]*/) < dis)
					incrementList.Add (i);
				}
			}

			foreach (int i in incrementList) {
				int b = objList [i].id;
				GameObject oj = objList [i].obj;

				if (b < posList.Count - 1)
					oj.transform.forward = objs [b + 1].transform.position - objs [b].transform.position /* posList[b + 1] - posList[b]*/;
				// oj.transform.position = objs[b].transform.position/*posList[b]*/;

				Pair p = new Pair (b + 1, oj);

				objList.RemoveAt (i);
				objList.Insert (i, p);

				if (objList [i].id > posList.Count - 1) {
					removeList.Add (i);
					//continue;
				}
			}
			incrementList.Clear ();
		}
    }

    void DestroyObj()
    {
        foreach (int i in removeList)
        {
            GameObject.Destroy(objList[i].obj);
            objList.RemoveAt(i);
        }
        removeList.Clear();
    }

	void UpdateDistance(int index){
		try{
		GameObject taregt =	objList [index].obj;
		int targetindex = objList [index].id;
		GameObject follow = objList [index - 1].obj;
		int followindex = objList [index - 1].id;
		Vector3 targetposition = objs [targetindex].transform.position;
		Vector3 originposition = objs [targetindex-1].transform.position;
		Ray ray = new Ray (originposition, targetposition - originposition);

		if (targetindex == followindex) {
			taregt.transform.forward = ray.direction;
			taregt.transform.position = ray.GetPoint ((follow.transform.position - originposition).magnitude - moveSpeed * createSpeedByFrame);
		}
		if (targetindex != followindex) {
			float followdistance = (follow.transform.position - targetposition).magnitude;
			if (followdistance < moveSpeed * createSpeedByFrame) {
				taregt.transform.forward = ray.direction;
				taregt.transform.position = ray.GetPoint ((targetposition - originposition).magnitude - createSpeedByFrame * moveSpeed + followdistance);
			}
			else {
					objList [index].id = followindex;
					UpdateDistance (index);
			}
		}
		}catch(System.Exception e){
			//if(index)
		}
	}

#if UNITY_EDITOR
    // Window has been selected
    //void OnFocus()
    //{
    //    // Remove delegate listener if it has previously
    //    // been assigned.
    //    SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    //    // Add (or re-add) the delegate.
    //    SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    //}

    //void OnDestroy()
    //{
    //    // When the window is destroyed, remove the delegate
    //    // so that it will no longer do any drawing.
    //    SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    //}

    //void OnSceneGUI(SceneView sceneView)
    //{
    //    // Do your drawing here using Handles.
    //    Handles.BeginGUI();
    //    // Do your drawing here using GUI.
    //    Handles.EndGUI();
    //}
#endif
}