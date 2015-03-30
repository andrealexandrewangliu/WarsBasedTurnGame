using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject Grid;
	public float Speed;
	public float ScrollSpeed;
	public float MinHeight;
	public float MaxHeight;
	private Vector3 CameraCoordinates;

	// Use this for initialization
	void Start () {
		GridControl GridControlScript = (GridControl) Grid.GetComponent ("GridControl");
		Vector2 GridSize = GridControlScript.MapSize;
		CameraCoordinates = new Vector3 (GridSize.x / 2, transform.position.y, (GridSize.y / 2));
		transform.position = new Vector3 (CameraCoordinates.x, CameraCoordinates.y, CameraCoordinates.z-(0.75f*CameraCoordinates.y));

		if (GetComponent<Camera>().orthographic){
			GetComponent<Camera>().orthographicSize = transform.position.y/3;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		float moveScroll = Input.GetAxis("Mouse ScrollWheel");

		//(left,right),(up,down),(foward,backward)
		Vector3 movement = new Vector3 (moveHorizontal, moveScroll * ScrollSpeed, moveVertical) * Speed * Time.deltaTime;

		if (CameraCoordinates.y + movement.y < MinHeight)
			movement.y  = MinHeight - transform.position.y;
		else if (CameraCoordinates.y + movement.y > MaxHeight)
			movement.y  = MaxHeight - transform.position.y;

		CameraCoordinates += movement;
		//Time.deltaTime is to make the game consistent with framerate

		if (GetComponent<Camera>().orthographic){
			GetComponent<Camera>().orthographicSize = transform.position.y/3;
		}
		transform.position = new Vector3 (CameraCoordinates.x, CameraCoordinates.y, CameraCoordinates.z-(0.75f*CameraCoordinates.y));
	}
}
