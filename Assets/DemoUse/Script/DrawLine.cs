using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
	public Material lineMaterial;
	public Material red;
	Material mat;
	public GameObject obj;
	static public bool bDraw = false;
	static public bool bTarget = false;
	// Use this for initialization
	void Start ()
	{
		mat = new Material("Shader \"Lines/Colored Blended\" {" +    
			                               "SubShader { Pass { " +    
			                               "    Blend SrcAlpha OneMinusSrcAlpha " +    
			                               "    ZWrite Off Cull Off Fog { Mode Off } " +    
			                               "    BindChannels {" +    
			                               "      Bind \"vertex\", vertex Bind \"color\", color }" +    
			                               "} } }");//生成画线的材质    
		        mat.hideFlags = HideFlags.HideAndDontSave;    
		        mat.color = Color.green;   
		        mat.shader.hideFlags = HideFlags.HideAndDontSave;    
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnPostRender()
	{
		if (bDraw == false)
			return;

		//GL.PushMatrix();

		if (bTarget == false) {
			GL.Color (Color.red);
			mat.color = Color.red;

		}
			
		else
		{
			mat.color = Color.green;
			lineMaterial.SetPass(0);
			GL.Color(Color.green);
		}			

	
		mat.SetPass (0);

		GL.Begin(GL.LINES);

		GL.Vertex(obj.transform.position);
		GL.Vertex(obj.transform.forward * 1 + obj.transform.position);

		GL.End();
	}
}

