﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveSerializedObject  {

    public bool[] towerList;
    public int currentLevel = 1;

    public SaveSerializedObject(bool[] towerListSaves)
    {
        towerList = towerListSaves;
    }
    public void UpdateCurrentLevel()
    {
        
    }

    //public void UpdateTowerList(bool[] newTowerList)
    //{
    //    towerList = newTowerList;
    //}
    //Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
}