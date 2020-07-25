﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Plasma : Tower
{

    public float distanceToEnemyTest;
    public CapsuleCollider laser;
    [SerializeField] ParticleSystem spray;
    List<EnemyHealth> targetsList = new List<EnemyHealth>();
    Tower_PlasmaHead plasmaTargeter;
    LineRenderer lineRenderer;

    float crystalDmgInterval = .25f;
    float crystalCurrentBeamTime = 0f;
    float minTowerDmg = 15;
    float maxTowerDmg = 30f;

    float maxCharge;
    float currentChargeTime = 0f;
    bool canFire = false;

    //1.25 worked well
    float laserOnTime = .25f;
    float laserCurrentTime = 0f;
    bool laserIsOn = false;

    private int headType = 0;

    Singleton singleton;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();

        //laser = transform.GetComponentInChildren<CapsuleCollider>();
    }


    public override void DelayedStart()
    {
        TowerTypeExplanation = "The plasma tower is the only turret that has a strong enough hit to pierce the target completely.  " +
            "This heavy shot comes at a slight cost to accuracy: not enough to entirely miss the target, but it does make it a gamble on where you hit them, " +
            "and in direct correlation, how much damage the shot causes.";

        maxCharge = 4f;
        goldCost = (int)TowerCosts.PlasmaTowerCost;
        attackRange = 30;
        towerDmg = 18;
        base.CheckUpgradesForRifledTower(ref towerDmg, ref attackRange);
        CheckAndApplyBuff();
    }


    // Update is called once per frame
    void Update()
    {

        if (preferedEnemyBody != null && preferedEnemyBody != targetEnemyBody)
        {
            float distanceToPreferedEnemy = Vector3.Distance(preferedEnemyBody.gameObject.transform.position, gameObject.transform.position);
            if (distanceToPreferedEnemy <= attackRange && targetEnemyBody.isTargetable)
            {
                print(preferedEnemyBody.gameObject.name);
                targetEnemyBody = preferedEnemyBody;
                targetEnemy = preferedEnemyBody.gameObject.transform;
            }
        }
        switch (headType)
        {
            case (int)PlasmaHead.Basic:
                TowerChargedShotAttacks();
                break;
            case (int)PlasmaHead.Crystal:
                if (targetEnemy)
                {
                    objectToPan.LookAt(targetEnemy);
                    FireAtEnemyWithCrystal();
                }
                else
                {
                    Shoot(false);
                    SetTargetEnemy();
                }


                break;
        }
        
    }

    private void TowerChargedShotAttacks()
    {
        if (!canFire)
        {
            currentChargeTime += 1 * Time.deltaTime;
            if (currentChargeTime > maxCharge)
            {
                canFire = true;
                currentChargeTime = 0f;
            }
        }

        if (laserIsOn)
        {
            //spray.emission.SetBurst(1);
            laserCurrentTime += 1 * Time.deltaTime;
            if (laserCurrentTime > laserOnTime)
            {
                //get list first, before turning off object.
                GetListOfEnemies();

                laserCurrentTime = 0f;
                canFire = false;
                laserIsOn = false;


                spray.Emit(10);

                HitEnemies();
                // hit then clear them
                targetsList.Clear();
                plasmaTargeter.ClearEnemies();
                laser.gameObject.SetActive(false);
            }
        }

        if (targetEnemy)
        {
            objectToPan.LookAt(targetEnemy);
            FireAtEnemy();
        }
        else
        {
            Shoot(false);
            SetTargetEnemy();
        }
    }

    public void HitEnemies()
    {
        print(targetsList.Count + " enemies in list");
        foreach (EnemyHealth enemy in targetsList)
        {
            try
            {
                enemy.HitByNonProjectile(towerDmg);
            } catch(Exception e)
            {
                print("problem hitting the guy " + enemy.name);
                // nothing it may have died since being in the list.
            }
            
        }
    }


    public void GetListOfEnemies()
    {
        plasmaTargeter = GetComponentInChildren<Tower_PlasmaHead>();

        targetsList = plasmaTargeter.getEnemies();
        print("targets " + targetsList.Count);
    }


    public override void DetermineTowerTypeBase(int towerInt)
    {
        switch (towerInt)
        {
            case (int)PlasmaBase.Basic:
                //nothing, normal settings?
                TowerBaseExplanation = "Basic base.";
                break;
            //case (int)PlasmaBase.:
            //    TowerBaseExplanation = "Industrial base.";
            //    break;
            default:
                print("Default base, I am towerint of : " + towerInt);
                //nothing
                break;
        }
    }


    public override void DetermineTowerHeadType(int towerInt)
    {
        switch (towerInt)
        {
            case (int)PlasmaHead.Basic:
                headType = (int)PlasmaHead.Basic;
                TowerAugmentExplanation = "The default head of the Plasma Turret.  Hits in a line for randomised damage.";
                minTowerDmg = 10;
                maxTowerDmg = 30;
                //nothing;
                break;
            case (int)PlasmaHead.Crystal:
                // base is .25 ats so 4-12 DPS, maybe add 1 max per channel buff, ends at 4-20 dmg.  which is 8, 10, 12 DPS  but bad at target swapps.
                //Sounds balanced, avg is 12 DPS. but considering the ramp-up time and randomness, i think its good.
                // maybe make these a co-routine for stacks falling off.
                headType = (int)PlasmaHead.Crystal;
                crystalDmgInterval = .25f;
                lineRenderer = GetComponentInChildren<LineRenderer>();
                lineRenderer.SetPosition(0, (gameObject.transform.position + new Vector3(0, 5.5f, 0)));
                lineRenderer.useWorldSpace = true;
                TowerAugmentExplanation = "The crystal head of the Plasma Turret.  Amplifies the effects for a single target.";
                minTowerDmg = 1f;
                maxTowerDmg = 3f;
                //nothing;
                break;
            default:
                TowerAugmentExplanation = "The default head of the Plasma Turret.  Hits in a line for randomised damage.";
                break;
        }
    }


    public override void GetStringStats()
    {
        TowerStatsTxt = "Plasma Tower Stats \n" +
            "Attack Range = " + attackRange + "\n" +
            "Attack Damage = " + towerDmg + " \n" +
            "Attack Speed = This Tower charges over " + maxCharge  + " seconds \n" +
            "Targetting = Piercing shot through target.";
    }


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
        distanceToEnemyTest = distanceToEnemy;
    }


    private void FireAtEnemyWithCrystal()
    {

        float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);
        if (distanceToEnemy <= attackRange)
        {
            ShootWithCrystal(true);
        }
        else
        {
            ShootWithCrystal(false);
            SetTargetEnemy();
        }
        distanceToEnemyTest = distanceToEnemy;
    }

    // a better way to swap this? so not setting true every frame?
    private void ShootWithCrystal(bool isActive)
    {
        if (isActive)
        {
            crystalCurrentBeamTime += (1 * Time.deltaTime);
            if (crystalCurrentBeamTime > .25f)
            {
                crystalCurrentBeamTime = (crystalCurrentBeamTime % .25f);
                float towerDmg = UnityEngine.Random.Range(1, maxTowerDmg);
                //TODO NEED TO CHANGE this needs to only get the enemy health on TARGET CHANGE way too process intensive to get 4 times a second.
                targetEnemyBody.hitPoints -= towerDmg;
                targetEnemyBody.RefreshHealthBar();
                if (targetEnemyBody.hitPoints < 1)
                {
                    targetEnemyBody.KillsEnemyandAddsGold();
                }
            }
            //laser.gameObject.SetActive(true);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, (targetEnemy.transform.position));
        } else
        {
            lineRenderer.enabled = false;

        }
    }

    private void Shoot(bool isActive)
    {
        if (canFire && isActive)
        {
            laser.gameObject.SetActive(true);
            laserIsOn = true;
        }
    }


    public override float GetTowerCost()
    {
        float towerCost = 0;
        singleton = Singleton.Instance;

        towerCost = (int)TowerCosts.PlasmaTowerCost;

        float percentToPay = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.alloyResearch);

        towerCost = towerCost * percentToPay;

        return towerCost;
    }
}
