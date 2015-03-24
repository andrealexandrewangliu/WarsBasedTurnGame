using UnityEngine;
using System.Collections;

public class UnitSpec : MonoBehaviour {
	public GameObject chassisPrefab;	// ChassisWeapon prefab
	public GameObject turretPrefab;		// TurretWeapon prefab
	public GameObject mainPrefab;		// MainWeapon prefab	
	public GameObject subPrefab; 		// SubWeapon prefab
	private GameObject chassis;	// ChassisWeapon prefab
	private GameObject turret;		// TurretWeapon prefab
	private GameObject main;		// MainWeapon prefab	
	private GameObject sub; 		// SubWeapon prefab
	private ChassisSpec chassisStat; 		// SubWeapon prefab
	private Color UnitColor;
	
	public enum UnitTerrainStatus{
		Land,
		Air,
		Water,
		Deep
	};
	public UnitTerrainStatus terrainStatus;

	
	public float getWeight(){
		float calcweigth = 0;
		if (chassisStat != null) {
			calcweigth += chassisStat.getWeight();
		}
		return calcweigth;
	}
	
	public float getPowerWeightRatio(){
		float calcratio = 0;
		if (chassisStat != null) {
			calcratio += chassisStat.getPowerWeightRatio();
		}
		return calcratio;
	}
	
	public int getBaseMP(){
		int mp = Mathf.RoundToInt(chassisStat.getPowerWeightRatio() / 2);
		if (mp <= 0)
			mp = 1;
		return mp;
	}

	// Use this for initialization
	void Start () {
		GenerateBody ();
//		Debug.Log (getWeight ());
//		Debug.Log (getBaseMP ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void paintUnit(Color color){
		UnitColor = color;
		if (chassisStat != null)
			chassisStat.paint (UnitColor);
	}

	void GenerateBody(){
		chassis = (GameObject)Instantiate (chassisPrefab, new Vector3(0,0,0), Quaternion.identity);
		chassis.transform.parent = this.transform;
		chassis.transform.position = chassis.transform.parent.transform.position;
		chassisStat = (ChassisSpec) chassis.GetComponent ("ChassisSpec");
		chassisStat.GenerateBody (turretPrefab, mainPrefab, subPrefab);
		chassisStat.paint (UnitColor);
	}
}
