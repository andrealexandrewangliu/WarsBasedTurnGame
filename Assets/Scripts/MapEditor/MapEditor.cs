﻿using UnityEngine;
using System.Collections;

public class MapEditor : GridInteractionController {
	public int tilePaint = -1;
	// Use this for initialization
	void Start () {
		PlayMap.GridController = this;
		PlayMap.Grid = (GridControl)this.GetComponent ("GridControl");
		PlayMap.UnitPlacement = null;
	
	}

	public void setTilePaint(int id){
		tilePaint = id;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void clickCoordinate(int x, int y){
		if (tilePaint < 0){
			placeSelectHighlight(x, y);
		}
		else{
			removeSelectHighlight();
			PlayMap.Grid.ChangeTile(x,y,(byte) tilePaint);
		}
	}

}
