﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_Flame : Tower {

    // paramteres of each tower
    //[SerializeField] Transform objectToPan;
    //[SerializeField] float attackRange = 15f;
    //[SerializeField] ParticleSystem projectileParticle;
    //[SerializeField] ParticleSystem projectileParticleTwo;
    //[SerializeField] ParticleSystem projectileParticleThree;

    Flame_AOE head = null;
    Singleton singleton;
    // float particleLifetime;
    // float currentParticleLifetime;
    //public new int goldCost = 60;

    // State of tower
    //[SerializeField] Transform targetEnemy;

    readonly new bool cantargettingModule = true;
    readonly new bool canAlloyReasearch = true;
    readonly new bool canSturdyTank = true;
    readonly new bool canHeavyShelling = false;
    readonly new bool canTowerEngineer = true;

    private string attackAreaType = "Wide";
    private float healReductionIncrement = .10f;


    // Buff info
    //bool keepBuffed = false;


    override protected void Start()
    {
        base.Start();
        goldCost = (int)TowerCosts.FlameTowerCost;
        engineeringCostReduction = Singleton.Instance.GetPercentageModifier((int)TinkerUpgradeNumbers.towerEngineer);

        TowerTypeName = "Flame Tower";
        // nothing if it is unbuffed 

        // change this to percentage based buff.  otherwise it is an overwrite
        //if (!keepBuffed) { }
        //else
        //{
        //    //var particleLifetime = projectileParticle.main;
        //    //particleLifetime.startLifetimeMultiplier = .5f;
        //    //particleLifetime = projectileParticleTwo.main;

        //    //particleLifetime.startLifetimeMultiplier = .5f;

        //    //particleLifetime = projectileParticleThree.main;
        //    //particleLifetime.startLifetimeMultiplier = .5f;

        //    //.startLifetimeMultiplier;
        //    //  currentParticleLifetime = particleLifetime;
        //    // projectileParticle.main.startLifetimeMultiplier = currentParticleLifetime;

        //    attackRange = attackRange * 1.3f;
        //    GetComponentInChildren<Flame_AOE>().TowerBuff();

        //    keepBuffed = true;
        //}
    }

    public override void DelayedStart()
    {
        base.DelayedStart();

        TowerTypeExplanation = "The flame tower spews and ignites flammable material.  ";
        TowerTypeExplanation += "This causes burning damage over time, with both the severity and duration dependent on the material burning.  ";
        TowerTypeExplanation += "Unfortunately, something is either on fire or it is not, and stacking these debuffs can be wasteful.  ";



        head = GetComponentInChildren<Flame_AOE>();
        if (!keepBuffed) { }
        else
        {
            attackRange = attackRange * 1.3f;
            GetComponentInChildren<Flame_AOE>().TowerBuff();

            keepBuffed = true;
        }

        head.DelayedStart(keepBuffed);

        towerUpgradeDescriptionOne = "Upgrade tower Damage +20%";
        towerUpgradeDescriptionTwo = "Upgrade reduce enemy healing +10%";
        towerUpgradeDescriptionThree = "Upgrade tower AOE +15%";
    }

    public override void DetermineTowerTypeBase(int towerInt)
    {
        float towerDmgModifierPercent;
        float towerAttackRangeModifierPercent;
        float towerAttackSpeedModifierPercent;

        switch (towerInt)
        {
            case (int)FlameBase.Basic:
                TowerBaseExplanation = "The default base, with no modifiers.";
                //nothing, normal settings?
                break;
            case (int)FlameBase.Alien:
                towerDmgModifierPercent = .05f;
                towerAttackRangeModifierPercent = .05f;
                head.currentTowerDmg += (head.currentTowerDmg * towerDmgModifierPercent);
                head.BuffRange(towerAttackRangeModifierPercent);

                TowerBaseExplanation = "Tower damage +" + (int)(towerDmgModifierPercent * 100f) + '%';
                TowerBaseExplanation += "\nTower Area of Effect +" + (int)(towerDmgModifierPercent * 100f) + '%';
                //The aliens who helped build this were never good on the idea of 'compramise'.  
                //Where other towers would give a powerful mechanical trade-off, this simply improves slightly on the basic tower.
                break;
            case (int)FlameBase.Tall:
                // double range at 60% dmg.
                towerDmgModifierPercent = .30f;
                towerAttackRangeModifierPercent = .50f;
                head.BuffRange(towerAttackRangeModifierPercent);
                head.currentTowerDmg -= (head.currentTowerDmg * towerDmgModifierPercent);
                head.towerDmg -= (head.currentTowerDmg * towerDmgModifierPercent);

                TowerBaseExplanation = "Tower damage -" + (int)(towerDmgModifierPercent * 100f) + '%';
                TowerBaseExplanation += "\nTower Area of Effect +" + (int)(towerAttackRangeModifierPercent * 100f) + '%';
                break;
            default:
                print("Default base, I am towerint of : " + towerInt);
                //nothing
                break;
        }

        // TODO - make this recalculate range after range modifiers.
        towerDmg = head.towerDmg;
        currentTowerDmg = towerDmg;
    }

    public override void DetermineTowerHeadType(int towerInt)
    {
        switch (towerInt)
        {
            case (int)FlameHead.Basic:
                TowerAugmentExplanation = "The default tower head, with no modifiers.  The attack is a wide cone.";
                attackAreaType = "Wide";
                //nothing;
                break;
            case (int)FlameHead.FlameThrower:
                attackAreaType = "Long";
                TowerAugmentExplanation = "The flamethrower head, changes the attack area.  This version turns it, making it a long cone rather than wide cone.";

                head.ChangeParticleTime(1.5f);
                attackRange = head.SetTowerTypeFlameThrower();
                break;
        }
        currentAttackRange = head.GetTowerRange();
        attackRange = currentAttackRange;
    }

    //  The actual Dmg applier is on the head of the turret with the capsul collider.


    //Waypoint baseWaypoint    For if i pass it here
    //public void TowerBuff()
    //{

    //}


    /*
     public void TowerUpgrade()
     {
         // Upgrade before multiplying
         baseAttackRange += 10;
         attackRange = baseAttackRange;

         if (keepBuffed)
         {
             TowerBuff();
         }
     }
     */

    // Update is called once per frame
    void Update () {
        if(head == null)
        {
            return;
        }

        //first check if prefered enemy isin range, if so he beomes the target enemy.
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

        if (targetEnemy)
        {
            objectToPan.LookAt(targetEnemy.position);
            FireAtEnemy();
        }
        else
        {
            Shoot(false);
            SetTargetEnemy();
        }
	}

    //private void SetTargetEnemy()
    //{
    //    var sceneEnemies = FindObjectsOfType<EnemyMovement>();
    //    if (sceneEnemies.Length == 0) { return; }

    //    Transform closestEnemy = sceneEnemies[0].transform;

    //    foreach (EnemyMovement testEnemy in sceneEnemies)
    //    {
    //        closestEnemy = GetClosest(closestEnemy, testEnemy.transform);
    //    }

    //    targetEnemy = closestEnemy;
    //}
    
    //private Transform GetClosest(Transform transformA, Transform transformB)
    //{
    //    var distanceToA = Vector3.Distance(transform.position, transformA.position);
    //    var distanceToB = Vector3.Distance(transform.position, transformB.position);

    //    if (distanceToA <= distanceToB)
    //    {
    //        return transformA;
    //    }
    //    else
    //    {
    //        return transformB;
    //    }
    //}

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
        head.Shoot(isActive);
        //var emissionModule = projectileParticle.emission;
        //emissionModule.enabled = isActive;
        //var emissionModuleTwo = projectileParticleTwo.emission;
        //emissionModuleTwo.enabled = isActive;
        //var emissionModuleThree = projectileParticleThree.emission;
        //emissionModuleThree.enabled = isActive;
    }

    public override void GetStringStats()
    {
        TowerStatsTxt = "Flame Tower Stats \n" +
            "Attack Range = " + head.GetTowerRange() + "\n" +
            "Attack cone type = " + attackAreaType + "\n" +
            "Attack Damage = " + head.currentTowerDmg + "\n" +
            "Healing Reduction = " + head.healReduction + "\n" +
            "Attack speed = " + "Constant, Damage per sercond. \n" +
            "Damage Type = Spray \n" +
            "Targetting = Area Damage, centered on target.";
    }


    public override float GetTowerCost()
    {
        float towerCost = 0;
        singleton = Singleton.Instance;

        towerCost = (int)TowerCosts.FlameTowerCost;

        float percentToPay = singleton.GetPercentageModifier((int)TinkerUpgradeNumbers.alloyResearch);

        towerCost = towerCost * percentToPay;

        return towerCost;
    }

    public void SetButtonOne(Button buttonOne, Text text)
    {
        text.text = "Upgrade dmg";
        buttonOne.onClick.AddListener(GetStringStats);
    }


    //towerUpgradeDescriptionOne = "Upgrade tower Damage +20%";
    //towerUpgradeDescriptionTwo = "Upgrade reduce enemy healing +10%";
    //towerUpgradeDescriptionThree = "Upgrade tower AOE +15%";
    public override void InitializeUpgradeOptionTexts(ref string description1, ref string description2, ref string description3, ref string stats)
    {
        description1 = towerUpgradeDescriptionOne;
        description2 = towerUpgradeDescriptionTwo;
        description3 = towerUpgradeDescriptionThree;

        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out description1, out description2, out description3);
    }
    private void GetUpgradeCosts(out string upgradeTextOne, out string upgradeTextTwo, out string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        // this shenanigins, multiplies the base cost of the tower times .03x the times its EVER been upgraded, + .05 times the specific upgrade, + the .20% starting upgrade base.
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        string newExplanation = towerUpgradeDescriptionOne + "\nCost: " + currentUpgradeCost;
        upgradeTextOne = newExplanation;

        currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeTwoUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionTwo + "\nCost: " + currentUpgradeCost;
        upgradeTextTwo = newExplanation;

        currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));
        newExplanation = towerUpgradeDescriptionThree + "\nCost: " + currentUpgradeCost;
        upgradeTextThree = newExplanation;
    }

    public override void UpgradeBtnOne(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeOneUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        if (gold.upgradeCount < currentUpgradeCost)
        {
            print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost);
            //return;   Eventually this will stop it.
        }

        head.BuffDamage(.2f);

        gold.UpgradeCost(currentUpgradeCost);
        upgradeOneUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }
    public override void UpgradeBtnTwo(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeTwoUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        if (gold.upgradeCount < currentUpgradeCost)
        {
            print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost);
            //return;   Eventually this will stop it.
        }

        head.healReduction += healReductionIncrement;

        gold.UpgradeCost(currentUpgradeCost);
        upgradeTwoUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }
    public override void UpgradeBtnThree(ref string stats, ref string upgradeTextOne, ref string upgradeTextTwo, ref string upgradeTextThree)
    {
        float baseCost = GetTowerCost();
        int currentUpgradeCost = Mathf.RoundToInt((engineeringCostReduction * (baseCost * ((float)anyUpgradeUsed * anyUpgradeCostInc)) + (baseCost * ((float)upgradeThreeUsed * thisUpgradeCostInc))) + (baseCost * baseUpgradePercent));

        if (gold.upgradeCount < currentUpgradeCost)
        {
            print("Shouldnt allow, not enough parts!!! " + gold.upgradeCount + " < " + currentUpgradeCost);
            //return;   Eventually this will stop it.
        }

        currentAttackRange += (.2f * attackRange);
        head.BuffRange(.15f);

        gold.UpgradeCost(currentUpgradeCost);
        upgradeThreeUsed++;
        anyUpgradeUsed++;
        GetStringStats();
        stats = TowerStatsTxt;

        GetUpgradeCosts(out upgradeTextOne, out upgradeTextTwo, out upgradeTextThree);
    }

}
