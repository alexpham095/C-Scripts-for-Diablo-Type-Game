using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
	[Header ("Stats")]
	public float attackDistance;
	public float attackRate;

	private float nextAttack;

	//NAV MESH
	private NavMeshAgent navMeshAgent;
	private Animator anim;

	private Transform targetedEnemy;
	private bool enemyClicked;
	private bool walking;

	//OBJECTS
	private Transform clickedObject;
	private bool objectClicked;

	//DOUBLE CLICK
	private bool oneClick;
	private bool doubleClick;
	private float timerForDoubleClick;
	private float delay = 0.25f;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Input.GetButtonDown ("Fire1")) {
			navMeshAgent.ResetPath ();
			if (Physics.Raycast (ray, out hit, 1000)) {
				if (hit.collider.tag == "Enemy") {
					targetedEnemy = hit.transform;
					enemyClicked = true; 
				} else if (hit.collider.tag == "Chest") {
					objectClicked = true;
					clickedObject = hit.transform;
				} else if (hit.collider.tag == "Info") {
					objectClicked = true;
					clickedObject = hit.transform;

				} else {
					walking = true;
					enemyClicked = false;
					navMeshAgent.isStopped = false;
					navMeshAgent.destination = hit.point;
				}
			}
		}
			

		if (enemyClicked) {
			MoveAndAttack ();
		} else if (objectClicked && clickedObject.gameObject.tag == "Info") {
			ReadInfos (clickedObject);
		} else if (objectClicked && clickedObject.gameObject.tag == "Chest") {
			OpenChest (clickedObject);
		} else {
			if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
				walking = false;
			} else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance) {
				walking = true;
			}
		}
			
		//anim.SetBool ("isWalking", walking);

		if (Input.GetKey (KeyCode.LeftShift)) {
			navMeshAgent.speed = 5.5f;
			anim.SetBool ("isRunning", walking);
			anim.SetBool ("isWalking", false);
		} else {
			navMeshAgent.speed = 3.5f;
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isWalking", walking);
		}

		if (!walking) {
			anim.SetBool ("isIdling", true);
		} else {
			anim.SetBool ("isIdling", false);
		}
	}

	void MoveAndAttack ()
	{
		if (targetedEnemy == null) {
			return;
		} 

		navMeshAgent.destination = targetedEnemy.position;

		if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance) {
			navMeshAgent.isStopped = false;
			walking = true;
		} else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance) {
			anim.SetBool ("isAttacking", false);
			transform.LookAt (targetedEnemy);
			Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

			if (Time.time > nextAttack) {
				nextAttack = Time.time + attackRate;
				//CALL ATTACK WITH DIRTOATTACK
				anim.SetBool ("isAttacking", true);
			}
			navMeshAgent.isStopped = true;
			walking = false;
		}
	}

	void ReadInfos (Transform target)
	{
		//set target
		navMeshAgent.destination = target.position;
		//go close
		if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance) {
			navMeshAgent.isStopped = false;
			walking = true;
		} else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance) {
			navMeshAgent.isStopped = true;
			transform.LookAt (target);
			walking = false;

			// print info 
			print (target.GetComponent<Infos> ().info);


			objectClicked = false;
			navMeshAgent.ResetPath ();
		}
	}

	void OpenChest (Transform target)
	{
		//set target
		navMeshAgent.destination = target.position;
		//go close
		if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance) {
			navMeshAgent.isStopped = false;
			walking = true;
		} else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance) {
			navMeshAgent.isStopped = true;
			transform.LookAt (target);
			walking = false;

			// play Animation
			target.gameObject.GetComponentInChildren<Animator> ().SetTrigger ("Play");


			objectClicked = false;
			navMeshAgent.ResetPath ();
		}
	}

	void CheckDoubleClick ()
	{
		if (Input.GetButtonDown ("fire1")) {
			if (!oneClick) {
				oneClick = true;
				timerForDoubleClick = Time.time; //current game time
			} else {
				oneClick = false;
				doubleClick = true;
			}
		}

		if (oneClick) {
			if ((Time.time - timerForDoubleClick) > delay) {
				oneClick = false;
				doubleClick = false;
			}
		}
	}
}
