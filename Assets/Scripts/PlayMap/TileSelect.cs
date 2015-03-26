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
//	void Start(){
//		showHighlight();
//	}

	void OnMouseDown(){
		if (active) {
			GameObject grid = GameObject.Find("Grid");
			if (grid != null){
				((UnitControl)grid.GetComponent("UnitControl")).
					clickCoordinate((int)coordinate.x, (int)coordinate.y);
			}
		}
	}
	
	public void showHighlight(){
		this.transform.FindChild ("Highlight").gameObject.SetActive(true);
	}
	
	public void hideHighlight(){
		this.transform.FindChild ("Highlight").gameObject.SetActive(false);
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
