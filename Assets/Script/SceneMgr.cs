using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public sealed class SceneMgr
{
    public SceneCtr ActiveScene
    {
        get;
        private set;
    }

    private static SceneMgr _instance = null;
    public static SceneMgr Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new SceneMgr();
            }
            return _instance;
        }
    }

    public SceneCtr Make(int size, float density /*[0f,1f]*/)
    {
        GameObject tBoardRes = Resources.Load("Prefabs/Board") as GameObject;
        if (tBoardRes != null)
        {
            GameObject tBoard = GameObject.Instantiate(tBoardRes);
            SceneCtr tSceneCtr = tBoard.GetComponent<SceneCtr>();
            tSceneCtr.Intialize(size, density);

            return this.ActiveScene = tSceneCtr;
        }

        return this.ActiveScene = null;
    }

    
}
