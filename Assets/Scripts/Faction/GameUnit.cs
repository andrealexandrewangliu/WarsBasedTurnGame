using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour {
	public GameObject unitPrefab;
	public GameObject UnitBody;
	public UnitSpec UnitBodySpec;
	public GameObject Faction;
	public bool HasMoved = false;
	public PlayerFaction playerFactionSpec;
	public UnitTerrainStatus terrainStatus = UnitTerrainStatus.Land;
	public UnitRotationStatus UnitRotation = UnitRotationStatus.N;
	public UnitRotationStatus TurretRotation = UnitRotationStatus.N;
	private static float UnitSpeed = 10.0f;
	public UnitRotationStatus confirmRotation = UnitRotationStatus.N;
	public UnityEngine.UI.Text HealthLabel;
	private bool rotationConfirmed = true;
	private ArrayList Waypoint = new ArrayList();
	private ArrayList WaypointD = new ArrayList();
	public int x, y;

	public float health;
	public int[] ammo;
	public int supply = 100;


	public enum UnitTerrainStatus{
		Land,  //Unit is touching the ground
		Float, //Unit is floating above ground or water
		High,  //Unit is touching the ground on high altitude
		HighFloat, //Unit is floating above ground or water on high altitude
		Air,   //Unit is flying
		Water, //Unit is within water surface
		Deep   //Unit is deep underwater
	};



	public enum UnitRotationStatus{
		N, // North
		S, // South
		E, // East
		W  // West
	};
	
	public static UnitRotationStatus OppositeRotationStatus(UnitControlMovement.OriginDirection rotation){
		switch (rotation) {
		case UnitControlMovement.OriginDirection.E:
			return UnitRotationStatus.W;
		case UnitControlMovement.OriginDirection.W:
			return UnitRotationStatus.E;
		case UnitControlMovement.OriginDirection.S:
			return UnitRotationStatus.N;
		case UnitControlMovement.OriginDirection.N:
			return UnitRotationStatus.S;
		}

		return (UnitRotationStatus)rotation;
	}

	public static UnitRotationStatus OppositeRotationStatus(UnitRotationStatus rotation){
		switch (rotation) {
		case UnitRotationStatus.E:
			return UnitRotationStatus.W;
		case UnitRotationStatus.W:
			return UnitRotationStatus.E;
		case UnitRotationStatus.S:
			return UnitRotationStatus.N;
		case UnitRotationStatus.N:
			return UnitRotationStatus.S;
		}


		return rotation;
	}
	
	public float getPercentual100HP (){
		return Mathf.Ceil ((health / UnitBodySpec.getMaxHealth ()) * 100.0f);
	}
	public float getPercentual100SP (){
		return Mathf.Ceil (((float)supply / (float) UnitBodySpec.getMaxSupply ()) * 100.0f);
	}

	public int getArmorIndex(UnitRotationStatus flank, UnitRotationStatus rotation){
		if (flank == rotation)
			return 0;
		else if (flank == OppositeRotationStatus(rotation))
			return 2;
		return 1;
	}
	
	public float getArmor(UnitRotationStatus flank, bool frontOnly){
		if (frontOnly) {
			int chassisArmorIndex = 0;
			int turretArmorIndex = 0;
			float totalDefense = 0;
			float[] armor;
		
			armor = UnitBodySpec.getArmor ();
			if (armor != null)
				totalDefense += armor [chassisArmorIndex];
		
			return totalDefense;
		}
		return getArmor (flank);
	}

	public float getArmor(UnitRotationStatus flank){
		int chassisArmorIndex = getArmorIndex (flank, UnitRotation);
		float totalDefense = 0;
		float[] armor;
		
		armor = UnitBodySpec.getArmor ();
		if (armor != null)
			totalDefense += armor [chassisArmorIndex];

		return totalDefense;
	}

	public int getFactionId(){
		return playerFactionSpec.ID;
	}

	public static void setUnitSpeed(float speed){
		UnitSpeed = speed;
	}
	public void addWaypoint(Vector2 movement, UnitControlMovement.OriginDirection direction){
		int x = (int) movement.x;
		int y = (int) movement.y;
		TileSelect tile = PlayMap.Grid.getTileSpec (x, y);
		float height = tile.FloorHeight;
		bool terrainDependent = true;

		if (UnitBodySpec.getUnitMovementType () == UnitSpec.UnitMove.Fly || 
			UnitBodySpec.getUnitMovementType () == UnitSpec.UnitMove.Sail || 
			UnitBodySpec.getUnitMovementType () == UnitSpec.UnitMove.Dive || 
			UnitBodySpec.getUnitMovementType () == UnitSpec.UnitMove.Heli)
			terrainDependent = false;
		
		if (terrainDependent) {
			// Hover units
			if (UnitBodySpec.getUnitMovementType () == UnitSpec.UnitMove.Float){
				switch (tile.FloorType) {
				case TileSelect.TileMovementType.Mountain:
					terrainStatus = UnitTerrainStatus.HighFloat;
					break;
				default:
					terrainStatus = UnitTerrainStatus.Float;
					break;
				}
				if (height < 0.2f)
					height = 0.25f;
				else
					height += 0.05f;
			}// Ground units
			else{ 
				switch (tile.FloorType) {
				case TileSelect.TileMovementType.Concrete:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.DeepWater:
					terrainStatus = UnitTerrainStatus.Water;
					break;
				case TileSelect.TileMovementType.Hill:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.Normal:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.Obstructed:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.Sand:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.DeepBridge:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.ShallowBridge:
					terrainStatus = UnitTerrainStatus.Land;
					break;
				case TileSelect.TileMovementType.ShallowWater:
					terrainStatus = UnitTerrainStatus.Water;
					break;
				case TileSelect.TileMovementType.Mountain:
					terrainStatus = UnitTerrainStatus.High;
					break;
				}
			}
		} 
		else {// Sea and Air units
			switch (terrainStatus) {
			case UnitTerrainStatus.Air:
				height = 0.5f;
				break;
			case UnitTerrainStatus.Deep:
				height = 0.15f;
				break;
			case UnitTerrainStatus.Water:
				height = 0.2f;
				break;
			}
		}
		
		addWaypoint (new Vector3 (movement.x + 0.5f, height, movement.y + 0.5f), direction);
	}

	public void addWaypoint(Vector3 movement, UnitControlMovement.OriginDirection direction){
		if (Waypoint.Count >= 1) {
			Vector3 lastWaypoint = (Vector3) Waypoint [Waypoint.Count - 1];
			if (lastWaypoint.y != movement.y){
				Vector3 travelchange = (movement - lastWaypoint).normalized * 0.4f;
				Vector3 prechangey = lastWaypoint + travelchange;
				Vector3 postchangey = movement - travelchange;
				Waypoint.Add (prechangey);
				Waypoint.Add (postchangey);
				Waypoint.Add (movement);
				
//				Debug.Log(prechangey);
//				Debug.Log(postchangey);

				addWaypointD (direction);
				addWaypointD (direction);
				addWaypointD (direction);
			}  else{
				Waypoint.Add (movement);
				addWaypointD (direction);
			}
		} else {
			Waypoint.Add (movement);
			addWaypointD (direction);
		}
	}

	private void addWaypointD(UnitControlMovement.OriginDirection direction){

		WaypointD.Add (OppositeRotationStatus (direction));
	}

	private float getAngle(UnitRotationStatus rotation){
		switch (rotation) {
		case UnitRotationStatus.N:
			return 270;
		case UnitRotationStatus.S:
			return 90;
		case UnitRotationStatus.E:
			return 0;
		case UnitRotationStatus.W:
			return 180;
		}
		return 0;
	}

	// Use this for initialization
	void Start () {
		playerFactionSpec = (PlayerFaction)Faction.GetComponent ("PlayerFaction");
		UnitBody = (GameObject)Instantiate (unitPrefab);
		UnitBodySpec = ((UnitSpec)UnitBody.GetComponent ("UnitSpec"));
		UnitBodySpec.paintUnit(playerFactionSpec.FactionColor);
		UnitBodySpec.GenerateBody ();
		UnitBody.transform.parent = this.transform;
		UnitBody.transform.position = this.transform.position;
		switch (UnitBodySpec.getUnitClassSize ()) {
		case UnitSpec.UnitSize.M:
			UnitBody.transform.localScale = new Vector3(3.5f,3.5f,3.5f);
			break;
		default:
			break;
		}
		SetRotation ();
		health = UnitBodySpec.getMaxHealth ();
		supply = UnitBodySpec.getMaxSupply ();
		ammo = new int[UnitBodySpec.turrets.Length];
		for (int i = 0; i < ammo.Length; i++) {

			ammo[i] = ((TurretSpec)UnitBodySpec.turrets[i].GetComponent("TurretSpec")).getMaxAmmo();

		}
	}
	
	public void SetRotation(){
		UnitBodySpec.RotateBody (getAngle (UnitRotation));
		if (!UnitBodySpec.RotateTurret ((360 - getAngle (UnitRotation)) + getAngle (TurretRotation))) {
			TurretRotation = UnitRotation;
		}
	}
	
	void SetRotation(UnitRotationStatus rotation){
		UnitBodySpec.RotateBody (getAngle (rotation));
		UnitBodySpec.RotateTurret ((360 - getAngle (rotation)) + getAngle (rotation));
	}
	
	public void ConfirmMovement(){
		UnitRotation = TurretRotation = confirmRotation;
		rotationConfirmed = true;
		EndMovement();
	}
	
	public void CancelMovement(){
		SetRotation ();
		rotationConfirmed = true;
	}
	
	public int getMovement(){
		return UnitBodySpec.getBaseMP ();
	}
	
	public UnitSpec.UnitMove getMovementType(){
		return UnitBodySpec.getUnitMovementType ();
	}

	// Update is called once per frame
	void Update () {
		if (Waypoint.Count > 0) {
			Vector3 destination = (Vector3)Waypoint[0];
			UnitRotationStatus rotation = (UnitRotationStatus) WaypointD[0];
			float border = UnitSpeed / 50;
			if (Vector3.Distance (destination, transform.position) > border) {
				transform.position += (destination - transform.position).normalized * UnitSpeed * Time.deltaTime;
				if (confirmRotation != rotation || rotationConfirmed){
					SetRotation(rotation);
					confirmRotation = rotation;
					rotationConfirmed = false;
				}
				if (Vector3.Distance (destination, transform.position) <= border) {
					transform.position = destination;
					Waypoint.RemoveAt(0);
					WaypointD.RemoveAt(0);
				}
			}
		}
	}

	public void UpdateUnit(){
		HealthLabel.text = ((int)getPercentual100HP()).ToString();
	}

	public void kill(){
		playerFactionSpec.ActiveUnits.Remove (gameObject);
		gameObject.SetActive (false);
	}

	public static GameObject InstantiateUnit(GameObject prefabUnit, GameObject f){
		GameObject unit = (GameObject)Instantiate (Resources.Load("Prefabs/Units/Unit") as GameObject);
		GameUnit unitData = (GameUnit) unit.GetComponent ("GameUnit");
		unitData.unitPrefab = prefabUnit;
		unitData.Faction = f;
		PlayerFaction faction = (PlayerFaction) unitData.Faction.GetComponent ("PlayerFaction");
		unitData.playerFactionSpec = faction;
		faction.ActiveUnits.Add (unit);
		
		UnityEngine.UI.Image UIPanel = (UnityEngine.UI.Image) unit.transform.FindChild("UI").GetChild(0).GetChild(0).gameObject.GetComponent("Image");
		Color panelColor = faction.FactionColor * Menus.ColorLim;
		panelColor.a = 1;
		UIPanel.color = panelColor;

		return unit;
	}

	public void EndMovement(){
		if (!HasMoved) {
			TileSelect tile = PlayMap.Grid.getTileSpec (x, y);
			tile.capture (this);
			HasMoved = true;
		}
	}
}
