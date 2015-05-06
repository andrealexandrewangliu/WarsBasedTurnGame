using UnityEngine;
using System.Collections;

public class UnitControlMovement{
	private int sizex, sizey;
	private int[,] cost;
	private OriginDirection[,] direction;
	private ArrayList movementPattern = new ArrayList();
	private ArrayList highlightedTiles = new ArrayList();
	private int originx = -1, originy = -1;
	private GameUnit unit = null;

	public UnitControlMovement(int x, int y){
		this.sizex = x;
		this.sizey = y;
		this.direction = new OriginDirection[sizex, sizey];
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
		originx = -1;
		originy = -1;
		unit = null;
	}

	public void clear(){
		cost = new int[sizex, sizey];
		//direction = new OriginDirection[sizex, sizey];
		clearHighlights ();
	}
	
	public void clearHighlights(){
		foreach (TileSelect tile in highlightedTiles) {
			tile.HighLightColorType = TileSelect.HighlightType.Movement;
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

	private static int getMovementCost(UnitSpec.UnitMove unitMovementType, TileSelect.TileMovementType tileLandType, bool isAmphibious){
		if (isAmphibious && 
		    unitMovementType != UnitSpec.UnitMove.Dive && 
		    unitMovementType != UnitSpec.UnitMove.Float && 
		    unitMovementType != UnitSpec.UnitMove.Fly && 
		    unitMovementType != UnitSpec.UnitMove.Heli && 
		    unitMovementType != UnitSpec.UnitMove.Sail) {
			switch (tileLandType) {
			case TileSelect.TileMovementType.DeepWater:
				return 5;
			case TileSelect.TileMovementType.ShallowWater:
				return 5;
			default:
				return getMovementCost (unitMovementType, tileLandType);
			}
		}
		return getMovementCost (unitMovementType, tileLandType);
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
			case TileSelect.TileMovementType.DeepBridge:
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
			case TileSelect.TileMovementType.DeepBridge:
				return 2;
			case TileSelect.TileMovementType.ShallowBridge:
				return 2;
			}
			break;
		case UnitSpec.UnitMove.Sail:
			switch (tileLandType) {
			case TileSelect.TileMovementType.DeepWater:
				return 2;
			case TileSelect.TileMovementType.ShallowWater:
				return 3;
			case TileSelect.TileMovementType.DeepBridge:
				return 2;
			case TileSelect.TileMovementType.ShallowBridge:
				return 3;
			}
			break;
		case UnitSpec.UnitMove.Thread:
			switch (tileLandType) {
			case TileSelect.TileMovementType.Normal:
				return 2;
			case TileSelect.TileMovementType.Sand:
				return 3;
			case TileSelect.TileMovementType.Concrete:
				return 2;
			case TileSelect.TileMovementType.Hill:
				return 10;
			case TileSelect.TileMovementType.Obstructed:
				return 6;
			case TileSelect.TileMovementType.DeepBridge:
				return 2;
			case TileSelect.TileMovementType.ShallowBridge:
				return 2;
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
			case TileSelect.TileMovementType.DeepBridge:
				return 2;
			case TileSelect.TileMovementType.ShallowBridge:
				return 2;
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
			case TileSelect.TileMovementType.DeepBridge:
				return 1;
			case TileSelect.TileMovementType.ShallowBridge:
				return 1;
			}
			break;
		case UnitSpec.UnitMove.HalfTrack:
			switch (tileLandType) {
			case TileSelect.TileMovementType.Normal:
				return 2;
			case TileSelect.TileMovementType.Sand:
				return 3;
			case TileSelect.TileMovementType.Concrete:
				return 2;
			case TileSelect.TileMovementType.Hill:
				return 10;
			case TileSelect.TileMovementType.Obstructed:
				return 8;
			case TileSelect.TileMovementType.DeepBridge:
				return 2;
			case TileSelect.TileMovementType.ShallowBridge:
				return 2;
			}
			break;
		}


		return 0;
	}

	public void HighLightRangedMarks(int x, int y){
		int maxRange = unit.UnitBodySpec.getMaxRange ();
		int yrange, distance;
		for (int px = -maxRange; px <= maxRange; px++) {
			int tx = x + px;
			if (tx >= 0 && tx < sizex){
				yrange = maxRange - Mathf.Abs(px);
				for (int py = -yrange; py <= yrange; py++){
					int ty = y + py;
					if (ty >= 0 && ty < sizey){
						distance = Mathf.Abs(tx - x) + Mathf.Abs (ty - y);
						if (((UnitControl) PlayMap.GridController).HasSeenEnemyUnit(tx, ty, unit.playerFactionSpec)){
							if (distance > 1 && UnitControlBattle.canTarget(unit, ((UnitControl) PlayMap.GridController).getUnitData(tx, ty), distance)){
								TileSelect tile = PlayMap.Grid.getTileSpec(tx,ty);
//								Debug.Log ("RangePoint (" + tx + "," + ty +") for max " + maxRange); 
								tile.HighLightColorType = TileSelect.HighlightType.EnemyRTargetable;
								highlightedTiles.Add(tile);
							}
						}
					}
				}
			}
		}
	}
	
