﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame_AOE : MonoBehaviour {


    [SerializeField] float towerDmg = 5;
    [SerializeField] private float currentTowerDmg = 5;
    [SerializeField] float baseAttackRange;
    [SerializeField] float currentAttackRange;
    [SerializeField] float baseAttackWidth;
    [SerializeField] float currentAttackWidth;
    [SerializeField] CapsuleCollider flameAOE;

    bool keepBuffed = false;

    void Start()
    {
        if (!keepBuffed)
        {
            currentAttackRange = flameAOE.radius;
            baseAttackRange = flameAOE.radius ;
        }
    }

    public void TowerBuff()
    {
        baseAttackRange = flameAOE.radius;
        currentAttackRange = flameAOE.radius;
        baseAttackWidth = flameAOE.height;
        currentAttackWidth = flameAOE.height;

        currentTowerDmg = currentTowerDmg * 1.2f;
        currentAttackRange = currentAttackRange * 1.5f;
        currentAttackWidth = currentAttackWidth * 1.5f;
        flameAOE.height = currentAttackWidth;
        flameAOE.radius = currentAttackRange;
        print("I am buffed!");

        keepBuffed = true;
    }


    public float Damage()
    {
        return currentTowerDmg;
    }



    private void OnTriggerStay(Collider other)
    {
        print("Something is burning");
        if (other.gameObject.GetComponentInParent<EnemyHealth>())
        {
            print("Workin");
            other.GetComponentInParent<EnemyHealth>().CaughtFire(currentTowerDmg);
        }
    }

}
