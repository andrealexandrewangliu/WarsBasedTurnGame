using UnityEngine;
using System.Collections;

public class UnitControl : GridInteractionController {
	public float UnitSpeed = 10.0f;

	private GameObject[,] UnitPlacement; // Instances of prefabUnit
	private int selectTileX = -1;
	private int selectTileY = -1;
	private int targetTileX = -1;
	private int targetTileY = -1;
	private UnitControlMovement UnitMovementController;
	private UnitControlBattle UnitBattleController;
	private int selectUnitX = -1;
	private int selectUnitY = -1;
	private bool confirmPath = false;
	private Vector3 unitCancelTransform;
	private bool hasCancelTransform = false;

	public UnitControlBattle getBattleController(){
		return UnitBattleController;
	}

	public void EndTurn(){
		UnitControlTurn.nextPlayer ();
		cancelMovement ();
	}
	
	public GameObject getUnit(int x, int y){
		return UnitPlacement [x, y];
	}
	public GameUnit getUnitData(int x, int y){
		return (GameUnit) UnitPlacement [x, y].GetComponent ("GameUnit");
	}
	
	public bool HasUnit(int x, int y){
		return UnitPlacement [x, y] != null;
	}
	
	public bool HasSeenEnemyUnit(int x, int y, PlayerFaction unitFaction){
		if (UnitPlacement [x, y] != null) {
			GameUnit unit = (GameUnit) UnitPlacement [x, y].GetComponent("GameUnit");
			return !unitFaction.isAlliedWith(unit.playerFactionSpec);
		}
		return false;
	}
	
