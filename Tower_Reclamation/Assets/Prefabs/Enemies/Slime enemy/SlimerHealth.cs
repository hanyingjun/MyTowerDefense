﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimerHealth : EnemyHealth {

	// Use this for initialization
	override protected void Start () {
        base.Start();
        hitPoints = hitPoints * 1.2f;
        hitPointsMax = hitPoints;
	}
	
	// Update is called once per frame
	override protected void Update () {
        base.Update();
	}
}