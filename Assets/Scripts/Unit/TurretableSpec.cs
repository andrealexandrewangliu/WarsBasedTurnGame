using UnityEngine;
using System.Collections;

public abstract class TurretableSpec : MonoBehaviour {
	public GameObject[] TurretSets;
	public TurretSpec[] TurretMainSpec;
	public TurretableSpec parent;

	public abstract float TurretArmorInfluence ();
	
	public void addTurretsGameobjects(ArrayList objects){
		foreach (TurretSpec main in TurretMainSpec) {
			objects.Add(main);
			main.addTurretsGameobjects(objects);
		}
	}
	
	public float getTurretsWeights(){
		float total = 0;
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			if (TurretMainSpec[i] != null){
				total += TurretMainSpec[i].getWeight() * TurretSets[i].transform.childCount;
			}
		}
		return total;
	}
	
	public float getTurretsHealth(){
		float total = 0;
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			if (TurretMainSpec[i] != null){
				total += TurretMainSpec[i].getMaxHealth() * TurretSets[i].transform.childCount;
			}
		}
		return total;
	}
	
	public int getTurretsSupply(){
		int total = 0;
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			if (TurretMainSpec[i] != null){
				total += TurretMainSpec[i].getMaxSupply() * TurretSets[i].transform.childCount;
			}
		}
		return total;
	}
	
	public float[] getTurretsArmor(){
		float[] total = {0,0,0};
		int count = 0;
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			if (TurretMainSpec[i] != null){
				float[] armor = TurretMainSpec[i].getArmor ();
				if (armor[0] > 0){
					total[0] += armor[0];
					total[1] += armor[1];
					total[2] += armor[2];
					count ++;
				}
			}
		}
		if (count > 0) {
			total [0] /= count;
			total [1] /= count;
			total [2] /= count;
		}
		else {
			total[0] = -1;
			total[1] = -1;
			total[2] = -1;
		}
		return total;
	}

	public void paintTurrets(Color color){
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			if (TurretMainSpec[i] != null){
				int childCount = TurretSets[i].transform.childCount;
				for (int j = 0; j < childCount; j++){
					GameObject turretObject = TurretSets[i].transform.GetChild(j).GetChild(0).gameObject;
					if (turretObject != null){
						TurretSpec spec = (TurretSpec) turretObject.GetComponent("TurretSpec");
						UnitPainter painter = (UnitPainter) turretObject.GetComponent("UnitPainter");
						painter.paint(color);
						spec.paint(color);
					}
				}
			}
		}

	}

	public int GenerateTurrets(GameObject[] turretsPrefab, int index, int gunCount){
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			int childCount = TurretSets[i].transform.childCount;
			if (TurretMainSpec[i] == null){ // Generate only for specless turret sets
				int cindex = index;
				for (int j = 0; j < childCount; j++){
					GameObject turretHardpointObject = TurretSets[i].transform.GetChild(j).gameObject;
					GameObject turret = (GameObject)Instantiate (turretsPrefab[index], new Vector3(0,0,0), Quaternion.identity);
					turret.transform.parent = turretHardpointObject.transform;
					turret.transform.position = turret.transform.parent.transform.position;
					TurretSpec turretStat = (TurretSpec) turret.GetComponent ("TurretSpec");
					turretStat.totalBarrage = gunCount * turretStat.gunCount ();
					if (TurretMainSpec[i] == null)
						TurretMainSpec[i] = turretStat;
					turretStat.parent = this;
					cindex = turretStat.GenerateTurrets (turretsPrefab, index + 1, gunCount * childCount);
				}
				index = cindex;
			}
			else{
				TurretMainSpec[i].totalBarrage = gunCount * TurretMainSpec[i].gunCount () * childCount;
			}
		}



		return index;
	}
	
	public int GenerateGuns(GameObject[] gunsPrefab, int index){
		for (int i = 0; i < TurretMainSpec.Length; i++) {
			if (TurretMainSpec[i] != null){
				int childCount = TurretSets[i].transform.childCount;
				for (int j = 0; j < childCount; j++){
					GameObject turretObject = TurretSets[i].transform.GetChild(j).GetChild(0).gameObject;
					int cindex = index;
					if (turretObject != null){
						TurretSpec spec = (TurretSpec) turretObject.GetComponent("TurretSpec");
						cindex = spec.GenerateBody(gunsPrefab[index], gunsPrefab, index + 1);

					}
					index = cindex;
				}
			}
		}
		return index;
		
	}
}
