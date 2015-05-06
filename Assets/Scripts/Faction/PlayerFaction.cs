using UnityEngine;
using System.Collections;

public class PlayerFaction : MonoBehaviour {
	public Color FactionColor;
	public GameObject BaseFactionNation;
	public BaseFaction BaseFactionSpecs;
	public string Name;
	public ArrayList ActiveUnits = new ArrayList();
	public GameObject[] Units = new GameObject[5];
	public int ID = 0;
	public int[] allies;

	// Use this for initialization
	void Start () {
	}

	public void DebugPrintUnits(){
		Debug.Log("Faction:" + ID + " Unit Count:" + ActiveUnits.Count);
		foreach (GameObject unit in ActiveUnits) {
			GameUnit unitData = (GameUnit) unit.GetComponent("GameUnit");
			Debug.Log("Faction:" + ID + " Unit:" + unitData.UnitBodySpec.name);
		}
	}
	
	public void WakeAllUnits(){
		foreach (GameObject unit in ActiveUnits) {
			GameUnit unitData = (GameUnit) unit.GetComponent("GameUnit");
			unitData.HasMoved = false;
		}
	}
	
	public void SleepAllUnits(){
		foreach (GameObject unit in ActiveUnits) {
			GameUnit unitData = (GameUnit) unit.GetComponent("GameUnit");
			unitData.EndMovement();
		}
	}


	public GameObject getUnitPrefab (int id){
		if (id > BaseFactionSpecs.Units.Length) {
			id -= BaseFactionSpecs.Units.Length;
			if (id > Units.Length)
				return null;
			return Units[id];
		}
		return BaseFactionSpecs.Units [id];
	}


	public bool isAlliedWith(PlayerFaction player){
		if (player.ID == ID)
			return true;
		if (allies != null && allies.Length > 0){
			foreach (int allyId in allies){
				if (allyId == player.ID)
					return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
