﻿using UnityEngine;
using System.Collections;


public class Coin : Loot {

	public IEnumerator Pickup (Ent collector) {
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		affectedByGravity = false;

		Vector3 pos = transform.position + Vector3.up * collector.transform.localScale.y * 0.5f;
		
		while (Vector2.Distance(transform.position, pos) > 0.1f) { //(Time.time <= startTime + duration) {
			pos = collector.transform.position + Vector3.up * collector.transform.localScale.y * 0.5f;
			transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 5f);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 1f);
			yield return null;
		}

		Destroy(gameObject);
	}
}
