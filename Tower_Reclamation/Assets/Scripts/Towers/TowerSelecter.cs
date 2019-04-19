﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelecter : MonoBehaviour
{
    [Header("Tower Blueprint")]
    [SerializeField] Dropdown towerBarrel;
    [SerializeField] Dropdown towerTurret;
    [SerializeField] Dropdown towerBase;

    Tower newTower;
    Tower decidedTower;

    [Header("Rifle Towers")]
    [SerializeField]
    Tower basicRifledTower;


    [Header("Flame Towers")]
    [SerializeField] Tower basicFlameTower;
    [SerializeField] Tower tallFlameTower;
    [SerializeField] Tower heavyFlameTower;
    [SerializeField] Tower lightFlameTower;
    [SerializeField] Tower alienFlameTower;


    [Header("Lightening Towers")]
    [SerializeField] Tower basicLightTower;



    float turnSpeed = 6f;

    [SerializeField] GameObject towerPlaceholder;
    Vector3 towerPosition;
    Bounds bound;
    BoxCollider collider;

    // Use this for initialization
    void Start()
    {
        // this value is for the turret room only sandbox.
        //towerPosition = new Vector3(5.2f, -1f, -2.70f);

        //this value is for the base turret room.
        towerPosition = towerPlaceholder.transform.position;
        //towerPosition = new Vector3(50.0f, 76f, -123.0f);

        towerBase.value = 0;
        towerTurret.value = 0;
        towerBarrel.value = 0;


        if (towerBarrel.value == 0 && towerTurret.value == 0 && towerBase.value == 0)
        {
            print("reading dropdown");
            newTower = Instantiate(basicRifledTower, towerPosition, Quaternion.identity);
            newTower.transform.localScale = new Vector3(.3f, .3f, .3f);

        }
        collider = newTower.GetComponent<BoxCollider>();
        bound = collider.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            newTower = FindObjectOfType<Tower>();

            //newTower.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
            var dtx = Input.GetAxis("Mouse X") * turnSpeed;
            // var dty = Input.GetAxis("Mouse Y") * turnSpeed;
            var pivot = bound.center;

            newTower.transform.RotateAround(pivot, Vector3.up, dtx);
            // newTower.transform.RotateAround(pivot, Vector3.right, dty);
        }
    }

    public void ResetNumbersOnBaseChange()
    {
        towerBase.value = 0;
        towerBarrel.value = 0;
    }


    public void ResetTowerPicture()
    {
        DestroyObject(newTower.gameObject);
        decidedTower = PickTower();
        newTower = Instantiate(decidedTower, towerPosition, Quaternion.identity);
        
        newTower.transform.localScale = new Vector3(.3f, .3f, .3f);
        print("summoned");
    }

    // change to tower type
    public Tower PickTower()
    {
        if (towerTurret.value == 0)
        {
            print("Rifled Tower selected");
            FocusRifledTowers();
            decidedTower = basicRifledTower;
        }

        if (towerTurret.value == 1)
        {
            print("Flame Turret selected");
            FocusFireTowers();
            PickFireTower();        
        }

        if (towerTurret.value == 2)
        {
            print("Light Turret selected");
            FocusLighteningTowers();
            decidedTower = basicLightTower;
        }


        return decidedTower;
    }

    private void PickFireTower()
    {
        if (towerTurret.value == 1 && towerBase.value == 0)
        {
            decidedTower = basicFlameTower;
        }
        if (towerTurret.value == 1 && towerBase.value == 1)
        {
            decidedTower = tallFlameTower;
        }
        if (towerTurret.value == 1 && towerBase.value == 2)
        {
            decidedTower = heavyFlameTower;
        }
        if (towerTurret.value == 1 && towerBase.value == 3)
        {
            decidedTower = lightFlameTower;
        }
        if (towerTurret.value == 1 && towerBase.value == 4)
        {
            decidedTower = alienFlameTower;
        }
    }

    private void FocusFireTowers()
    {
        towerBase.ClearOptions();
        List<string> fireBases = new List<string> { "Basic Base", "Tall Base", "Heavy Base", "Light Base", "Alien Base" };
        towerBase.AddOptions(fireBases);
    }

    private void FocusRifledTowers()
    {
        towerBase.ClearOptions();
        List<string> rifledBases = new List<string> { "Basic Base" };
        towerBase.AddOptions(rifledBases);
    }

    private void FocusLighteningTowers()
    {
        towerBase.ClearOptions();
        List<string> lighteningBases = new List<string> { "Basic Base" };
        towerBase.AddOptions(lighteningBases);
    }


}
//towerBarrel.options.Clear();