﻿using UnityEngine;
using System.Collections;

public class Player : Ent {

	private InputManager inputManager;


	public override void Awake () {
		inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
		base.Awake();
	}


	protected override void SetInput () {
		input = Vector2.zero;
		if (inputManager.left) { input.x = -1f; }
		if (inputManager.right) { input.x = 1f; }
		if (inputManager.up) { input.y = 1f; }
		if (inputManager.down) { input.y = -1f; }

		if (inputManager.A) {
			SetJump(inputManager.down, inputManager.up ? 1.25f : 1f);
		}

		if (inputManager.B) { SetActionB(); }

		if (inputManager.C) { SetActionC(); }
	}
}
