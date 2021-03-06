﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowerHealth : EnemyHealth {

    protected BurrowerMovement burrowerMove;
    protected bool burrowed = false;
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        enemyName = "Burrower";
        burrowerMove = GetComponent<BurrowerMovement>();
        hitPoints = hitPoints * .65f;
        hitPointsMax = hitPoints;
        burrowed = burrowerMove.burrowed;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    public void Burrowed()
    {
        burrowed = true;
    }
    public void Unburrowed()
    {
        burrowed = false;
    }

    public void TellMovementToStartBurrow()
    {
        burrowerMove.IWasHit();
    }

    override public void HitByNonProjectile(float damage, string towerName)
    {
        if (burrowed) // cant shoot me im underground bitch.
        {
            return;
        }

        float dmg = damage;
        hitPoints = hitPoints - dmg;
        healthImage.fillAmount = (hitPoints / hitPointsMax);
        TellMovementToStartBurrow();
        hitparticleprefab.Play();
        Singleton.AddTowerDamage(towerName, damage);

        if (hitPoints <= 0)
        {
            // if it has already been killed and is waiting for cleanup / dlete, dont double dip gold.

            //Adds gold upon death, then deletes the enemy.
            KillsEnemyandAddsGold();
            damageLog.UpdateDamageAndKills(towerName, damage, enemyName);
        }
        else
        {
            damageLog.UpdateDamage(towerName, damage);
            GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
        }
    }

    override protected void OnParticleCollision(GameObject other)
    {
        if (burrowed) // cant shoot me im underground bitch.
        {
            return;
        }

        string towerName = "";
        float dmg = 0;
        dmg = other.GetComponentInParent<Tower>().Damage(ref towerName);
        ProcessHit(dmg, towerName);

        healthImage.fillAmount = (hitPoints / hitPointsMax);
        TellMovementToStartBurrow();
        if (hitPoints <= 0)
        {
            //Adds gold upon death, then deletes the enemy.
            damageLog.UpdateKills(towerName, enemyName);
            KillsEnemyandAddsGold();
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
        }
    }

    override public IEnumerator Burning(float fireDmg)
    {
        //burning underground sucks, but ill allow it ATM.
        if (burrowed)
            yield return new WaitForSeconds(1f);

        if (hitPoints < 1)
        {
            KillsEnemyandAddsGold();
        }
        if (onFire && time > 0)
        {
            time -= 1 * Time.deltaTime;
            hitPoints -= burnDmg * Time.deltaTime;
            TellMovementToStartBurrow();
            healthImage.fillAmount = (hitPoints / hitPointsMax);
        }
        else
        {
            onFire = false;
        }
        yield return new WaitForSeconds(1f);
    }

}
