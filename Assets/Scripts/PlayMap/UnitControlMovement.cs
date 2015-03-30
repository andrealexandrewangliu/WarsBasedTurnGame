using UnityEngine;
using System.Collections;

public class UnitControlMovement{
	private int sizex, sizey;
	private GridControl TilesSpec;
	private UnitControl UnitsSpec;
	private int[,] cost;
	private OriginDirection[,] direction;
	private ArrayList movementPattern = new ArrayList();
	private ArrayList highlightedTiles = new ArrayList();
	private int debt = 0;
	private int originx = -1, originy = -1;

	public UnitControlMovement(int x, int y, GridControl tiles, UnitControl units){
		this.sizex = x;
		this.sizey = y;
		TilesSpec = tiles;
		UnitsSpec = units;
		clear ();
	}

	public enum OriginDirection{
		N,
		S,
		E,
		W
	};
	public void fullclear(){
		clear ();
		hidePath ();
		movementPattern.Clear ();
		debt = 0;
		originx = -1;
		originy = -1;
	}

	public void clear(){
		cost = new int[sizex, sizey];
		direction = new OriginDirection[sizex, sizey];
		clearHighlights ();
	}
	
	public void clearHighlights(){
		foreach (TileSelect tile in highlightedTiles) {
			tile.hideHighlight();
		}
		highlightedTiles.Clear ();
	}
	
	public void showHighlights(){
		foreach (TileSelect tile in highlightedTiles) {
			tile.showHighlight();
		}
		renderPath ();
	}

	private static int getMovementCost(UnitSpec.UnitMove unitMovementType, TileSelect.TileMovementType tileLandType){
		switch (unitMovementType) {
		case UnitSpec.UnitMove.Fly:
			return 2;
		case UnitSpec.UnitMove.Heli:
			return 2;
		case UnitSpec.UnitMove.Dive:
			switch (tileLandType) {
			case TileSelect.TileMovementType.DeepWater:
				return 3;
			}
			break;
		case UnitSpec.UnitMove.Float:
			switch (tileLandType) {
			case TileSelect.TileMovementType.Normal:
				return 2;
			case TileSelect.TileMovementType.Sand:
				return 2;
			case TileSelect.TileMovementType.Concrete:
				return 2;
			case TileSelect.TileMovementType.Hill:
				return 3;
			case TileSelect.TileMovementType.Obstructed:
				return 5;
			case TileSelect.TileMovementType.Mountain:
				return 5;
			case TileSelect.TileMovementType.DeepWater:
				return 3;
			case TileSelect.TileMovementType.ShallowWater:
				return 3;
			}
			break;
		case UnitSpec.UnitMove.Sail:
			switch (tileLandType) {
			case TileSelect.TileMovementType.DeepWater:
				return 2;
			case TileSelect.TileMovementType.ShallowWater:
				return 3;
			}
			break;
		case UnitSpec.UnitMove.Thread:
			switch (tileLandType) {
			case TileSelect.TileMovementType.Normal:
				return 3;
			case TileSelect.TileMovementType.Sand:
				return 3;
			case TileSelect.TileMovementType.Concrete:
				return 2;
			case TileSelect.TileMovementType.Hill:
				return 10;
			case TileSelect.TileMovementType.Obstructed:
				return 6;
			}
			break;
		case UnitSpec.UnitMove.Walk:
			switch (tileLandType) {
			case TileSelect.TileMovementType.Normal:
				return 3;
			case TileSelect.TileMovementType.Sand:
				return 5;
			case TileSelect.TileMovementType.Concrete:
				return 2;
			case TileSelect.TileMovementType.Hill:
				return 4;
			case TileSelect.TileMovementType.Obstructed:
				return 4;
			case TileSelect.TileMovementType.Mountain:
				return 10;
			case TileSelect.TileMovementType.ShallowWater:
				return 5;
			}
			break;
		case UnitSpec.UnitMove.Wheel:
			switch (tileLandType) {
			case TileSelect.TileMovementType.Normal:
				return 3;
			case TileSelect.TileMovementType.Sand:
				return 4;
			case TileSelect.TileMovementType.Concrete:
				return 1;
			case TileSelect.TileMovementType.Hill:
				return 10;
			case TileSelect.TileMovementType.Obstructed:
				return 8;
			}
			break;
		}


		return 0;
	}

