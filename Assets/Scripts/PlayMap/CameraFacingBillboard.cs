using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour {
	
	
	public Camera m_Camera = null;
	
	void Awake(){
		if (m_Camera == null){
			m_Camera = Camera.main;
		}
	}
	
	
	void Update()
	{
		Awake ();
		transform.LookAt(transform.position + m_Camera.transform.rotation * -Vector3.back,
		                 m_Camera.transform.rotation * Vector3.up);
	}
}
