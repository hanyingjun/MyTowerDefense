﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    [Range(0.1f, 120f)]
    [SerializeField]
    float secondsBetweenSpawns = 2.00f;
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

    bool betweenWaves = false;
    float timeBetweenWaves = 12f;
    float waveTimer;
    [SerializeField] Slider slider;

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
        level = FindObjectOfType<CurrentWave>();
        slider.maxValue = timeBetweenWaves;
        win.enabled = false;
        
        enemyList = FindObjectOfType<Singleton>().GetEnemyList();  //GetComponent<Singleton>().GetEnemyList();
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
                //var enemySpawnLoc = Instantiate(currentEnemy, transform.position, Quaternion.identity);
                //enemySpawnLoc.transform.parent = enemiesLocation;

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
                //GetComponent<AudioSource>().PlayOneShot(enemySpawnAudio);

                yield return new WaitForSeconds(secondsBetweenSpawns);
            }
            else /// not an enemy
            {
                // make < 0 switch?
                if (betweenWaves)
                {
                    level.WaveUpOne();
                    currentlySpawning = false;
                    betweenWaves = false;
                    yield return StartCoroutine(WaitBetweenWaves());
                }
                
            }
        }

        // check for win
        while (FindObjectsOfType<EnemyMovement>().Length > 0)
        {
            yield return new WaitForSeconds(1);
        }
        if (stillAlive)
        {
            win.enabled = true;
        }

        yield return new WaitForSeconds(4);
        FindObjectOfType<LoadNextArea>().LoadBase();

        yield return StartCoroutine(WaitBetweenWaves());
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
        //print("talks over time to fight!");
        if (!currentlySpawning)
        {
            waveTimer = 0;
            StartCoroutine(SpawnSpecificEnemies());
        }
    }
    //Get path on start so that you cant build towers wrongly
    //public IEnumerator ContinualSpawnEnemies()
    //{
    //    // resets the timer when a wave is spawned.
    //    waveTimer = 0;

    //    while (monstersSpawned < 6 && stillAlive && level.waveCount < 5)
    //    {
    //        currentlySpawning = true;
    //        var enemySpawnLoc = Instantiate(enemyPrefab1, transform.position, Quaternion.identity);
    //        enemySpawnLoc.transform.parent = enemiesLocation;
    //        monstersSpawned++;
    //        GetComponent<AudioSource>().PlayOneShot(enemySpawnAudio);
    //        yield return new WaitForSeconds(secondsBetweenSpawns);
    //    }
    //    FindObjectOfType<CurrentWave>().WaveUpOne();
    //    currentlySpawning = false;
    //    monstersSpawned = 0;

    //    // If on Last wave, check enemy number for win.
    //    if (level.waveCount == 5)
    //    {
    //        while(FindObjectsOfType<EnemyMovement>().Length > 0)
    //        {
    //            yield return new WaitForSeconds(1);
    //        }
    //        if(stillAlive)
    //            win.enabled = true;
    //        yield return new WaitForSeconds(4);
    //        FindObjectOfType<LoadNextArea>().LoadBase();
    //    }
        
    //    yield return StartCoroutine(WaitBetweenWaves());
    //}

    IEnumerator WaitBetweenWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        if (!currentlySpawning)
        {
            waveTimer = 0;
            //yield return StartCoroutine(ContinualSpawnEnemies());
            yield return StartCoroutine(SpawnSpecificEnemies());
        }
    }

    
    void Update()
    {
        if (stillAlive && !currentlySpawning)
        {
            waveTimer += 1 * Time.deltaTime;
        }
        slider.value = waveTimer;


        if (Input.GetKeyDown(KeyCode.Space) && !currentlySpawning)
        {
            waveTimer = 0;
            currentlySpawning = true;
            betweenWaves = false;
            //StartCoroutine(ContinualSpawnEnemies());
            StartCoroutine(SpawnSpecificEnemies());
        }
    }

}

// continue seperation add HP back to serialized field.




// bad for loop
//  monstersSpawned = 0; monstersSpawned < 5; monstersSpawned ++