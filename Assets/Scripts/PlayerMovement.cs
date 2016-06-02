using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float walkSpeed = 5;
	public float runSpeed = 15;
	public float gravity;
	public int vidaPersonaje;
	private CharacterController ccontroller;
	private Animator anim;
	private bool atacar;
	private int contador_tiempo;
	private NavMeshAgent navmes;
	private GameObject contr1;
	private float distancia;

	void Start(){
		//Manejo del personaje
		ccontroller = GetComponent<CharacterController>();
		//Sistema de animacion
		anim = gameObject.GetComponentInChildren<Animator>();
		vidaPersonaje = 300;
		atacar = false;
		contador_tiempo = 0;
		navmes = gameObject.GetComponent <NavMeshAgent> ();
		contr1 = GameObject.Find ("zombie anime");
		distancia = 1;
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
		ccontroller.Move(movement * Time.deltaTime); 
		if(Input.GetKeyDown("e")){ 
			print ("ESTOY ATACANDO");
			atacar = true;
		} 
		if (atacar && contador_tiempo > 50) {
			atacar = false;
			contador_tiempo = 0;
		}

		float aux = Vector3.Distance (contr1.transform.position, gameObject.transform.position);
		if (distancia > aux && atacar) {
			contr1.GetComponent <Contrincante>().QuitarVidaContr();
			int auxvida = contr1.GetComponent <Contrincante>().GetVidaContr();
			print ("VIDA contr " + auxvida);
		} 

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
		if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)) {
			if (Input.GetButton ("Run")) {
				anim.SetBool ("Run", true);
				anim.SetBool ("Walk", false);
				anim.SetBool ("Atack", false);
			} else {
				anim.SetBool ("Run", false);
				anim.SetBool ("Walk", true);
				anim.SetBool ("Atack", false);
			}
		} else if (atacar) {
			anim.SetBool ("Atack", true);
			anim.SetBool ("Run", false);
			anim.SetBool ("Walk", false);
			contador_tiempo++;
		}
		else{
			anim.SetBool("Walk", false);
			anim.SetBool("Run", false);
			anim.SetBool ("Atack", false);
		}

		//Debug.Log (anim);
	}

	public void QuitarVida(){
		vidaPersonaje--;
	}

	public int GetVida(){
		return vidaPersonaje;
	}
}
