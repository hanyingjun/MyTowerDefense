﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChooseNextMissionPath : MonoBehaviour {

    Singleton singleton;

    [SerializeField] Text choiceOneDescription;
    [SerializeField] Text choiceTwoDescription;

    [SerializeField] Button choiceOneBtn;
    [SerializeField] Button choiceTwoBtn;
    public bool isHasChosen = false;
    private bool isLoaded = false;

    public List<int> firstEnemySet = new List<int>();
    public List<int> secondEnemySet = new List<int>();

    private int mostCommonEnemy = 0;
    private float mostCommonEnemyCount = 0f;

    private int percentOfEnemies = 0;

    // Use this for initialization
    void Start () {
        singleton = Singleton.Instance;

        if (!isLoaded)
        {
            GetEnemyPathChoices();
            isHasChosen = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadPathChoices(int[] firstPath, int[] secondPath)
    {
        firstEnemySet.Clear();
        secondEnemySet.Clear();

        firstEnemySet = new List<int>(firstPath);
        secondEnemySet = new List<int>(secondPath);
        // setup the button now.
        CalculateMostCommonEnemy(firstEnemySet);
        choiceOneDescription.text = "We are seeing a lot of " + DetermineEnemyType(mostCommonEnemy)
            + ".  They comprise about " + percentOfEnemies.ToString() + "% of the enemies.";

        CalculateMostCommonEnemy(secondEnemySet);
        choiceTwoDescription.text = "We are seeing a lot of " + DetermineEnemyType(mostCommonEnemy)
            + ".  They comprise about " + percentOfEnemies.ToString() + "% of the enemies.";

        isLoaded = true;
    }

    public void ChooseFirstPath()
    {
        singleton.DecidedPath(firstEnemySet);

        //choiceOneBtn.interactable = false;
        //choiceTwoBtn.interactable = false;

        // set hasChose = true after waves complete? on laod of base or something.
        isHasChosen = true;
        singleton.isHasPickedAPath = true;
    }

    public void ChooseSecondPath()
    {
        singleton.DecidedPath(secondEnemySet);
        //choiceOneBtn.interactable = false;
        //choiceTwoBtn.interactable = false;

        // set hasChose = true after waves complete? on laod of base or something.
        isHasChosen = true;
        singleton.isHasPickedAPath = true;
    }


    private void GetEnemyPathChoices()
    {
        firstEnemySet.Clear();
        secondEnemySet.Clear();
        // being set = to it permanent not at the snapshot
        firstEnemySet = singleton.CreateEnemyList(firstEnemySet);
        CalculateMostCommonEnemy(firstEnemySet);
        choiceOneDescription.text = "We are seeing a lot of " + DetermineEnemyType(mostCommonEnemy)
            + ".  They comprise about " + percentOfEnemies.ToString() + "% of the enemies.";

        secondEnemySet = singleton.CreateEnemyList(secondEnemySet);
        CalculateMostCommonEnemy(secondEnemySet);
        choiceTwoDescription.text = "We are seeing a lot of " + DetermineEnemyType(mostCommonEnemy)
            + ".  They comprise about " + percentOfEnemies.ToString() + "% of the enemies.";
    }


    //gets button information
    private void CalculateMostCommonEnemy(List<int> enemySet)
    {
        Dictionary<int, int> enemyCalc = new Dictionary<int, int>();

        enemyCalc.Clear();
        float enemyCount = 0;
        mostCommonEnemyCount = 0;
        mostCommonEnemy = 0;
        percentOfEnemies = 0;

        // this adds all enemies in the list to a dictionary, compacting them into a dynamic summation of their count.
        foreach (int currentEnemy in enemySet)
        {
            if (enemyCalc.ContainsKey(currentEnemy))
            {
                enemyCalc[currentEnemy] += 1;
                //print("im adding a repeat! enemy # is at " + enemyCalc[currentEnemy]);
            }
            else
            {
                enemyCalc.Add(currentEnemy, 1);
            }
        }

        //this adds total enemy count for %, as well as finds most common enemy.
        foreach(KeyValuePair<int, int> entry in enemyCalc)
        {
           // print("im inside the dictionary loop!");
            enemyCount += entry.Value;
            if(mostCommonEnemyCount < entry.Value)
            {
                mostCommonEnemyCount = entry.Value;
                mostCommonEnemy = entry.Key;               
            }
            //print("enemy numbers " + entry.Value + " for enemy " + entry.Key);
        }
        float floatPercent = ((mostCommonEnemyCount / enemyCount) * 100);
        percentOfEnemies = Mathf.RoundToInt( floatPercent );
        //print("float % = " + floatPercent + " and enemy % =" + percentOfEnemies + "  -- common enemy count = " + mostCommonEnemyCount+ " and enemy count = " + enemyCount);
    }

    public string DetermineEnemyType(int enemy)
    {
        string enemyName = "";
        switch (enemy) {
            case 1:
                enemyName = "Generics";
                break;
            case 2:
                enemyName = "Burrowers";
                break;
            case 3:
                enemyName = "Rollers";
                break;
            case 4:
                enemyName = "Doubles";
                break;


            case 20:
                enemyName = "Slimers";
                break;
            case 21:
                enemyName = "Healers";
                break;

            default:
                enemyName = "Unknown";
                break;
        }

        return enemyName;
    }

}
