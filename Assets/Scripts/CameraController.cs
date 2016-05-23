using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private GameObject player;
	private Vector3 offset, offset2, offset3;

	public int margin;

	// Use this for initialization
	void Start () {
		player = transform.parent.gameObject;
		offset = offset2 = cameraPosition();;
		offset3 = new Vector3(offset.x, offset.y, offset.z);

		transform.position = player.transform.position + offset;

	}
	
	// Update is called once per frame
	void Update () {

		//Si el personaje entra en el margen inferior
		if(player.transform.position.z < margin){
			if(offset2.x == offset3.x && offset2.y < offset3.y && offset2.z < offset3.z){
				//offset2 += new Vector3(0, (float)0.4, (float)0.8);
			}
		}
		else{
			if(offset2 != offset){
				//offset2 -= new Vector3(0, (float)0.8, (float)0.8);
			}
		}
		transform.position = player.transform.position + offset2;
		transform.LookAt(player.transform);
	}

	private Vector3 cameraPosition(){
		Vector3 posCamera = new Vector3();

		posCamera.x = -(transform.parent.position.x - transform.position.x);
		posCamera.y = -(transform.parent.position.y - transform.position.y);
		posCamera.z = -(transform.parent.position.z - transform.position.z);
		
		return posCamera;
	}
}
