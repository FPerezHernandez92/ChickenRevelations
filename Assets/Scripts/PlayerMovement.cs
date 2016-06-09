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
	private float distancia;
	public Contrincante contrin1;
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
		distancia = 1;
		teclapulsada = false;
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
					contrin1.QuitarVidaContr ();
					int auxvida;
					auxvida = contrin1.GetVidaContr ();
				}
			}

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
			contrin1 = collider.gameObject.GetComponent <Contrincante> ();
			atacar = true;
		}
		if ((collider.gameObject.tag == "Finish") && (num_contrincantes_restantes == 0)) {
			GameObject.Find("TextContrincante").GetComponent<Text>().text = ("HAS GANADO");
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