	public void mapTargets(int x, int y){
		clear ();
		if (originx < 0) {
			originx = x;
			originy = y;
		}
		if (x == originx && y == originy)
			HighLightRangedMarks (x, y);
		testTarget(x,y+1);
		testTarget(x,y-1);
		testTarget(x+1,y);
		testTarget(x-1,y);
		showHighlights ();
	}

	public void mapMovement(int x, int y, GameUnit unit){
		clear ();
		if (originx < 0) {
			originx = x;
			originy = y;
		}
		this.unit = unit;
		HighLightRangedMarks (x, y);
		dijkstraTest(x,y+1,0,OriginDirection.S,true);
		dijkstraTest(x,y-1,0,OriginDirection.N,true);
		dijkstraTest(x+1,y,0,OriginDirection.W,true);
		dijkstraTest(x-1,y,0,OriginDirection.E,true);
		showHighlights ();
	}
	
	private void reMapMovement(){
		clear ();
		int x = -5, y = -5;
		int debt = 0;
		foreach (Vector2 coord in movementPattern) {
			x = (int) coord.x;
			y = (int) coord.y;
			TileSelect tile = PlayMap.Grid.getTileSpec(x,y);
			debt += getMovementCost(unit.getMovementType(), tile.FloorType, unit.UnitBodySpec.isAmphibious ());
			cost[x, y] = -debt;
		}
		dijkstraTest(x,y+1,debt,OriginDirection.S,false);
		dijkstraTest(x,y-1,debt,OriginDirection.N,false);
		dijkstraTest(x+1,y,debt,OriginDirection.W,false);
		dijkstraTest(x-1,y,debt,OriginDirection.E,false);
		showHighlights ();
	}

	public bool attackSelect(int x, int y, GameUnit unit){
		int tx = x;
		int ty = y;
		int ccost = 60000;


		if(movementPattern.Count > 0){
			Vector2 coord = (Vector2)movementPattern[movementPattern.Count - 1];
			if (Mathf.Abs (tx - (int)coord.x) + Mathf.Abs (ty - (int)coord.y) == 1) // ADJACENT TARGET
				return true;
		}
		else if (Mathf.Abs (tx - originx) + Mathf.Abs (ty - originy) == 1) // ADJACENT TARGET
			return true;
		
		if (x - 1 > 0 && cost[x - 1, y] < ccost && cost[x - 1, y] > 0 && !((UnitControl) PlayMap.GridController).HasUnit(x - 1, y)) { // check West
			ccost = cost[x - 1, y];
			tx = x - 1;
		}
		if (x + 1 < sizex && cost[x + 1, y] < ccost && cost[x + 1, y] > 0 && !((UnitControl) PlayMap.GridController).HasUnit(x + 1, y)) { // check East
			ccost = cost[x + 1, y];
			tx = x + 1;
		}
		if (y + 1 < sizey && cost[x, y + 1] < ccost && cost[x, y + 1] > 0 && !((UnitControl) PlayMap.GridController).HasUnit(x, y + 1)) { // check North
			ccost = cost[x, y + 1];
			tx = x;
			ty = y + 1;
		}
		if (y - 1 > 0 && cost[x, y - 1] < ccost && cost[x, y - 1] > 0 && !((UnitControl) PlayMap.GridController).HasUnit(x, y - 1)) { // check South
			ccost = cost[x, y - 1];
			tx = x;
			ty = y - 1;
		}
		if (tx != x || ty != y) {
			return selectPath(tx, ty, unit);
		}
		return false;
	}

