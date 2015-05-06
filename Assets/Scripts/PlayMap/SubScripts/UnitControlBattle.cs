using UnityEngine;
using System.Collections;

public class UnitControlBattle {
	public GameUnit Attacker;
	public GameUnit Defender;
	public float AttackerPenalty;
	public float DefenderPenalty;
	public TurretSpec TurretOfChoice;
	public TurretSpec TurretOfChoiceE = null;
	public bool RangedBattle = false;
	public int distance = 1;
	public GameUnit.UnitRotationStatus attackAngle = GameUnit.UnitRotationStatus.N;

	private UnitControlBattle(GameUnit Attacker, GameUnit Defender, int distance, 
	                          GameUnit.UnitRotationStatus attackAngle, 
	                          float atkpen, float defpen){
		this.Attacker = Attacker;
		this.Defender = Defender;
		this.distance = distance;
		this.attackAngle = attackAngle;
		this.AttackerPenalty = atkpen;
		this.DefenderPenalty = defpen;
		if (distance > 1)
			RangedBattle = true;
		selectWeapon ();
		selectWeaponE ();
	}

	public static UnitControlBattle CalculateBattle(GameUnit attacker, int ax, int ay, int dx, int dy){
		if (attacker != null && ((UnitControl) PlayMap.GridController).HasUnit (dx, dy)) {
			GameUnit defender = ((UnitControl) PlayMap.GridController).getUnitData (dx, dy);
			int xd = ax - dx;
			int yd = ay - dy;
			float atkpen = 1;
			float defpen = 1;
			if (attacker.terrainStatus != GameUnit.UnitTerrainStatus.Air && attacker.terrainStatus != GameUnit.UnitTerrainStatus.Deep) {
				defpen = 1.0f - PlayMap.Grid.getTileSpec (ax, ay).defense;
			}
			if (defender.terrainStatus != GameUnit.UnitTerrainStatus.Air && defender.terrainStatus != GameUnit.UnitTerrainStatus.Deep) {
				atkpen = 1.0f - PlayMap.Grid.getTileSpec (dx, dy).defense;
			}


			if (Mathf.Abs (xd) == Mathf.Abs (yd)) { // Diagonal attack
				GameUnit.UnitRotationStatus xr;
				GameUnit.UnitRotationStatus yr;
				if (xd < 0) { //Left to Right
					xr = GameUnit.UnitRotationStatus.E;
				} else { //Right to Left
					xr = GameUnit.UnitRotationStatus.W;
				}
				if (yd < 0) { //Down to Up
					yr = GameUnit.UnitRotationStatus.N;
				} else { //Up to Down
					yr = GameUnit.UnitRotationStatus.S;
				}

				if (xr == defender.UnitRotation || xr == GameUnit.OppositeRotationStatus (defender.UnitRotation)) {
					return new UnitControlBattle (attacker, defender, Mathf.Abs (xd) + Mathf.Abs (yd), yr, atkpen, defpen);
				} else {
					return new UnitControlBattle (attacker, defender, Mathf.Abs (xd) + Mathf.Abs (yd), xr, atkpen, defpen);
				}
			} else if (Mathf.Abs (xd) > Mathf.Abs (yd)) { //East or West
				if (xd < 0) { //Left to Right
					return new UnitControlBattle (attacker, defender, Mathf.Abs (xd) + Mathf.Abs (yd), GameUnit.UnitRotationStatus.E, atkpen, defpen);
				} else { //Right to Left
					return new UnitControlBattle (attacker, defender, Mathf.Abs (xd) + Mathf.Abs (yd), GameUnit.UnitRotationStatus.W, atkpen, defpen);
				}
			} else { //North or South
				if (yd < 0) { //Down to Up
					return new UnitControlBattle (attacker, defender, Mathf.Abs (xd) + Mathf.Abs (yd), GameUnit.UnitRotationStatus.N, atkpen, defpen);
				} else { //Up to Down
					return new UnitControlBattle (attacker, defender, Mathf.Abs (xd) + Mathf.Abs (yd), GameUnit.UnitRotationStatus.S, atkpen, defpen);
				}
			}
		}
		return null;
	}

	
	public void selectWeapon(){
		float bestDamage = 0;
		foreach (TurretSpec turret in Attacker.UnitBodySpec.turrets) {
			float newDamage;
			GunSpec weapon = turret.gunStat;
			if (canTarget(Attacker, Defender, distance, turret, weapon)){
				newDamage = estimateRawDamage(Defender.getArmor(
					GameUnit.OppositeRotationStatus(attackAngle)), weapon,
				    Defender.UnitBodySpec.getNumbers());
				if (newDamage > bestDamage){
					bestDamage = newDamage;
					TurretOfChoice = turret;
				}
			}
		}
		
	}

