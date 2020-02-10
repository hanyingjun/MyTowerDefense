﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighteningTower : Tower {

    // Todo try to make this a physics.OverlapSphere.
    //[SerializeField] public SphereCollider attackAOE;
    [SerializeField] public float chargeTime = 8f;
    [SerializeField] public float currentChargeTime = 0;
    public bool isCharged = false;
    Singleton singleton;
    bool reducedCost = false;
    //public new int goldCost = 80;

    [SerializeField] protected Light charge;
    [SerializeField] protected ParticleSystem projectileParticle;
    //paramteres of each tower
    //SphereCollider attackAOE;
    //float attackRange;
    //float chargeTime;
    //float currentChargeTime;
    //bool isCharged = false;

    //Light charge;
    //ParticleSystem projectileParticle;
    //float towerDmg;
    //private float currentTowerDmg;
    //List<EnemyMovement> targets;

    // State of tower
    //[SerializeField] Transform targetEnemy;

    protected override void Start()
    {
        singleton = FindObjectOfType<Singleton>();
        if (singleton.silverWiring)
        {
            reducedCost = true;
        }


        towerDmg = 30;
        goldCost = (int)TowerCosts.LighteningTowerCost;

        if (!keepBuffed)   {    }
        if (keepBuffed)
        {
            attackRange = attackRange * 1.4f;
            currentTowerDmg = currentTowerDmg * 1.2f;
            //attackAOE.radius = attackAOE.radius * 1.4f;
        }
    }
    /// <summary>
    ///  *****************************************Change layer to ignore raycast fixes targetting bug.   ***************************** need to make sure i can click children though for tower upgrades.
    /// </summary>
    /// <param buttonName="targets"></param>


    //Waypoint baseWaypoint    For if i pass it here


    private void CheckEnemyRange(List<EnemyMovement> targets)
    {
        
        var sceneEnemies = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemy in sceneEnemies)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < attackRange)
            {
                targets.Add(enemy);
            }
        }
        print(targets.Count);
    }

    private void OnTriggerStay(Collider other)
    {
 
        if (isCharged)
        {
            List<EnemyMovement> targets = new List<EnemyMovement>();
            print("I am charged and enemies are nearby!!");
            CheckEnemyRange(targets);
            //var sceneEnemies = FindObjectsOfType<EnemyMovement>();
            for (int i = 0; i < targets.Count; i++)
            {
                print("POW");
                targets[i].GetComponent<EnemyHealth>().hitPoints -= towerDmg;
                if (targets[i].GetComponent<EnemyHealth>().hitPoints < 1)
                {
                    targets[i].GetComponent<EnemyHealth>().KillsEnemyandAddsGold();
                }

            }
            currentChargeTime = 0;
            isCharged = false;
            targets.Clear();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentChargeTime < chargeTime)
        {
            currentChargeTime += Time.deltaTime;
            charge.intensity = currentChargeTime / 6.33f;
        }
        else
        {
            isCharged = true;
        }

    }

    public override int GetTowerCost()
    {
        int towerCost = 0;

        towerCost = (int)TowerCosts.LighteningTowerCost;
        singleton = FindObjectOfType<Singleton>();

        if (singleton.silverWiring)
        {
            towerCost = Mathf.RoundToInt(towerCost * (float)((int)TinkerUpgradePercent.mark1 / 100f));
        }

        return towerCost;
    }


    /*
        private void FireAtEnemy()
        {
            float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);
            if (distanceToEnemy <= attackRange)
            {
                Shoot(true);
            }
            else
            {
                Shoot(false);
                SetTargetEnemy();
            }
        }

        private void Shoot(bool isActive)
        {
            var emissionModule = projectileParticle.emission;
            emissionModule.enabled = isActive;
        }
        */
}
