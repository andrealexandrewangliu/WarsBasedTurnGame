using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour {
	public GameObject unitPrefab;
	public GameObject UnitBody;
	public GameObject Faction;
	private PlayerFaction playerFactionSpec;

	// Use this for initialization
	void Start () {
		playerFactionSpec = (PlayerFaction)Faction.GetComponent ("PlayerFaction");
		UnitBody = (GameObject)Instantiate (unitPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity);
		((UnitSpec)UnitBody.GetComponent ("UnitSpec")).paintUnit(playerFactionSpec.FactionColor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static GameObject InstantiateUnit(GameObject prefabUnit, GameObject f){
		GameObject unit = (GameObject)Instantiate (Resources.Load<GameObject>("Prefabs/Units/Unit.prefab"));
		GameUnit unitData = (GameUnit) unit.GetComponent ("GameUnit");
		unitData.unitPrefab = prefabUnit;
		unitData.Faction = f;
		return unit;
	}
}