	public void mapMovement(int x, int y, int movement, UnitSpec.UnitMove movementType){
		clear ();
		cost [x, y] = -2;
		if (originx < 0) {
			originx = x;
			originy = y;
		}
		dijkstraTest(x,y+1,movement,0,OriginDirection.S,movementType,true);
		dijkstraTest(x,y-1,movement,0,OriginDirection.N,movementType,true);
		dijkstraTest(x+1,y,movement,0,OriginDirection.W,movementType,true);
		dijkstraTest(x-1,y,movement,0,OriginDirection.E,movementType,true);
		showHighlights ();
	}

	public void selectPath(int x, int y){
		int px, py;
		int safecounter = 0;
		bool first = true;
		if (cost [x, y] > 0) {
			px = x;
			py = y;
			while (px != originx || py != originy){
				movementPattern.Insert(0, new Vector2(px, py));
//				string pathLog = "PathLog: [" + safecounter + "] x=" + px + ", y=" + py +" d=" + direction[px,py];
//				Debug.Log(pathLog);

				switch(direction[px,py]){
				case OriginDirection.N:
					py++;
					break;
				case OriginDirection.S:
					py--;
					break;
				case OriginDirection.E:
					px++;
					break;
				case OriginDirection.W:
					px--;
					break;
				}
				safecounter++;
				if (safecounter > 100){
					Debug.Log("PathIssue");
					break;
				}

			}
		}
		renderPath ();
	}

	public void renderPath(){
		int routeSize = movementPattern.Count;
		if (routeSize > 0){
			Vector2 coord;
			OriginDirection d;
			PathShow pathRenderer = TilesSpec.getTileSpec(originx, originy).PathRenderer;
			pathRenderer.Type = PathShow.PathType.Origin;
			for (int i = 0; i < routeSize; i++) {
				coord= (Vector2) movementPattern [i];
				d = direction[(int) coord.x, (int) coord.y];
				switch (d){
				case OriginDirection.N:
					pathRenderer.Next = OriginDirection.S;
					break;
				case OriginDirection.S:
					pathRenderer.Next = OriginDirection.N;
					break;
				case OriginDirection.E:
					pathRenderer.Next = OriginDirection.W;
					break;
				case OriginDirection.W:
					pathRenderer.Next = OriginDirection.E;
					break;
				}
				pathRenderer.ShowPath ();
				pathRenderer = TilesSpec.getTileSpec((int) coord.x, (int) coord.y).PathRenderer;
				pathRenderer.Prev = direction[(int) coord.x, (int) coord.y];
				if (i >= routeSize -1)
					pathRenderer.Type = PathShow.PathType.Target;
				else
					pathRenderer.Type = PathShow.PathType.Path;
			}
			pathRenderer.ShowPath ();
		}
	}

	public void hidePath(){
		PathShow pathRenderer;
		if (originx >= 0) {
			pathRenderer = TilesSpec.getTileSpec (originx, originy).PathRenderer;
			pathRenderer.HidePath ();
			foreach (Vector2 coord in movementPattern){
				pathRenderer = TilesSpec.getTileSpec((int) coord.x, (int) coord.y).PathRenderer;
				pathRenderer.HidePath ();
			}
		}
	}
	
	public ArrayList getRoute(){
		return movementPattern;
	}
	
	public int getRouteSize(){
		return movementPattern.Count;
	}
	
	public Vector2 getRoute(int index){
		return (Vector2)movementPattern[index];
	}
	
	public OriginDirection getRouteDirection(int index){
		Vector2 coord = (Vector2) movementPattern [index];
		return direction[(int) coord.x, (int) coord.y];
	}

	private void dijkstraTest(int x, int y, int movement, int debt, OriginDirection d, UnitSpec.UnitMove movementType, bool force){
		if (x >= 0 && x < this.sizex && y >= 0 && y < this.sizey && cost[x,y] != -2) {
			TileSelect tile = TilesSpec.getTileSpec(x,y);
			int newcost = getMovementCost(movementType, tile.FloorType);
			if (newcost != 0){
				newcost += debt;
				if ((newcost < cost[x,y] || cost[x,y] <= 0) && (force || newcost <= movement)){
					cost[x,y] = newcost;
					direction[x,y] = d;
					highlightedTiles.Add(tile);
					dijkstraTest(x,y+1,movement,newcost,OriginDirection.S,movementType,false);
					dijkstraTest(x,y-1,movement,newcost,OriginDirection.N,movementType,false);
					dijkstraTest(x+1,y,movement,newcost,OriginDirection.W,movementType,false);
					dijkstraTest(x-1,y,movement,newcost,OriginDirection.E,movementType,false);
				}
			}
		}
	}

}
