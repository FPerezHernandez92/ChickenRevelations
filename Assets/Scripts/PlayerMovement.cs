using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


	public float walkSpeed = 5;
	public float runSpeed = 15;
	private float acceleration = 8;
	public float gravity;



	private CharacterController ccontroller;
	private Animator anim;

	void Start(){

		//Manejo del personaje
		ccontroller = GetComponent<CharacterController>();

		//Sistema de animacion
		anim = gameObject.GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void FixedUpdate () {

		Vector3 movement = Vector3.zero;

		//Solo si se encuentra en el suelo, sino se le aplica gravedad
		if(ccontroller.isGrounded){
			movement = keyboardController();
		}

		//Gravedad
		movement.y -= gravity * Time.deltaTime * 10;

		//Animacion
		animAdmin();

		Debug.Log (movement);

		ccontroller.Move(movement * Time.deltaTime); 
	}


	private Vector3 keyboardController(){


		//Vector del movimiento
		Vector3 movementVector = Vector3.zero;
		movementVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));			

		movementVector = (Input.GetButton("Run")) ? movementVector *= runSpeed : movementVector *= walkSpeed;

		if(movementVector != Vector3.zero)
			transform.forward = Vector3.Normalize(movementVector);

		return movementVector;
	}

	private void animAdmin(){
		/*if((CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0) ||
			(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)){
			anim.SetBool("Walk", true);

			if(CrossPlatformInputManager.GetButton("Yellow") || Input.GetButton("Run")){
				if(Input.GetButton("Run"))
					anim.SetBool("Run", true);
			}
			else
				anim.SetBool("Run", false);

		}
		else{
			anim.SetBool("Walk", false);
			anim.SetBool("Run", false);
		}*/
	}
}
