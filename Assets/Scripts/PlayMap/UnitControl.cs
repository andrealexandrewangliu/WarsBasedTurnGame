using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour {
	public GameObject SelectHighlight;
	private GridControl GridControlScript;
	public GameObject Factions;
	private GameObject[,] UnitPlacement; // Instances of prefabUnit
	public int selectTileX = -1;
	public int selectTileY = -1;
	private UnitControlMovement UnitMovementController;
	public float UnitSpeed = 10.0f;
	public int selectUnitX = -1;
	public int selectUnitY = -1;

	
	public GameObject getUnit(int x, int y){
		return UnitPlacement [x, y];
	}
	public GameUnit getUnitData(int x, int y){
		return (GameUnit) UnitPlacement [x, y].GetComponent ("GameUnit");
	}

	// Use this for initialization
	void Start () {
		GameUnit.setUnitSpeed (UnitSpeed);
		GridControlScript = (GridControl)this.GetComponent ("GridControl");
		UnitPlacement = new GameObject[(int)GridControlScript.MapSize.x,(int)GridControlScript.MapSize.y];
		SpawnUnit (1, 1, 0, 0);
		UnitMovementController = new UnitControlMovement ((int)GridControlScript.MapSize.x, (int)GridControlScript.MapSize.y
		                                                  ,GridControlScript, this);
	}
	
	Vector2 Move(int ox, int oy, int tx, int ty, UnitControlMovement.OriginDirection d){
		var unit = UnitPlacement [ox, oy];
		if (UnitPlacement [tx, ty] == null) {
			UnitPlacement [ox, oy] = null;
			UnitPlacement [tx, ty] = unit;
			GameUnit unitStat = (GameUnit)unit.GetComponent ("GameUnit");
			unitStat.addWaypoint (new Vector3 (tx + 0.5f, GridControlScript.getFloorHeight (tx, ty), ty + 0.5f), d);
			return new Vector2(tx, ty);
		} else {
			GameUnit unitStat = (GameUnit)unit.GetComponent ("GameUnit");
			GameUnit unitOtherStat = (GameUnit)UnitPlacement [tx, ty].GetComponent ("GameUnit");
			if (unitStat.getFactionId() != unitOtherStat.getFactionId()){
				return new Vector2(-1, -1);
			}
			return new Vector2(ox, oy);
		}
	}

	void SpawnUnit(int x, int y, int faction, int unitid){
		GameObject prefab = SelectUnitPrefab (faction, unitid);
		GameObject playerFaction = Factions.transform.GetChild(faction).gameObject;
		PlayerFaction playerFactionSpec = (PlayerFaction)playerFaction.GetComponent ("PlayerFaction");

		//var unit = (GameObject)Instantiate (prefab, new Vector3(x + 0.5f, 0.2f, y + 0.5f), Quaternion.identity);
		var unit = GameUnit.InstantiateUnit (prefab, playerFaction);
		unit.transform.position = new Vector3 (x + 0.5f, GridControlScript.getFloorHeight(x,y), y + 0.5f);
		unit.transform.parent = this.transform.FindChild("Units").transform;
		playerFactionSpec.ActiveUnits.Add (unit);
		UnitPlacement [x, y] = unit;
	}

	GameObject SelectUnitPrefab (int faction, int unitid){
		GameObject playerFaction = Factions.transform.GetChild(faction).gameObject;
		PlayerFaction playerFactionSpec = (PlayerFaction)playerFaction.GetComponent ("PlayerFaction");
		BaseFaction baseFactionSpec = (BaseFaction)playerFaction.transform.FindChild("BaseFaction").GetComponent ("BaseFaction");
		if (baseFactionSpec.Units.Length > unitid) {
			return baseFactionSpec.Units [unitid];
		} 
		else {
			unitid -= baseFactionSpec.Units.Length;
		} 
		if (playerFactionSpec.Units.Length > unitid) {
			return playerFactionSpec.Units [unitid];
		} 
		return baseFactionSpec.Units [0]; 
	}
	
	public void placeSelectHighlight(int x, int y){
//		SelectHighlight.transform.position = new Vector3(x + 0.5f,
//		                                                 SelectHighlight.transform.position.y,
//		                                                 y + 0.5f);
		SelectHighlight.transform.position = new Vector3(x + 0.5f,
		                                                 Mathf.Max(GridControlScript.getTileSpec(x,y).FloorHeight,0.2f) + 0.11f,
		                                                 y + 0.5f);
		SelectHighlight.SetActive (true);
	}
	public void removeSelectHighlight(){
		SelectHighlight.SetActive (false);
	}

	public void clickCoordinate(int x, int y){

		// First selection or no unit seleted
		if (selectTileX < 0 || selectUnitX < 0) {
			selectTileX = x;
			selectTileY = y;
			
			if (UnitPlacement[selectTileX,selectTileY] != null){
				selectUnitX = x;
				selectUnitY = y;
				GameUnit unit = (GameUnit) UnitPlacement[selectTileX,selectTileY].GetComponent("GameUnit");
				UnitMovementController.mapMovement(x,y,unit.getMovement(), unit.getMovementType());
			}
		} else {
			if (selectUnitX >= 0){

				//Second choosing of the same tile
				if(selectTileX == x && selectTileY == y && UnitMovementController.getRouteSize() > 0){
					int px = selectUnitX, py = selectUnitY;

					//Resets selection
					selectUnitX = selectTileX = -1;
					selectUnitY = selectTileY = -1;

					if (UnitMovementController.getRouteSize() > 0){
						for(int i = 0; i < UnitMovementController.getRouteSize(); i++){
							Vector2 coord = UnitMovementController.getRoute(i);
							Vector2 moveReturn = Move(px, py, (int) coord.x, (int) coord.y, UnitMovementController.getRouteDirection(i));
							if (moveReturn.x >= 0){
								px = (int) moveReturn.x;
								py = (int) moveReturn.y;
							}
							else{
								break;
							}
						}
					}
					UnitMovementController.fullclear();
				}
				else{
					selectTileX = x;
					selectTileY = y;
					UnitMovementController.fullclear();
					GameUnit unit = (GameUnit) UnitPlacement[selectUnitX,selectUnitY].GetComponent("GameUnit");
					UnitMovementController.mapMovement(selectUnitX,selectUnitY,unit.getMovement(), unit.getMovementType());
					UnitMovementController.selectPath(x,y);
				}
			}
			else{
				selectTileX = x;
				selectTileY = y;
			}
		}


		if (selectTileX >= 0){
			placeSelectHighlight(x, y);
		}
		else{
			removeSelectHighlight();
		}
	}


	
	// Update is called once per frame
	void Update () {
	}
}