	public bool HasEnemyUnit(int x, int y, PlayerFaction unitFaction){
		if (UnitPlacement [x, y] != null) {
			GameUnit unit = (GameUnit) UnitPlacement [x, y].GetComponent("GameUnit");
			return !unitFaction.isAlliedWith(unit.playerFactionSpec);
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		PlayMap.GridController = this;
		GameUnit.setUnitSpeed (UnitSpeed);
		PlayMap.Grid = (GridControl)this.GetComponent ("GridControl");
		UnitPlacement = new GameObject[(int)PlayMap.Grid.MapSize.x,(int)PlayMap.Grid.MapSize.y];
		PlayMap.UnitPlacement = UnitPlacement;
		SpawnUnit (4, 4, 0, 0);
		SpawnUnit (4, 5, 0, 1);
		SpawnUnit (3, 4, 0, 2);
		SpawnUnit (3, 5, 0, 3);
		SpawnUnit (5, 5, 1, 0);
		SpawnUnit (5, 6, 1, 4);
		SpawnUnit (6, 5, 1, 5);
		UnitMovementController = new UnitControlMovement ((int)PlayMap.Grid.MapSize.x, (int)PlayMap.Grid.MapSize.y);

		EndTurn ();
	}

	void SpawnUnit(int x, int y, int faction, int unitid){
		GameObject prefab = SelectUnitPrefab (faction, unitid);
		GameObject playerFaction = Factions.transform.GetChild(faction).gameObject;
		PlayerFaction playerFactionSpec = (PlayerFaction)playerFaction.GetComponent ("PlayerFaction");

		//var unit = (GameObject)Instantiate (prefab, new Vector3(x + 0.5f, 0.2f, y + 0.5f), Quaternion.identity);
		var unit = GameUnit.InstantiateUnit (prefab, playerFaction);
		GameUnit gameUnit = (GameUnit) unit.GetComponent ("GameUnit");
		unit.transform.position = new Vector3 (x + 0.5f, PlayMap.Grid.getFloorHeight(x,y), y + 0.5f);
		unit.transform.parent = this.transform.FindChild("Units").transform;
		UnitPlacement [x, y] = unit;
		gameUnit.x = x;
		gameUnit.y = y;

	}

	GameObject SelectUnitPrefab (int faction, int unitid){
		GameObject playerFaction = Factions.transform.GetChild(faction).gameObject;
		PlayerFaction playerFactionSpec = (PlayerFaction)playerFaction.GetComponent ("PlayerFaction");
		return playerFactionSpec.getUnitPrefab(unitid); 
	}

	public void confirmMovement(){
		var unit = UnitPlacement [selectUnitX, selectUnitY];
		GameUnit unitStat = (GameUnit)unit.GetComponent ("GameUnit");

		if (!unitStat.HasMoved) {
			int px = selectUnitX, py = selectUnitY;
			int routeSize = UnitMovementController.getRouteSize();

			unitCancelTransform = unit.transform.position;
			hasCancelTransform = true;
		
			targetTileX = selectUnitX;
			targetTileY = selectUnitY;

			if (routeSize > 0) {
				for (int i = 0; i < routeSize; i++) {
					Vector2 coord = UnitMovementController.getRoute (i);
					unitStat.addWaypoint (coord, UnitMovementController.getRouteDirection (i));
					px = (int)coord.x;
					py = (int)coord.y;
				}
			}

			targetTileX = px;
			targetTileY = py;

			if (UnitMovementController.getPathSize () != routeSize) { // INTERRUPT!
				endMovement ();
			} else {
				UnitMovementController.mapTargets (px, py);
				PlayMap.LockMovement = true;
				PlayMap.UnitMenu.SetActive (true);
			}
		}
	}
	
	public void endMovement(){
		var unit = UnitPlacement [selectUnitX, selectUnitY];
		if (unit != null) {
			GameUnit unitStat = (GameUnit)unit.GetComponent ("GameUnit");

			if (targetTileX != selectUnitX || targetTileY != selectUnitY) {
				TileSelect tileOld = PlayMap.Grid.getTileSpec(selectUnitX, selectUnitY);

				tileOld.resetCapture();

				UnitPlacement [targetTileX, targetTileY] = UnitPlacement [selectUnitX, selectUnitY];
				UnitPlacement [selectUnitX, selectUnitY] = null;
				
				unitStat.x = targetTileX;
				unitStat.y = targetTileY;
			}

			unitStat.ConfirmMovement ();

		}
		//Resets selection
		targetTileX = selectUnitX = selectTileX = -1;
		targetTileY = selectUnitY = selectTileY = -1;


		UnitMovementController.fullclear();
		PlayMap.LockMovement = false;
		PlayMap.UnitMenu.SetActive(false);
		PlayMap.HideAttackInfo();
	}
	
	public void cancelMovement(){
		if (selectUnitX >= 0 && selectUnitY >= 0) {
			var unit = UnitPlacement [selectUnitX, selectUnitY];
			GameUnit unitStat = (GameUnit)unit.GetComponent ("GameUnit");
			if (hasCancelTransform){
				unit.transform.position = unitCancelTransform;
				hasCancelTransform = false;
			}

			unitStat.CancelMovement ();

			//Resets selection
			targetTileX = selectUnitX = selectTileX = -1;
			targetTileY = selectUnitY = selectTileY = -1;
		
			UnitMovementController.fullclear ();
			PlayMap.LockMovement = false;
			PlayMap.UnitMenu.SetActive (false);
			PlayMap.HideAttackInfo ();
		}
	}

	public void confirmAttack(){
		float attack = UnitBattleController.AttackerPercentual100Damage ();
		float counter = UnitBattleController.CounterPercentual100Damage ();
		float attackerHp = UnitBattleController.Attacker.getPercentual100HP ();
		float defenderHp = UnitBattleController.Defender.getPercentual100HP ();
		attack *= attackerHp / 100.0f;
		float edefenderHp = defenderHp - attack;
		if (edefenderHp < 0)
			edefenderHp = 0;
		counter *= edefenderHp / 100.0f;
		float eattackerHp = attackerHp - counter;
		if (eattackerHp < 0)
			eattackerHp = 0;
		
		if (edefenderHp <= 0) {
			UnitBattleController.Defender.kill();
			UnitPlacement [selectTileX, selectTileY] = null;
		} 
		else {
			UnitBattleController.Defender.health = UnitBattleController.Defender.health * edefenderHp / defenderHp;
			UnitBattleController.Defender.UpdateUnit ();
		}
		
		if (eattackerHp <= 0) {
			UnitBattleController.Attacker.kill ();
			UnitPlacement [selectUnitX, selectUnitY] = null;
		} 
		else {
			UnitBattleController.Attacker.health = UnitBattleController.Attacker.health * eattackerHp / attackerHp;
			UnitBattleController.Attacker.UnitRotation = 
				UnitBattleController.Attacker.TurretRotation = 
					UnitBattleController.Attacker.confirmRotation = UnitBattleController.attackAngle;
			UnitBattleController.Attacker.SetRotation ();
			UnitBattleController.Attacker.UpdateUnit ();
		}
		
		endMovement ();
	}



	public override void clickCoordinate(int x, int y){
		bool pathconfirming = false;
		if (PlayMap.LockMovement) {
			TileSelect tile = PlayMap.Grid.getTileSpec(x, y);
			if (tile.HighLightColorType == TileSelect.HighlightType.EnemyTargetable || tile.HighLightColorType == TileSelect.HighlightType.EnemyRTargetable){
				if (selectTileX == x && selectTileY == y){
					confirmAttack ();
				}
				else{
					var unit = UnitPlacement [selectUnitX, selectUnitY];
					GameUnit unitStat = (GameUnit)unit.GetComponent ("GameUnit");
					UnitBattleController = UnitControlBattle.CalculateBattle(unitStat, targetTileX, targetTileY, x, y);
					PlayMap.ShowAttackInfo(x, y);
				}
			}
			else{
				PlayMap.HideAttackInfo();
			}
			selectTileX = x;
			selectTileY = y;
		}
		else if (UnitPlacement [x, y] != null) {
			//Set tile selection
			TileSelect tile = PlayMap.Grid.getTileSpec(x, y);

			if (selectUnitX >= 0 && // there is a unit already selected
			    tile.HighLightColorType == TileSelect.HighlightType.EnemyTargetable){ // selecting enemy target for direct combat

//				Debug.Log("ENGAGE!");
				//Second choosing of the same tile
				if(selectTileX == x && selectTileY == y && confirmPath && UnitMovementController.getRouteSize() > 0){
					confirmMovement();
				}
				else{
					GameUnit unit = (GameUnit) UnitPlacement[selectUnitX,selectUnitY].GetComponent("GameUnit");
					if (!unit.HasMoved)
						pathconfirming = UnitMovementController.attackSelect(x, y, unit);
				}
				selectTileX = x;
				selectTileY = y;
			}
			else if (tile.HighLightColorType == TileSelect.HighlightType.EnemyRTargetable){ // selecting indirect combat
//				Debug.Log("BOMBARD!");
				selectTileX = x;
				selectTileY = y;
			}
			else{
				selectTileX = x;
				selectTileY = y;

				if (selectUnitX == x && selectUnitY == y){ //Same unit
					confirmMovement();
				}
				else{
					// a unit was selected
					
	//				Debug.Log("ATTENTION!");

					UnitMovementController.fullclear();

					GameUnit unit = (GameUnit) UnitPlacement[selectTileX,selectTileY].GetComponent("GameUnit");
					if (!unit.HasMoved){
						selectUnitX = x;
						selectUnitY = y;
						UnitMovementController.mapMovement(x,y,unit);
					}
				}
			}
		}
		else {
			if (selectUnitX >= 0){

				//Second choosing of the same tile
				if(selectTileX == x && selectTileY == y && confirmPath && UnitMovementController.getRouteSize() > 0){
					confirmMovement();
				}
				else{
					//Set tile selection
					selectTileX = x;
					selectTileY = y;

					GameUnit unit = (GameUnit) UnitPlacement[selectUnitX,selectUnitY].GetComponent("GameUnit");
					if (!unit.HasMoved)
						pathconfirming = UnitMovementController.selectPath(x,y,unit);
					if (!pathconfirming){ // no valid selection
						selectUnitX = selectTileX = -1;
						selectUnitY = selectTileY = -1;
						
						UnitMovementController.fullclear();
					}

				}
			}
			else{
				//Set tile selection
				selectTileX = x;
				selectTileY = y;
			}
		}


		confirmPath = pathconfirming;

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
