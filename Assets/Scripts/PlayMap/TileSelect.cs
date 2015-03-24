using UnityEngine;
using System.Collections;

public class TileSelect : MonoBehaviour {
	public Vector2 coordinate;
	public bool active = true;

	void OnMouseDown(){
		if (active) {
			GameObject grid = GameObject.Find("Grid");
			if (grid != null){
				((UnitControl)grid.GetComponent("UnitControl")).
					clickCoordinate((int)coordinate.x, (int)coordinate.y);
			}
		}
	}

}
