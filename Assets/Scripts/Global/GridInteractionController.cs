using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GridInteractionController : MonoBehaviour{
	public GameObject Factions;
	public GameObject SelectHighlight;
	public abstract void clickCoordinate(int x, int y);
	
	
	public void placeSelectHighlight(int x, int y){
		//		SelectHighlight.transform.position = new Vector3(x + 0.5f,
		//		                                                 SelectHighlight.transform.position.y,
		//		                                                 y + 0.5f);
		SelectHighlight.transform.position = new Vector3(x + 0.5f,
		                                                 Mathf.Max(PlayMap.Grid.getTileSpec(x,y).FloorHeight,0.2f) + 0.11f,
		                                                 y + 0.5f);
		SelectHighlight.SetActive (true);
	}
	public void removeSelectHighlight(){
		SelectHighlight.SetActive (false);
	}
}

