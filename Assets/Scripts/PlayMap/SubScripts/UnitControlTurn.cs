using UnityEngine;
using System.Collections;

public class UnitControlTurn {
	private static int turn = 0;
	private static int player = -1;

	private static void nextTurn(){
		turn ++;
	}

	public static void nextPlayer(){
		PlayerFaction faction = PlayMap.getFaction (player);
		if (faction != null)
			faction.SleepAllUnits ();
		
		player ++;
		faction = PlayMap.getFaction (player);
		if (faction == null) {
			player = 0;
			nextTurn ();
			faction = PlayMap.getFaction (player);
		}
		if (faction != null) {
			faction.WakeAllUnits ();
			PlayMap.Menu.Paint(faction.FactionColor);
		}
	}
}
