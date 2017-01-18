using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public sealed class SceneMgr : MonoBehaviour
{
    public const int FIXED_RATE_COUNT = 60;

    public SceneCtr ActiveScene
    {
        get;
        private set;
    }

    public UIWidget SceneBox;

    void Awake()
    {
        UnityEngine.Application.targetFrameRate = FIXED_RATE_COUNT;
    }

    public SceneCtr Make(float density /*[0f,1f]*/)
    {
        GameObject tBoardRes = Resources.Load("Prefabs/Board") as GameObject;
        if (tBoardRes != null)
        {
            GameObject tBoard = GameObject.Instantiate(tBoardRes);
            SceneCtr tSceneCtr = tBoard.GetComponent<SceneCtr>();
            tSceneCtr.Intialize(density, SceneBox);

            return this.ActiveScene = tSceneCtr;
        }

        return this.ActiveScene = null;
    }

    
}
