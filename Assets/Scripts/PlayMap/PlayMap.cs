using UnityEngine;
using System.Collections;

public class PlayMap {
	public static GridControl Grid;
	public static byte[,] MapTerrain;
	public static GameObject[,] MapTiles;
	public static GameObject[,] UnitPlacement;
	public static GameObject UnitMenu;
	public static GameObject AttackInfo;
	public static Menus Menu;
	
	public static GridInteractionController GridController;

	public static bool LockMovement = false;

	public static void MoveTargetCamera(int x, int y){
		if (Menu != null) {
			Menu.TargetCamera.transform.position = new Vector3 (x + 0.5f, Menu.TargetCamera.transform.position.y, y + 0.5f);
		}
	}

	public static void HideAttackInfo(){
		AttackInfo.SetActive (false);
	}

	public static PlayerFaction getFaction(int id){
		if (GridController.Factions.transform.childCount > id && id>=0)
			return (PlayerFaction) GridController.Factions.transform.GetChild(id).gameObject.GetComponent("PlayerFaction");

		return null;
	}

	public static void ShowAttackInfo(int x, int y){
		if (GridController is UnitControl) {
			TileSelect tile = Grid.getTileSpec (x, y);
			bool highlight, cursor;

			PlayMap.AttackInfo.SetActive (true);

			Menu.DefenderImage.UpdateUnitImage ();

			AttackInfo attackInformation = (AttackInfo)PlayMap.AttackInfo.GetComponent ("AttackInfo");
			UnitControlBattle UnitBattleController = ((UnitControl) GridController).getBattleController ();
			float attack = UnitBattleController.AttackerPercentual100Damage ();
			float counter = UnitBattleController.CounterPercentual100Damage ();
			float attackerHp = UnitBattleController.Attacker.getPercentual100HP ();
			float defenderHp = UnitBattleController.Defender.getPercentual100HP ();
			attack *= attackerHp / 100.0f;
			float edefenderHp = defenderHp - attack;
			if (edefenderHp < 0)
				edefenderHp = 0;
			counter *= edefenderHp / 100.0f;
			float eattackerHp = attackerHp - counter;
			if (eattackerHp < 0)
				eattackerHp = 0;

			attackInformation.AttackDirection.text = UnitBattleController.AttackDirectionType ();
			attackInformation.AttackerHP.text = (int)(eattackerHp) + " %";
			attackInformation.DefenderHP.text = (int)(edefenderHp) + " %";

			attackInformation.EstimateAttack.text = (int)attack + " %";
			if (counter > 0)
				attackInformation.EstimateCounter.text = (int)counter + " %";
			else
				attackInformation.EstimateCounter.text = "--";
		
			attackInformation.UnitName.text = UnitBattleController.Defender.name;
			attackInformation.UnitHP.text = (int)defenderHp + " %";
		}
	}
}
