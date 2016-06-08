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
	public Contrincante contrinanterior;
	public int contador_muertos;
	public int num_contrincantes_restantes;
	public bool hasganado;
	public bool personajeMuerto;

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
		contador_muertos = 0;
		contrinanterior = null;
		num_contrincantes_restantes = 8;
		hasganado = false;
		personajeMuerto = false;
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (personajeMuerto)
			GameObject.Find ("TextContrincante").GetComponent<Text> ().text = ("HAS PERDIDO");
		
		if (!personajeMuerto && !hasganado) {
			setTextVida ();
			if (num_contrincantes_restantes != 0) {
				setTextContrincante ();
			} else {
				GameObject.Find ("TextContrincante").GetComponent<Text> ().text = ("VE A LA CUEVA");

			}
		
			Vector3 movement = Vector3.zero;
			//Solo si se encuentra en el suelo, sino se le aplica gravedad
			if (ccontroller.isGrounded) {
				movement = keyboardController ();
			}
			//Gravedad
			movement.y -= gravity * Time.deltaTime * 10;
			//Animacion
			animAdmin ();
			ccontroller.Move (movement * Time.deltaTime); 
			if (Input.GetKeyDown ("e")) { 
				teclapulsada = true;
			} else {
				teclapulsada = false;
			}
			if (atacar && contador_tiempo > 50) {
				atacar = false;
				contador_tiempo = 0;
			}

			if (contrin1 != null) {
				float aux = Vector3.Distance (contrin1.transform.position, gameObject.transform.position);
				if (distancia > aux && atacar && teclapulsada) {
					//if (auxquitarvidaC == 7) {
					//contr1.GetComponent <Contrincante> ().QuitarVidaContr ();
					contrin1.QuitarVidaContr ();
					int auxvida;
					auxvida = contrin1.GetVidaContr ();
					//auxvida = contr1.GetComponent <Contrincante> ().GetVidaContr ();
					//if (auxvida >= 0)
					//print ("VIDA contr " + auxvida);
					auxquitarvidaC = 0;
					//} 
					auxquitarvidaC++;
				}
			}

			if (num_contrincantes_restantes == 0)
				print ("Corre a por el tesoro");

			if (vidaPersonaje == 0) {
				anim.SetBool("Walk", false);
				anim.SetBool("Run", false);
				anim.SetBool ("Atack", false);
				anim.SetBool ("Muerto", true);
				personajeMuerto = true;
			}
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
			contrin1 = collider.gameObject.GetComponent <Contrincante> ();
			atacar = true;
		}
		if ((collider.gameObject.tag == "Finish") && (num_contrincantes_restantes == 0)) {
			GameObject.Find("TextContrincante").GetComponent<Text>().text = ("HAS GANADO");
			print ("has ganado");
			hasganado = true;
			contrin1.SetContadorTiempoMuerto ();
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
		if (vidaPersonaje > 0)
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

	public void setTextContrincante(){
		GameObject.Find("TextContrincante").GetComponent<Text>().text = ("Restantes: " + num_contrincantes_restantes.ToString());

	}

	public void AumentarContrincanteMuerto(){
		num_contrincantes_restantes--;
	}

	public void QuitarContrincanteMuerto(){
		num_contrincantes_restantes++;
	}

	public bool estaElPersonajeMuerto(){
		return personajeMuerto;
	}
}
