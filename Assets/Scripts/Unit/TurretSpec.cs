using UnityEngine;
using System.Collections;

public class TurretSpec : TurretableSpec {
	public bool turnable = false;
	public float frontArmor = 0;
	public float sideArmor = 0;
	public float rearArmor = 0;
	public float health = 0;
	public float weigth = 0;
	public int supply = 0; // Supply Bonus

	public int totalBarrage;

	public bool highDepression = false;
	public GameObject gun;
	public GunSpec gunStat;
	public UnitSpec.UnitSize turretSize;
	public UnitSpec.UnitSize gunSize;
	public int TurretIndex;
	
	public override float TurretArmorInfluence (){
		return 0.25f;
	}

	// Use this for initialization
	void Start () {
	}
	
	public int gunCount (){
		return gameObject.transform.FindChild ("Cannons").childCount;
	}

	public int turnableIndex(){
		if (turnable) // Index is the current turret is 0
			return 0;
		else if(parent is TurretSpec){
			TurretSpec parentTurret = (TurretSpec) parent;
			int turretIndex = parentTurret.turnableIndex ();
			if (turretIndex >=0)
				return turretIndex + 1; // Index is at least the previous turnable index plus 1 (needs to descend atleast the parent's index + 1
		}
		return -1;
	}

	
	public float getMaxHealth(){
		return health + getTurretsHealth ();
	}
	
	public int getMaxSupply(){
		return supply + getTurretsSupply ();
	}

	public int getMaxAmmo(){
		if (gunStat != null)
			return gunStat.ammo;
		return 0;
	}
	
	public float[] getArmor(){
		float [] armors = getTurretsArmor();
		if (armors [0] > 0) {
			float influence = TurretArmorInfluence ();
			armors [0] *= influence;
			armors [1] *= influence;
			armors [2] *= influence;
			armors [0] += frontArmor * (1.0f - influence);
			armors [1] += sideArmor * (1.0f - influence);
			armors [2] += rearArmor * (1.0f - influence);
		} 
		else {
			armors [0] = frontArmor;
			armors [1] = sideArmor;
			armors [2] = rearArmor;
		}
		return armors;
	}


	
	public float getWeight(){
		float calcweigth = weigth;
		if (gunStat != null) {
			calcweigth += gunStat.getWeight();
		}
		calcweigth += getTurretsWeights();
		return calcweigth;
	}

	public void paint(Color color){
		((UnitPainter)this.GetComponent ("UnitPainter")).paint(color);
		gunStat.paint (color);
		paintTurrets (color);
	}

	public int GenerateBody(GameObject mainPrefab, GameObject[] subPrefab, int index){
		if (mainPrefab != null) {
			GameObject cannons = this.transform.FindChild ("Cannons").gameObject;
			int gunCount = cannons.transform.childCount;
			for (int i = 0; i < gunCount; i++){
				gun = (GameObject)Instantiate (mainPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
				gun.transform.parent = cannons.transform.GetChild(i).FindChild("Extension").transform;
				gun.transform.position = gun.transform.parent.transform.position;
				GunSpec wspec = (GunSpec)gun.GetComponent ("GunSpec");
				wspec.Turret = this;
				//gun.transform.localEulerAngles = new Vector3(0,0,0);
				if (gunStat == null)
					gunStat = wspec;
			}
			TurretIndex = index;
		}
		index = GenerateGuns (subPrefab, index);

		return index;
	}
}
