using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour {
	private GridControl GridControlScript;
	public GameObject Factions;
	private GameObject[,] UnitPlacement;
	public int selectTileX = -1;
	public int selectTileY = -1;


	// Use this for initialization
	void Start () {
		GridControlScript = (GridControl)this.GetComponent ("GridControl");
		UnitPlacement = new GameObject[(int)GridControlScript.MapSize.x,(int)GridControlScript.MapSize.y];
		SpawnUnit (1, 1, 0, 0);
		Move (1, 1, 1, 2);
	}

	void Move(int ox, int oy, int tx, int ty){
		var unit = UnitPlacement [ox, oy];
		UnitPlacement [ox, oy] = null;
		UnitPlacement [tx, ty] = unit;
		unit.transform.position = new Vector3 (tx + 0.5f, 0.2f, ty + 0.5f);
	}

	void SpawnUnit(int x, int y, int faction, int unitid){
		GameObject prefab = SelectUnitPrefab (faction, unitid);
		GameObject playerFaction = Factions.transform.GetChild(faction).gameObject;
		PlayerFaction playerFactionSpec = (PlayerFaction)playerFaction.GetComponent ("PlayerFaction");

		//var unit = (GameObject)Instantiate (prefab, new Vector3(x + 0.5f, 0.2f, y + 0.5f), Quaternion.identity);
		var unit = GameUnit.InstantiateUnit (prefab, playerFaction);
		unit.transform.position = new Vector3 (x + 0.5f, 0, y + 0.5f);
		unit.transform.parent = this.transform.FindChild("Units").transform;
		playerFactionSpec.ActiveUnits.Add (unit);
		UnitPlacement [x, y] = unit;
		);
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

	public void clickCoordinate(int x, int y){
		if (selectTileX < 0) {
			selectTileX = x;
			selectTileY = y;
		} else {
			if (UnitPlacement[selectTileX,selectTileY] != null){
				if (selectTileX != x || selectTileY != y)
					Move(selectTileX,selectTileY,x,y);
				selectTileX = -1;
				selectTileY = -1;
			}
			else{
				selectTileX = x;
				selectTileY = y;
			}
		}
		Debug.Log(string.Format("clickCoordinate: {0} selection: {1}", new Vector2 (x, y), new Vector2 (selectTileX, selectTileY)));
	}


	
	// Update is called once per frame
	void Update () {
	}
}
