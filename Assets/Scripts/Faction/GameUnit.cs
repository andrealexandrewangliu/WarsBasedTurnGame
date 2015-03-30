using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour {
	public GameObject unitPrefab;
	public GameObject UnitBody;
	private UnitSpec UnitBodySpec;
	public GameObject Faction;
	private PlayerFaction playerFactionSpec;
	public UnitTerrainStatus terrainStatus;
	public UnitRotationStatus UnitRotation = UnitRotationStatus.N;
	public UnitRotationStatus TurretRotation = UnitRotationStatus.N;
	private static float UnitSpeed = 10.0f;
	public UnitRotationStatus confirmRotation = UnitRotationStatus.N;
	private bool rotationConfirmed = true;
	private ArrayList Waypoint = new ArrayList();
	private ArrayList WaypointD = new ArrayList();


	public enum UnitTerrainStatus{
		Land,
		Air,
		Water,
		Deep
	};
	public enum UnitRotationStatus{
		N, // North
		S, // South
		E, // East
		W  // West
	};

	public int getFactionId(){
		return playerFactionSpec.ID;
	}

	public static void setUnitSpeed(float speed){
		UnitSpeed = speed;
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
				
				Debug.Log(prechangey);
				Debug.Log(postchangey);

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
		switch (direction) {
		case UnitControlMovement.OriginDirection.W:
			WaypointD.Add (UnitRotationStatus.E);
			break;
		case UnitControlMovement.OriginDirection.S:
			WaypointD.Add (UnitRotationStatus.N);
			break;
		case UnitControlMovement.OriginDirection.N:
			WaypointD.Add (UnitRotationStatus.S);
			break;
		case UnitControlMovement.OriginDirection.E:
			WaypointD.Add (UnitRotationStatus.W);
			break;
		default:
			if (WaypointD.Count>0)
				WaypointD.Add (WaypointD[WaypointD.Count-1]);
			else
				WaypointD.Add (UnitRotation);
			break;
		}
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
	}
	
	void SetRotation(){
		UnitBodySpec.RotateBody (getAngle (UnitRotation));
		if (!UnitBodySpec.RotateTurret ((360 - getAngle (UnitRotation)) + getAngle (TurretRotation))) {
			TurretRotation = UnitRotation;
		}
	}
	
	void SetRotation(UnitRotationStatus rotation){
		UnitBodySpec.RotateBody (getAngle (rotation));
		UnitBodySpec.RotateTurret ((360 - getAngle (rotation)) + getAngle (rotation));
	}

	void ConfirmMovement(){
		UnitRotation = TurretRotation = confirmRotation;
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

	public static GameObject InstantiateUnit(GameObject prefabUnit, GameObject f){
		GameObject unit = (GameObject)Instantiate (Resources.Load("Prefabs/Units/Unit") as GameObject);
		GameUnit unitData = (GameUnit) unit.GetComponent ("GameUnit");
		unitData.unitPrefab = prefabUnit;
		unitData.Faction = f;
		return unit;
	}
}
