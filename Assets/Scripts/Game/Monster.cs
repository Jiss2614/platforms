﻿using UnityEngine;
using System.Collections;


[System.Serializable]
public class Ai {
	public float visionSpeed = 0.5f;
	public float moveSpeed = 0.5f;
	public float attackSpeed = 2f;

	public float visionRange = 10f;
}


public class Monster : Humanoid {

	public Ai ai = new Ai();

	protected Player player;
	protected bool aware = false;


	public override void Awake () {
		player = GameObject.Find("Player").GetComponent<Player>();
		base.Awake();

		// initialize ai loops
		StartCoroutine(AiVision());
		StartCoroutine(AiMove());
		StartCoroutine(AiAttack());
	}


	protected override void SetInput () {
		//print (state + " " + CanThink());
	}


	// ===========================================================
	// Ai Vision
	// ===========================================================

	private IEnumerator AiVision () {
		yield return new WaitForSeconds(ai.visionSpeed);
		if (!player) { yield break; }

		if (state == States.IDLE) {
			// cast a ray to player to check if we become aware/unaware of him
			Vector2 rayOrigin = new Vector2 (transform.position.x, transform.position.y + GetHeight() / 2);
			Vector2 direction = (new Vector2 (player.transform.position.x, player.transform.position.y + GetHeight() / 2) - rayOrigin).normalized;
			float distance = ai.visionRange;

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance, controller.collisionMask);
			Debug.DrawRay(rayOrigin, direction * distance, Color.cyan);

			yield return StartCoroutine(SetAware(hit && hit.transform.gameObject.tag == "Player"));
		}
		
		StartCoroutine(AiVision());
	}


	private IEnumerator SetAware (bool value) {
		if (aware == value) { yield break; }

		input.x = 0;
		velocity.x = 0;

		if (value) {
			float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
			sprite.localScale = new Vector2(dir * Mathf.Abs(sprite.localScale.x), sprite.localScale.y);
		} else {
			aware = value;
		}

		StartCoroutine(UpdateInfo(value ? "!" : "?"));
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(UpdateInfo(null));

		aware = value;
	}


	// ===========================================================
	// Ai Movement
	// ===========================================================

	private IEnumerator AiMove () {
		yield return new WaitForSeconds(ai.moveSpeed);
		if (!player) { yield break; }

		if (state == States.IDLE) {
			float d = player.transform.position.x - transform.position.x;
			input.x = aware && Mathf.Abs(d) > 1.0f ? Mathf.Sign(d) : 0;
		}

		StartCoroutine(AiMove());
	}


	// ===========================================================
	// Ai Attack
	// ===========================================================

	private IEnumerator AiAttack () {
		yield return new WaitForSeconds(ai.attackSpeed);
		if (!player) { yield break; }

		if (state == States.IDLE) {
			float playerDist = Vector2.Distance(transform.position, player.transform.position);
			if (playerDist <= 1.5f) {
				// turn versus player and attack
				float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
				sprite.localScale = new Vector2(dir * Mathf.Abs(sprite.localScale.x), sprite.localScale.y); 
				
				yield return StartCoroutine(Attack());
			}
		}

		StartCoroutine(AiAttack());
	}



	// TODO:

	// monsters will check if there is floor on their next move
	// if there is no floor they need to check which tile will they go
	//		- fall to tile below
	//		- jump to accessible tile in jump range

	// if controller.collisions.left/right
	//		- check if jump in direction will suceed 
	//			yes: jump 
	//			no: move in opposite direction


}