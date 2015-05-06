using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TileSelect : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	public Vector2 coordinate;
	public bool active = true;
	public bool borderOcclusion = false;
	public GameObject[] NorthBorder;
	public GameObject[] SouthBorder;
	public GameObject[] EastBorder;
	public GameObject[] WestBorder;
	public float FloorHeight = 0.2f;
	public TileMovementType FloorType = TileMovementType.Normal;
	public HighlightType HighLightColorType = HighlightType.Movement;
	public GameObject Highlight;
	public GameObject CursorHighlight;
	public PathShow PathRenderer;
	public float defense = 0;

	public enum HighlightType{
		Movement,
		Enemy,
		EnemyTargetable,
		EnemyRTargetable,
		Support
	}

	public enum TileMovementType{
		Normal,
		Concrete,
		Sand,
		Hill,
		Mountain,
		Obstructed,
		ShallowBridge, //Shallow Water + Bridge
		DeepBridge,    //Deep Water + Bridge
		ShallowWater,
		DeepWater
	}
	
//	public enum UnitMove{
//		Walk,			//Drone, Walker
//		Wheel,			//Wheeled
//		Thread,			//Track
//		Swim,			//Boat
//		Dive,			//Sub
//		Float,			//Hover, HoverDrone
//		Balloon,		//Heli
//		Fly				//Jet
//	};

	void Start(){
		PathRenderer = (PathShow) transform.FindChild ("PathIcon").gameObject.GetComponent ("PathShow");
		Highlight = this.transform.FindChild ("Highlight").gameObject;
		CursorHighlight = this.transform.FindChild ("TileCursorHighlight").gameObject;
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		if (active) {
			PlayMap.GridController.clickCoordinate((int)coordinate.x, (int)coordinate.y);

		}
	}

	#endregion

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		PlayMap.MoveTargetCamera ((int) coordinate.x, (int) coordinate.y);
		CursorHighlight.SetActive(true);
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		CursorHighlight.SetActive(false);
	}

	#endregion
	
	public void showHighlight(){
		switch (HighLightColorType) {
		case HighlightType.Movement:
			Highlight.GetComponent<Renderer> ().material.color = new Color (27.0f / 255.0f, 174.0f / 255.0f, 195.0f / 255.0f);
			break;
		case HighlightType.Enemy:
			Highlight.GetComponent<Renderer> ().material.color = new Color (255.0f / 255.0f, 144.0f / 255.0f, 16.0f / 255.0f);
			break;
		case HighlightType.EnemyTargetable:
			Highlight.GetComponent<Renderer> ().material.color = new Color (238.0f / 255.0f, 39.0f / 255.0f, 16.0f / 255.0f);
			break;
		case HighlightType.EnemyRTargetable:
			Highlight.GetComponent<Renderer> ().material.color = new Color (255.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
			break;

		}
		Highlight.SetActive(true);
	}
	
	public void hideHighlight(){
		Highlight.SetActive(false);
	}
	
	public void HideNorth(){
		foreach (var border in NorthBorder){
			border.SetActive(false);
		}
	}
	
	public void HideSouth(){
		foreach (var border in SouthBorder){
			border.SetActive(false);
		}
	}
	
	public void HideWest(){
		foreach (var border in WestBorder){
			border.SetActive(false);
		}
	}
	
	public void HideEast(){
		foreach (var border in EastBorder){
			border.SetActive(false);
		}
	}
	
	public void resetCapture(){
		TileCapturable capturable = (TileCapturable) GetComponent ("TileCapturable");
		if (capturable != null){
			capturable.resetCapture();
		}
	}

	public void capture(GameUnit unit){
		TileCapturable capturable = (TileCapturable) GetComponent ("TileCapturable");
		if (capturable != null){
			capturable.capture(unit);
		}
	}
}
