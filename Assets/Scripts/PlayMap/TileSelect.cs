using UnityEngine;
using System.Collections;

public class TileSelect : MonoBehaviour {
	public Vector2 coordinate;
	public bool active = true;
	public bool borderOcclusion = false;
	public GameObject[] NorthBorder;
	public GameObject[] SouthBorder;
	public GameObject[] EastBorder;
	public GameObject[] WestBorder;
	public float FloorHeight = 0.2f;
	public TileMovementType FloorType = TileMovementType.Normal;
	private GameObject grid;
	private GameObject Highlight;
	private GameObject CursorHighlight;
	public PathShow PathRenderer;

	public enum TileMovementType{
		Normal,
		Concrete,
		Sand,
		Hill,
		Mountain,
		Obstructed,
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
		grid = GameObject.Find("Grid");
		Highlight = this.transform.FindChild ("Highlight").gameObject;
		CursorHighlight = this.transform.FindChild ("TileCursorHighlight").gameObject;
	}

	void OnMouseDown(){
		if (active) {
			if (grid != null){
				((UnitControl)grid.GetComponent("UnitControl")).
					clickCoordinate((int)coordinate.x, (int)coordinate.y);
			}
		}
	}
	
	public void OnMouseEnter(){
		CursorHighlight.SetActive(true);
	}
	
	public void OnMouseExit(){
		CursorHighlight.SetActive(false);
	}
	
	public void showHighlight(){
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
}
