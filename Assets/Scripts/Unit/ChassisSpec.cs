using UnityEngine;
using System.Collections;

public class ChassisSpec : MonoBehaviour {
	public float frontArmor = 0;
	public float sideArmor = 0;
	public float rearArmor = 0;
	public float health = 0;
	public float weigth = 0;
	public float horsepower = 0;
	public GameObject turret;
	private TurretSpec turretStat;
	public UnitSpec.UnitType type = UnitSpec.UnitType.Drone;
	public UnitSpec.UnitSize size;
	public UnitSpec.UnitSize turretSize;

	// Use this for initialization
	void Start () {
	
	}
	
	public float getWeight(){
		float calcweigth = weigth;
		if (turretStat != null) {
			calcweigth += turretStat.getWeight();
		}
		return calcweigth;
	}
	
	public float getPowerWeightRatio(){
		float calcratio = horsepower / getWeight();

		return calcratio;
	}

	// Update is called once per frame
	void Update () {
	
	}

	
	public void paint(Color color){
		((UnitPainter)this.GetComponent ("UnitPainter")).paint(color);
		turretStat.paint (color);
	}
	
	public void GenerateBody(GameObject turretPrefab, GameObject mainPrefab, GameObject subPrefab){
		turret = (GameObject)Instantiate (turretPrefab, new Vector3(0,0,0), Quaternion.identity);
		turret.transform.parent = this.transform.FindChild("TurretHardpoint").transform;
		turret.transform.position = turret.transform.parent.transform.position;
		turretStat = (TurretSpec) turret.GetComponent ("TurretSpec");
		turretStat.GenerateBody (mainPrefab, subPrefab);
	}
	
	public bool RotateTurret(float angle){
		if (turretStat.turnable) 
			transform.FindChild ("TurretHardpoint").transform.localEulerAngles = new Vector3 (0, angle, 0);
		return turretStat.turnable;
	}
}