	public void selectWeaponE(){
		if (!RangedBattle && attackAngle != Defender.UnitRotation) { // No counter in ranged or back attacks
			float bestDamage = 0;
			foreach (TurretSpec turret in Defender.UnitBodySpec.turrets) {
				float newDamage;
				GunSpec weapon = turret.gunStat;
				if (Defender.UnitRotation == GameUnit.OppositeRotationStatus(attackAngle) || // Head on attack
				    turret.turnableIndex() >= 0){ // Or the turret is turnable
					if (weapon.counter && canTarget (Defender, Attacker, distance, turret, weapon)) {
						newDamage = estimateRawDamage (Attacker.getArmor (
							attackAngle, false), weapon,
						    Attacker.UnitBodySpec.getNumbers());
						if (newDamage > bestDamage){
							bestDamage = newDamage;
							TurretOfChoiceE = turret;
						}
					}
				}
			}
		}
	}

	public float AttackerDamage(){
		return AttackerPenalty * estimateRawDamage(Defender.getArmor(
			GameUnit.OppositeRotationStatus(attackAngle)), TurretOfChoice.gunStat,
		    Defender.UnitBodySpec.getNumbers());
	}
	
	public float CounterDamage(){
		if (TurretOfChoiceE != null) {
			return DefenderPenalty * estimateRawDamage (Attacker.getArmor (
				attackAngle, true), TurretOfChoiceE.gunStat,
			    Attacker.UnitBodySpec.getNumbers ());
		}
		else
			return 0;
	}
	
	
	public float AttackerPercentual100Damage(){
		float damage = AttackerDamage ();
		if (damage > 0)
			return Mathf.Floor((damage / Defender.UnitBodySpec.getMaxHealth ()) * 100.0f);
		return 0;
	}
	
	public float CounterPercentual100Damage(){
		float damage = CounterDamage ();
		if (damage > 0)
			return Mathf.Ceil((damage / Attacker.UnitBodySpec.getMaxHealth ()) * 100.0f);
		return 0;
	}

	public string AttackDirectionType(){
		if (attackAngle == Defender.UnitRotation)
			return "Back";
		else if (Defender.UnitRotation == GameUnit.OppositeRotationStatus (attackAngle))
			return "Front";
		else
			return "Flank";
	}
	
	public static bool canFire(GameUnit attacker, GunSpec weapon){
//		foreach (GameObject turret in attacker.UnitBodySpec.turrets) {
//			TurretSpec turretSpec = (TurretSpec)turret.GetComponent("TurretSpec");
//			GunSpec weapon = turretSpec.gunStat;
//			if (canTarget(attacker, defender, distance, turretSpec, weapon))
//				return true;
//		}
//		return false;
		return true;
	}

	public static bool canTarget(GameUnit attacker, GameUnit defender, int distance){
		foreach (TurretSpec turret in attacker.UnitBodySpec.turrets) {
			GunSpec weapon = turret.gunStat;
			if (canTarget(attacker, defender, distance, turret, weapon))
				return true;
		}
		return false;
	}

	public static bool canTarget(GameUnit attacker, GameUnit defender, int distance, TurretSpec turret, GunSpec weapon){

		//DIRECT ATTACKS
		if (distance <= 1) {
			if (!weapon.direct)
				return false;
			return canTargetDirectRules(attacker.terrainStatus, defender.terrainStatus, turret, weapon);
		}
		//LONG RANGE ATTTACKS
		else if (!attacker.HasMoved && weapon.minrange <= distance && weapon.maxrange >= distance) {
//			Debug.Log ("TESTU"); 
			// Homing weapons can target all except submerged, cannot be used while submerged
			if (weapon.type == GunSpec.GunType.Homing && 
			    attacker.terrainStatus != GameUnit.UnitTerrainStatus.Deep && 
			    defender.terrainStatus != GameUnit.UnitTerrainStatus.Deep)
				return true;
			else if (attacker.terrainStatus == GameUnit.UnitTerrainStatus.Air || // Attacker is flying
			         attacker.terrainStatus == GameUnit.UnitTerrainStatus.Deep || // Attacker is submerged
			         weapon.type == GunSpec.GunType.Torpedo){ // Torpedo weapons
				//use direct attack rules
				return canTargetDirectRules(attacker.terrainStatus, defender.terrainStatus, turret, weapon);
			}
			else if(turret.highDepression) { //Attacker is either grounded or on sea, turret need to have a good angle
				switch (weapon.type){
				case GunSpec.GunType.Direct: // Can only perform anti air against aerial units
					return defender.terrainStatus == GameUnit.UnitTerrainStatus.Air;
				case GunSpec.GunType.Heavy: // Can only perform attacks against non flying and non submerged units
					return defender.terrainStatus != GameUnit.UnitTerrainStatus.Air && defender.terrainStatus != GameUnit.UnitTerrainStatus.Deep;
				}
			}
		}
		return false;
	}

