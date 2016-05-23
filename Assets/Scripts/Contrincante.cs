using UnityEngine;
using System.Collections;

public class Contrincante : MonoBehaviour {

	private int estado;
	private Animator animcontr;
	private NavMeshAgent navmes;
	private GameObject player;
	private float distancia;


	// Use this for initialization
	void Start () {
		estado = 0;
		animcontr = gameObject.GetComponentInChildren<Animator>();
		animcontr.SetBool ("WaitC", true);
		navmes = gameObject.GetComponent <NavMeshAgent>();
		player = GameObject.Find ("Comp");
		distancia = 2;
	}

	// Update is called once per frame
	void Update () {
		//Animacion
		animAdmin ();
		if (navmes.SetDestination (player.transform.position)) {
			float aux = Vector3.Distance (player.transform.position, gameObject.transform.position);
			if (distancia > aux) {
				estado = 2;
				player.GetComponent <PlayerMovement>().QuitarVida();
				int auxvida = player.GetComponent <PlayerMovement>().GetVida();
				print ("VIDA " + auxvida);
			} else {
				estado = 1;
		
			}
		}
	}

	private void animAdmin(){
		if (estado == 0) {
			animcontr.SetBool ("WalkC", false);
			animcontr.SetBool ("WaitC", true);
			animcontr.SetBool ("AtackC", false);
		}
		else if (estado == 1){
			animcontr.SetBool ("WalkC", true);
			animcontr.SetBool ("WaitC", false);
			animcontr.SetBool ("AtackC", false);
		}
		else if (estado == 2) {
			animcontr.SetBool ("WalkC", false);
			animcontr.SetBool ("WaitC", false);
			animcontr.SetBool ("AtackC", true);
		}
	}
}