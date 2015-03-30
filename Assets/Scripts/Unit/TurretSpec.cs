using UnityEngine;
using System.Collections;

public class TurretSpec : MonoBehaviour {
	public bool turnable = false;
	public float frontArmor = 0;
	public float sideArmor = 0;
	public float rearArmor = 0;
	public float health = 0;
	public float weigth = 0;
	public bool highDepression = false;
	public bool hasSub = false;
	public GameObject subWeapon = null;
	public GameObject gun;
	private GunSpec gunStat;
	private TurretSpec subStat;
	public UnitSpec.UnitSize turretSize;
	public UnitSpec.UnitSize gunSize;

	// Use this for initialization
	void Start () {
	}
	
	
	public float getWeight(){
		float calcweigth = weigth;
		if (gunStat != null) {
			calcweigth += gunStat.getWeight();
		}
		if (subStat != null) {
			calcweigth += subStat.getWeight();
		}
		return calcweigth;
	}

	public void paint(Color color){
		((UnitPainter)this.GetComponent ("UnitPainter")).paint(color);
		gunStat.paint (color);
		if(subStat != null)
			subStat.paint (color);
	}

	public void GenerateBody(GameObject mainPrefab, GameObject subPrefab){
		gun = (GameObject)Instantiate (mainPrefab, new Vector3(0,0,0), Quaternion.identity);
		gun.transform.parent = this.transform.FindChild("GunHardpoint").transform;
		gun.transform.position = gun.transform.parent.transform.position;
		gunStat = (GunSpec) gun.GetComponent ("GunSpec");
		if (hasSub && subPrefab != null) {
			subWeapon = this.transform.FindChild ("SubHardpoint").gameObject;
			subStat = (TurretSpec) subWeapon.GetComponent ("TurretSpec");
			subStat.GenerateBody(subPrefab, null);
		}
	}
}
