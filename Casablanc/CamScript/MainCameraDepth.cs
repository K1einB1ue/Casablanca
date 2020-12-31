using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[ExecuteInEditMode]   // 可以在编辑界面看到深度绘制效果
public class MainCameraDepth : MonoBehaviour
{

	public Material mat;

	public Camera cam;
	private RenderTexture rt;

	void Start() {

		//		rt = new RenderTexture (width, height, 24);  // 24 bit depth
		//		cam.targetTexture = rt;
	}

	private void OnRenderImage(ref RenderTexture source,ref RenderTexture destination) {
		cam.depthTextureMode = DepthTextureMode.Depth;
		Graphics.Blit(source, destination, mat);
	}

	/*
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, mat);
		//mat is the material which contains the shader
		//we are passing the destination RenderTexture to
	}
	*/

	// Update is called once per frame
}
