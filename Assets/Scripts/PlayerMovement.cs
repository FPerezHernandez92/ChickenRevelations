using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	public float walkSpeed = 5;
	public float runSpeed = 15;
	public float gravity;
	public int vidaPersonaje;
	private CharacterController ccontroller;
	private Animator anim;
	private bool atacar;
	bool teclapulsada;
	private int contador_tiempo;
	private NavMeshAgent navmes;
	private GameObject contr1;
	private float distancia;
	public int auxquitarvidaC;
	public Contrincante contrin1;

	void Start(){
		//Manejo del personaje
		ccontroller = GetComponent<CharacterController>();
		//Sistema de animacion
		anim = gameObject.GetComponentInChildren<Animator>();
		vidaPersonaje = 100;
		atacar = false;
		contador_tiempo = 0;
		navmes = gameObject.GetComponent <NavMeshAgent> ();
		contr1 = GameObject.Find ("zombie anime");

		distancia = 1;
		auxquitarvidaC = 0;
		teclapulsada = false;
	}


	// Update is called once per frame
	void FixedUpdate () {
		setTextVida ();
		
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
		if (Input.GetKeyDown ("e")) { 
			print ("teclapulsada");
			teclapulsada = true;
		} else {
			print ("ya no esta pulsada");
			teclapulsada = false;
		}
		if (atacar && contador_tiempo > 50) {
			atacar = false;
			contador_tiempo = 0;
		}

		float aux = Vector3.Distance (contr1.transform.position, gameObject.transform.position);
		if (distancia > aux && atacar && teclapulsada) {
			//if (auxquitarvidaC == 7) {
				contr1.GetComponent <Contrincante> ().QuitarVidaContr ();
				int auxvida = contr1.GetComponent <Contrincante> ().GetVidaContr ();
				print ("VIDA contr " + auxvida);
				auxquitarvidaC = 0;
			//} 
			auxquitarvidaC++;
		}

	}

	void OnTriggerEnter( Collider collider){
		if (collider.gameObject.tag == "zombieE") {
			/*if (auxquitarvidaC >= 7) {
				collider.gameObject.GetComponent <Contrincante> ().QuitarVidaContr ();
				print ("ESTOY QUITANDO VIDA");
				int auxvida = collider.gameObject.GetComponent <Contrincante> ().GetVidaContr ();
				print ("VIDA contr " + auxvida);
				auxquitarvidaC = 0;
			} 
			print ("Voy a mas mas ");
			auxquitarvidaC++;*/

			//contrin1 = collider.gameObject.GetComponent <Contrincante> ();
			atacar = true;
		}
	}
	void OnTriggerExit( Collider collider){
		atacar = false;
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

	public void PonerVida(){
		if (vidaPersonaje <= 80)
			vidaPersonaje += 20;
		else
			vidaPersonaje = 100;
	}

	public int GetVida(){
		return vidaPersonaje;
	}

	public void setTextVida(){
		GameObject.Find("TextVida").GetComponent<Text>().text = vidaPersonaje.ToString();

	}
}
