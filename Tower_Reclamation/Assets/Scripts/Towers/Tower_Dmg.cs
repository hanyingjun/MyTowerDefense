﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Dmg : MonoBehaviour {

    public float dmg = 0;
    bool flameTower = false;
    bool rifledTower = false;


    // Use this for initialization
    void Start () {

    }
	
    public float towerDMG()
    {
        if (GetComponent<Towers>())
        {
            rifledTower = true;
            dmg = GetComponent<Towers>().Damage();
        }

        if (GetComponent<Tower_Flame>())
        {
            flameTower = true;
            dmg = GetComponent<Flame_AOE>().Damage();
        }

        return dmg;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
