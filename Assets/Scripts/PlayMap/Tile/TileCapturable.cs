using UnityEngine;
using System.Collections;

public class TileCapturable : MonoBehaviour {
	public float MaxPoints = 20;
	public float Points = 0;
	public PlayerFaction Owner = null;
	public UnityEngine.UI.Image OwnerImage;
	public UnityEngine.UI.Image ConquerorImage;

	public void resetCapture(){
		if (Points != 0) {
			Points = 0;
			updateTile();
		}
	}

	public static float controlStrength(GameUnit unit){
		if (unit.UnitBodySpec.getUnitClassSize () == UnitSpec.UnitSize.S) {
			float healthP = unit.health / unit.UnitBodySpec.getMaxHealth ();
			switch(unit.UnitBodySpec.getUnitType()){
			case UnitSpec.UnitType.Walker:
			case UnitSpec.UnitType.Wheeled:
			case UnitSpec.UnitType.Track:
			case UnitSpec.UnitType.Boat:
				return 10.0f * healthP;
			case UnitSpec.UnitType.Hover:
				return 7 * healthP;
			case UnitSpec.UnitType.Sub:
			case UnitSpec.UnitType.Heli:
				return 5 * healthP;
			}
		}
		return 0;
	}

	public void capture(GameUnit unit){
		if (Owner != unit.playerFactionSpec) {
			ConquerorImage.color = unit.playerFactionSpec.FactionColor;
			Points += controlStrength (unit);
			if (Points >= MaxPoints) {
				Owner = unit.playerFactionSpec;
				Points = 0;
			}
			updateTile();
		}
	}

	public void updateTile(){
		if (Owner != null) {
			OwnerImage.color = Owner.FactionColor;
		} 
		else {
			OwnerImage.color = Color.white;
		}
		ConquerorImage.fillAmount = Points / MaxPoints;
	}
}
