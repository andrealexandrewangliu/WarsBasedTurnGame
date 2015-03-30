using UnityEngine;
using System.Collections;

public class PathShow : MonoBehaviour {
	public UnitControlMovement.OriginDirection Next = UnitControlMovement.OriginDirection.N, 
											   Prev = UnitControlMovement.OriginDirection.S;
	public PathType Type = PathType.Path;
	public GameObject EndIcon;
	public GameObject CurveIcon;
	public GameObject StraightIcon;
	public GameObject StartIcon;

	public enum PathType{
		Origin,
		Target,
		Path
	}


	public void HidePath(){
		EndIcon.SetActive (false);
		CurveIcon.SetActive (false);
		StraightIcon.SetActive (false);
		StartIcon.SetActive (false);

	}

	public void ShowPath(){
		HidePath ();
		switch (Type) {
		case PathType.Origin:
			//START BLOCK
			switch(Next){
			case  UnitControlMovement.OriginDirection.N:
				StartIcon.SetActive (true);
				StartIcon.transform.localEulerAngles = new Vector3 (0, 0, 0);
				break;
			case  UnitControlMovement.OriginDirection.S:
				StartIcon.SetActive (true);
				StartIcon.transform.localEulerAngles = new Vector3 (0, 180, 0);
				break;
			case  UnitControlMovement.OriginDirection.E:
				StartIcon.SetActive (true);
				StartIcon.transform.localEulerAngles = new Vector3 (0, 90, 0);
				break;
			case  UnitControlMovement.OriginDirection.W:
				StartIcon.SetActive (true);
				StartIcon.transform.localEulerAngles = new Vector3 (0, 270, 0);
				break;
			}
			break;
		case PathType.Target:
			//END ARROW
			switch(Prev){
			case  UnitControlMovement.OriginDirection.S:
				EndIcon.SetActive (true);
				EndIcon.transform.localEulerAngles = new Vector3 (0, 0, 0);
				break;
			case  UnitControlMovement.OriginDirection.N:
				EndIcon.SetActive (true);
				EndIcon.transform.localEulerAngles = new Vector3 (0, 180, 0);
				break;
			case  UnitControlMovement.OriginDirection.W:
				EndIcon.SetActive (true);
				EndIcon.transform.localEulerAngles = new Vector3 (0, 90, 0);
				break;
			case  UnitControlMovement.OriginDirection.E:
				EndIcon.SetActive (true);
				EndIcon.transform.localEulerAngles = new Vector3 (0, 270, 0);
				break;
			}
			break;
		case PathType.Path:
			//STRAIGHT BLOCK NS
			if ((Next == UnitControlMovement.OriginDirection.N && Prev == UnitControlMovement.OriginDirection.S) ||  
			    (Next == UnitControlMovement.OriginDirection.S && Prev == UnitControlMovement.OriginDirection.N)){
				StraightIcon.SetActive (true);
				StraightIcon.transform.localEulerAngles = new Vector3 (0, 0, 0);
			}
			//STRAIGHT BLOCK EW 
			else if ((Next == UnitControlMovement.OriginDirection.E && Prev == UnitControlMovement.OriginDirection.W) ||  
			           (Next == UnitControlMovement.OriginDirection.W && Prev == UnitControlMovement.OriginDirection.E)){
				StraightIcon.SetActive (true);
				StraightIcon.transform.localEulerAngles = new Vector3 (0, 90, 0);
			}
			//CURVE BLOCK SW
			else if ((Next == UnitControlMovement.OriginDirection.S && Prev == UnitControlMovement.OriginDirection.W) ||  
			         (Next == UnitControlMovement.OriginDirection.W && Prev == UnitControlMovement.OriginDirection.S)){
				CurveIcon.SetActive (true);
				CurveIcon.transform.localEulerAngles = new Vector3 (0, 0, 0);
			}
			//CURVE BLOCK NW
			else if ((Next == UnitControlMovement.OriginDirection.N && Prev == UnitControlMovement.OriginDirection.W) ||  
			         (Next == UnitControlMovement.OriginDirection.W && Prev == UnitControlMovement.OriginDirection.N)){
				CurveIcon.SetActive (true);
				CurveIcon.transform.localEulerAngles = new Vector3 (0, 90, 0);
			}
			//CURVE BLOCK NE
			else if ((Next == UnitControlMovement.OriginDirection.N && Prev == UnitControlMovement.OriginDirection.E) ||  
			         (Next == UnitControlMovement.OriginDirection.E && Prev == UnitControlMovement.OriginDirection.N)){
				CurveIcon.SetActive (true);
				CurveIcon.transform.localEulerAngles = new Vector3 (0, 180, 0);
			}
			//CURVE BLOCK SE
			else if ((Next == UnitControlMovement.OriginDirection.S && Prev == UnitControlMovement.OriginDirection.E) ||  
			         (Next == UnitControlMovement.OriginDirection.E && Prev == UnitControlMovement.OriginDirection.S)){
				CurveIcon.SetActive (true);
				CurveIcon.transform.localEulerAngles = new Vector3 (0, 270, 0);
			}
			break;
		}
	}
}
