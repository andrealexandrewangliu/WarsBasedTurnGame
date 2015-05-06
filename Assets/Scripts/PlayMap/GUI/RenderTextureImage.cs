using UnityEngine;
using System.Collections;

public class RenderTextureImage : MonoBehaviour {
	public RenderTexture texture;
	private Rect rect;
	private UnityEngine.UI.Image ImageScript;
	private Texture2D T2D;
	private bool init = false;
	// Use this for initialization
	void Start () {
		Init ();
	}

	void Init(){
		if (!init) {
			ImageScript = (UnityEngine.UI.Image) this.GetComponent ("Image");
			T2D = new Texture2D (texture.width, texture.height);
			rect = new Rect (0, 0, texture.width, texture.height);
			init = true;
		}
	}

//	void Update(){
//		UpdateUnitImage ();
//	}
	
	// Update is called once per frame
	public void UpdateUnitImage () {
		Init ();
		RenderTexture prev = RenderTexture.active;
		RenderTexture.active = texture;
		T2D.ReadPixels (rect, 0, 0);
		T2D.Apply ();
		ImageScript.sprite = Sprite.Create (T2D, rect, new Vector2 (0, 0));
		
		RenderTexture.active = prev;
	}
}
