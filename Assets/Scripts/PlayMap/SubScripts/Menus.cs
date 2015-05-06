using UnityEngine;
using System.Collections;

public class Menus : MonoBehaviour {

	public GameObject TargetCamera;
	public RenderTextureImage DefenderImage;
	public UnityEngine.UI.Image[] UIPanels;
	public static float ColorLim = 0.5f;

	// Use this for initialization
	void Start () {
		PlayMap.Menu = this;
		PlayMap.UnitMenu = transform.FindChild("UnitMenu").gameObject;
		PlayMap.AttackInfo = transform.FindChild("AttackInfo").gameObject;
	}

	public void Paint(Color color){
		Color uiColor = color * ColorLim;
		uiColor.a = 1;
		foreach (UnityEngine.UI.Image UIPanel in UIPanels) {
			UIPanel.color = uiColor;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
