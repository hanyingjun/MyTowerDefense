﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemySpawner : MonoBehaviour
{

    [Range(0.1f, 120f)]
    [SerializeField]
    private float startSetupTime = 10f;
    private float startupTimer = 0;
    private bool begin = false;
    private bool checkForBoss = true;
    const float originalSecondsBetweenSpawns = 2.00f;
    private EnemyMovement currentEnemy;
    [SerializeField] EnemyMovement enemyPrefab1;
    [SerializeField] EnemyMovement enemyBurrower;
    [SerializeField] EnemyMovement enemyPrefab3;
    [SerializeField] EnemyMovement enemyDoubles;

    [SerializeField] EnemyMovement enemySlimer;
    [SerializeField] EnemyMovement enemyHealer;


    [SerializeField] Transform enemiesLocation;
    [SerializeField] AudioClip enemySpawnAudio;
    [SerializeField] Text win;

    public bool stillAlive = true;
    bool currentlySpawning = false;
    CurrentWave level;
    //public int level = 1;
    int monstersSpawned = 0;
    public List<int> enemyList = new List<int>();
    //Singleton enemyListContainer;

    GoldManagement GM = null;
    bool betweenWaves = false;
    public float timeBetweenWaves = 8.5f;
    public float secondsBetweenSpawns = 2.25f;
    float waveTimer;
    [SerializeField] Slider slider;
    public int enemyCounter = 0;

    public static List<EnemyHealth> EnemyAliveList = new List<EnemyHealth>();
    // Use this for initialization
    /*
     * I Need THESE FOR INJECTION
     *  int maxWave;
     *  timeBetweenWaves -- watch for slider breaking--Maybe have the enemy instantiation come from a reference that is updated each iteration
     *  number of enemy prefabs for an iteration?
     *  enemy prefabs
     * 
    */
    void Start()
    {
        EnemyAliveList.Clear();
        level = FindObjectOfType<CurrentWave>();
        slider.maxValue = timeBetweenWaves;
        win.enabled = false;
        enemyList = Singleton.Instance.GetEnemyList();  //GetComponent<Singleton>().GetEnemyList();
    }

    public IEnumerator SpawnSpecificEnemies() //List<int> enemyList
    {
        // get on stsart the neemy list from singleton.
        foreach (int x in enemyList)
        {

            //print("Here comes a specific enemy!");
            CheckArray(x);
            if (x > 0)
            {
                currentlySpawning = true;
                enemyCounter++;

                switch (x)
                {
                    case 1:
                        currentEnemy = enemyPrefab1;
                        SpawnGenericEnemy();
                        break;
                    case 2:
                        currentEnemy = enemyBurrower;
                        SpawnGenericEnemy();
                        break;
                    case 3:
                        currentEnemy = enemyPrefab3;
                        SpawnGenericEnemy();
                        break;
                    case 4:
                        currentEnemy = enemyDoubles;
                        SpawnGenericEnemy();
                        yield return new WaitForSeconds(.75f);
                        SpawnGenericEnemy();
                        break;


                    case 20:
                        currentEnemy = enemySlimer;
                        SpawnGenericEnemy();
                        break;
                    case 21:
                        currentEnemy = enemyHealer;
                        SpawnGenericEnemy();
                        break;
                }

                yield return new WaitForSeconds(secondsBetweenSpawns);
            }
            else /// not an enemy
            {
                // make < 0 switch?
                if (betweenWaves)
                {
                    level.WaveUpOne();
                    currentlySpawning = false;

                    // This is wait WHILE so its inverse.  wait while wave is less than time between, once this FALSE proceede.
                    yield return new WaitWhile(() => (waveTimer < timeBetweenWaves));
                    betweenWaves = false;
                    waveTimer = 0;

                    //check for boss
                    if (checkForBoss)
                    {
                        CheckForBoss();
                        checkForBoss = false;
                    }
                }
                
            }
        }

        bool isFinal = false;
        FinalWave finalWave = null;
        try
        {
            finalWave = FindObjectOfType<FinalWave>();
            if (finalWave != null)
            {
                isFinal = true;
            } else
            {
                isFinal = false;
                print("Had to tell it im not final :o");
            }
            
        } catch(Exception e )
        {
             isFinal = false;
        }

        print("Waiting for win!");
        // check for win
        while (FindObjectsOfType<EnemyMovement>().Length > 0)
        {
            yield return new WaitForSeconds(1);
        }
        if (stillAlive && isFinal)
        {
            win.enabled = true;
            finalWave.DisplayWinningCongratulations();
            yield return new WaitForSeconds(5f);
            Application.Quit();
        }
        else if (stillAlive)
        {
            win.enabled = true;
        }

        yield return new WaitForSeconds(4);
        FindObjectOfType<LoadNextArea>().LoadNextAreaPostBattle(Singleton.Instance.level);

    }

    private void CheckForBoss()
    {
        try
        {
            BossEnemy boss = FindObjectOfType<BossEnemy>();
            boss.SpawnBoss();
            boss.BuffBossMob();
        }
        catch (Exception e)
        {
            // nothing,  I failed to find and spawn boss, all good he doesnt exist.
        }
    }

    public void SpawnAppropriateEnemy(int enemy)
    {
        switch (enemy)
        {
            case 1:
                currentEnemy = enemyPrefab1;
                SpawnGenericEnemy();
                break;
            case 2:
                currentEnemy = enemyBurrower;
                SpawnGenericEnemy();
                break;
            case 3:
                currentEnemy = enemyPrefab3;
                SpawnGenericEnemy();
                break;
            case 4:

                break;
        }
    }

    public void CheckArray(int Enemy)
    {
        // maybe use global variables 'current enemy' and 'wait time' to set delays or w/e
        // example wait time = timeBetweenWaves
        // maybe case 0 is last wave? or endwave
        //print("checking which guy to spawn.....");
        switch (Enemy)
        {
            case 1:
                currentEnemy = enemyPrefab1;
                break;
            case 2:
                currentEnemy = enemyBurrower;
                break;
            case 3:
                currentEnemy = enemyPrefab3;
                break;

            case -1:
                betweenWaves = true;
                break;
            case -2:
                break;
        }
    }

    public void SpawnGenericEnemy()
    {
        var enemySpawnLoc = Instantiate(currentEnemy, transform.position, Quaternion.identity);
        enemySpawnLoc.transform.parent = enemiesLocation;

        GetComponent<AudioSource>().PlayOneShot(enemySpawnAudio);
    }

    public void StartBattle()
    {
        if (!currentlySpawning)
        {
            begin = true;
            waveTimer = 1;
            StartCoroutine(SpawnSpecificEnemies());
        }
    }

    
    IEnumerator WaitBetweenWaves()
    {
        //yield return new WaitForSeconds(timeBetweenWaves);
        yield return new WaitWhile(() => waveTimer > timeBetweenWaves);
        //if (!currentlySpawning)
        //{
        
            waveTimer = 0;
            //yield return StartCoroutine(ContinualSpawnEnemies());
            //yield return StartCoroutine(SpawnSpecificEnemies());
        //}
    }

    private void CheckGoldReference()
    {
        if (GM == null)
        {
            GM = FindObjectOfType<GoldManagement>();
        }
    }


    
    void Update()
    {
        if (!begin)
        {
            startupTimer += 1 * Time.deltaTime;

            if(startupTimer >= startSetupTime)
            {
                //print("You hsould see me only once");
                StartCoroutine(SpawnSpecificEnemies());
                begin = true;
                slider.value = startSetupTime / startupTimer;

                CheckGoldReference();
                GM.Started();
            }
        }

        if (stillAlive && !currentlySpawning && begin)
        {
            waveTimer += 1 * Time.deltaTime;
        }
        slider.value = waveTimer;


        if (Input.GetKeyDown(KeyCode.Space) && !currentlySpawning)
        {
            if (begin)
            {
                CheckGoldReference();
                GM.AddExtraGoldTimer((timeBetweenWaves - waveTimer));
            } else
            {
                CheckGoldReference();
                GM.Started();
            }
            waveTimer = timeBetweenWaves;
            startupTimer = startSetupTime;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<LoadNextArea>().LoadNextAreaPostBattle(Singleton.Instance.level);
        }

    }


    public void SetDelayedSpawnTime(float delyedTime)
    {
        startSetupTime = delyedTime;
    }

}
