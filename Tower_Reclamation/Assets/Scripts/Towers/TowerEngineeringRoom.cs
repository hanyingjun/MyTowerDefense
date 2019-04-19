﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerEngineeringRoom : MonoBehaviour {

    [SerializeField] Dropdown towerTurretOne;
    [SerializeField] Dropdown towerPartOne;

    [SerializeField] Dropdown towerTurretTwo;
    [SerializeField] Dropdown towerPartTwo;


    // Use this for initialization
    void Start () {
        towerTurretOne.ClearOptions();
        towerTurretTwo.ClearOptions();
        List<string> towerBases = new List<string> { "Rifled", "Assault", "Flame", "Lightening"};
        towerTurretOne.AddOptions(towerBases);
        towerTurretTwo.AddOptions(towerBases);

        towerPartOne.ClearOptions();
        towerPartTwo.ClearOptions();
        List<string> partOptions = new List<string> { "Barrel", "Base" };
        towerPartOne.AddOptions(partOptions);
        towerPartTwo.AddOptions(partOptions);
    }

    // Update is called once per frame
    void Update () {
		
	}
}