	public bool selectPath(int x, int y, GameUnit unit){
		int px, py;
		int safecounter = 0;
		this.unit = unit;
		if (cost [x, y] > 0) {
			movementPattern.Clear ();
			px = x;
			py = y;
			while (px != originx || py != originy) {
				movementPattern.Insert (0, new Vector2 (px, py));
//				string pathLog = "PathLog: [" + safecounter + "] x=" + px + ", y=" + py +" d=" + direction[px,py];
//				Debug.Log(pathLog);

				switch (direction [px, py]) {
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
				if (safecounter > 100) {
					Debug.Log ("PathIssue");
					break;
				}

			}
		} 
		else if (cost [x, y] < 0 && movementPattern.Count > 0){ // PathBacktracing
			PathShow pathRenderer;
			Vector2 coord = (Vector2) movementPattern[movementPattern.Count - 1];
			px = (int) coord.x;
			py = (int) coord.y;
			while (movementPattern.Count > 1 &&
			       (px != x  || py != y)){
				pathRenderer = PlayMap.Grid.getTileSpec(px, py).PathRenderer;
				pathRenderer.HidePath ();
				movementPattern.RemoveAt(movementPattern.Count - 1);
				coord = (Vector2) movementPattern[movementPattern.Count - 1];
				px = (int) coord.x;
				py = (int) coord.y;
			}
			pathRenderer = PlayMap.Grid.getTileSpec(px, py).PathRenderer;
			pathRenderer.Type = PathShow.PathType.Target;
		}

		if (cost [x, y] != 0) {
			reMapMovement ();
			return true;
		} 
		else {
			return false;
		}
	}

	public void renderPath(){
		int routeSize = movementPattern.Count;
		if (routeSize > 0){
			Vector2 coord;
			OriginDirection d;
			PathShow pathRenderer = PlayMap.Grid.getTileSpec(originx, originy).PathRenderer;
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
				pathRenderer = PlayMap.Grid.getTileSpec((int) coord.x, (int) coord.y).PathRenderer;
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
			pathRenderer = PlayMap.Grid.getTileSpec (originx, originy).PathRenderer;
			pathRenderer.HidePath ();
			foreach (Vector2 coord in movementPattern){
				pathRenderer = PlayMap.Grid.getTileSpec((int) coord.x, (int) coord.y).PathRenderer;
				pathRenderer.HidePath ();
			}
		}
	}
	
	public ArrayList getRoute(){
		return movementPattern;
	}
	
	public int getRouteSize(){
		int routeSize = 0;
		Vector2 coord;
		if (movementPattern.Count > 0) {
			bool foundEnemy = false;

			for (routeSize = 0; routeSize < movementPattern.Count; routeSize++) {
				coord = (Vector2)movementPattern [routeSize];
				if (((UnitControl) PlayMap.GridController).HasEnemyUnit ((int)coord.x, (int)coord.y, unit.playerFactionSpec)) {
					foundEnemy = true;
					break;
				}
			}
			if (foundEnemy){
				if (routeSize > 0){
					coord = (Vector2)movementPattern [routeSize];
					while (((UnitControl) PlayMap.GridController).HasUnit((int)coord.x, (int) coord.y)) {
						routeSize --;
						coord = (Vector2)movementPattern [routeSize];
					}
					return routeSize + 1;
				}
				else
					return 0;
			}
			return movementPattern.Count;
		}
		return 0;
	}

	public int getPathSize(){
		return movementPattern.Count;
	}
	
	public Vector2 getRoute(int index){
		return (Vector2)movementPattern[index];
	}
	
	public OriginDirection getRouteDirection(int index){
		Vector2 coord = (Vector2) movementPattern [index];
		return direction[(int) coord.x, (int) coord.y];
	}
	
	private void dijkstraTest(int x, int y, int debt, OriginDirection d, bool firstmove){
		int movement = unit.getMovement();
		UnitSpec.UnitMove movementType = unit.getMovementType ();
		if (x >= 0 && x < this.sizex && y >= 0 && y < this.sizey && // within boundaries
		    cost[x,y] >= 0 && // permission to map
		    (x!= originx || y!= originy) // Not unit origin
		    ) {
			TileSelect tile = PlayMap.Grid.getTileSpec(x,y);
			if (tile.HighLightColorType != TileSelect.HighlightType.EnemyRTargetable && //Ignore if there is a ranged marker
			    ((UnitControl) PlayMap.GridController).HasSeenEnemyUnit(x, y, unit.playerFactionSpec)){
				if (UnitControlBattle.canTarget(unit, ((UnitControl) PlayMap.GridController).getUnitData(x, y), 1))
					tile.HighLightColorType = TileSelect.HighlightType.EnemyTargetable;
				else
					tile.HighLightColorType = TileSelect.HighlightType.Enemy;
				highlightedTiles.Add(tile);
			}
			else {
				int newcost = getMovementCost(movementType, tile.FloorType, unit.UnitBodySpec.isAmphibious ());
				if (newcost != 0){
					newcost += debt;
					if ((newcost < cost[x,y] || cost[x,y] <= 0) && (firstmove || newcost <= movement)){
						cost[x,y] = newcost;
						direction[x,y] = d;
						//tile.HighLightColorType = TileSelect.HighlightType.Movement;
						highlightedTiles.Add(tile);
						dijkstraTest(x,y+1,newcost,OriginDirection.S,false);
						dijkstraTest(x,y-1,newcost,OriginDirection.N,false);
						dijkstraTest(x+1,y,newcost,OriginDirection.W,false);
						dijkstraTest(x-1,y,newcost,OriginDirection.E,false);
					}
				}
			}
		}
	}
	
	private void testTarget(int x, int y){
		if (x >= 0 && x < this.sizex && y >= 0 && y < this.sizey){
			TileSelect tile = PlayMap.Grid.getTileSpec(x,y);
			if (tile.HighLightColorType != TileSelect.HighlightType.EnemyRTargetable && //Ignore if there is a ranged marker
			    ((UnitControl) PlayMap.GridController).HasSeenEnemyUnit(x, y, unit.playerFactionSpec)){
				if (UnitControlBattle.canTarget(unit, ((UnitControl) PlayMap.GridController).getUnitData(x, y), 1))
					tile.HighLightColorType = TileSelect.HighlightType.EnemyTargetable;
				else
					tile.HighLightColorType = TileSelect.HighlightType.Enemy;
				highlightedTiles.Add(tile);
			}
		}
	}

}