	public static bool canTargetDirectRules(GameUnit.UnitTerrainStatus attacker, GameUnit.UnitTerrainStatus defender, TurretSpec turret, GunSpec weapon){
		// Homing weapons can target all except submerged, cannot be used while submerged
		if (weapon.type == GunSpec.GunType.Homing && 
		    attacker != GameUnit.UnitTerrainStatus.Deep && 
		    defender != GameUnit.UnitTerrainStatus.Deep)
			return true;

		switch (attacker) {
		case GameUnit.UnitTerrainStatus.Air:
			switch(defender){
			case GameUnit.UnitTerrainStatus.Air: // A vs A
				return weapon.type == GunSpec.GunType.Direct;
			case GameUnit.UnitTerrainStatus.Deep: // A vs Sub
				return weapon.type == GunSpec.GunType.Drop || weapon.type == GunSpec.GunType.Torpedo;
			case GameUnit.UnitTerrainStatus.High: // A vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return true;
				case GunSpec.GunType.Drop:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.HighFloat: // A vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return true;
				case GunSpec.GunType.Drop:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Land: // A vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return true;
				case GunSpec.GunType.Drop:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Float: // A vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return true;
				case GunSpec.GunType.Drop:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Water: // A vs Sea
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return true;
				case GunSpec.GunType.Drop:
					return true;
				case GunSpec.GunType.Torpedo:
					return true;
				}
				break;
			}
			break;
			
		case GameUnit.UnitTerrainStatus.Deep:
			switch(defender){
			case GameUnit.UnitTerrainStatus.Deep:  // Sub vs Sub
				return weapon.type == GunSpec.GunType.Direct || weapon.type == GunSpec.GunType.Torpedo;
			case GameUnit.UnitTerrainStatus.Water: // Sub vs Sea
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Torpedo:
					return true;
				}
				break;
			}
			break;

			
			
			
		case GameUnit.UnitTerrainStatus.High:
			switch(defender){
			case GameUnit.UnitTerrainStatus.Air: // G vs A
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.High:  // GH vs GH
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.HighFloat:  // GH vs GH
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Land: // GH vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Float: // GH vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Water:   // G vs Sea
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			}
			break;
			
			
		case GameUnit.UnitTerrainStatus.Land:
			switch(defender){
			case GameUnit.UnitTerrainStatus.Air: // G vs A
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.High:  // G vs GH
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.HighFloat:  // G vs GH
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.Land: // G vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Float: // G vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Water:   // G vs Sea
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			}
			break;
		
		
		
		case GameUnit.UnitTerrainStatus.Water:
			switch(defender){
			case GameUnit.UnitTerrainStatus.Air:   // Sea vs A
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.High:  // Sea vs GH
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.HighFloat:  // Sea vs GH
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return turret.highDepression;
				case GunSpec.GunType.Heavy:
					return turret.highDepression;
				}
				break;
			case GameUnit.UnitTerrainStatus.Land: // Sea vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Float: // Sea vs G
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Water:  // Sea vs Sea
				switch (weapon.type){
				case GunSpec.GunType.Direct:
					return true;
				case GunSpec.GunType.Heavy:
					return true;
				case GunSpec.GunType.Drop:
					return true;
				case GunSpec.GunType.Torpedo:
					return true;
				}
				break;
			case GameUnit.UnitTerrainStatus.Deep:  // Sea vs Sea
				switch (weapon.type){
				case GunSpec.GunType.Drop:
					return true;
				case GunSpec.GunType.Torpedo:
					return true;
				}
				break;
			}
			break;
		}


		return false;
	}


	public static float estimateRawDamage(float armor, GunSpec weapon, int unitCount){
		float damage = weapon.absolute;
		float barrageBonus = 1;
		if (weapon.Turret.totalBarrage <= unitCount) {
			barrageBonus = weapon.Turret.totalBarrage;
		}
		else {
			float overBonus = Mathf.Log10(50.0f * (float)weapon.Turret.totalBarrage / (float) unitCount) / 4;
			if (overBonus > 1)
				overBonus = 1;
			barrageBonus = (float) unitCount * (1.0f + overBonus);
			//Debug.Log("Barrage OVER: " + barrageBonus + "In " + weapon.Turret.totalBarrage + " vs " + unitCount);
		}
		if (armor < weapon.damage)
			damage += weapon.damage - Mathf.Max(0, armor - weapon.pierce);
		return damage * weapon.burstsize * barrageBonus;
	}


}
