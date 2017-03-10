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

    public UiMgr UiMgr;
    public UIWidget SceneBox;

    void Awake()
    {
        UnityEngine.Application.targetFrameRate = FIXED_RATE_COUNT;
    }

    public SceneCtr Make(dataconfig.GAMEPARAMSCONFIG_CONF_ARRAY config)
    {
        GameObject tBoardRes = Resources.Load("Prefabs/Board") as GameObject;
        if (tBoardRes != null)
        {
            GameObject tBoard = GameObject.Instantiate(tBoardRes);
            SceneCtr tSceneCtr = tBoard.GetComponent<SceneCtr>();
            tSceneCtr.New(this, config, SceneBox);

            return tSceneCtr;
        }

        return null;
    }

    public SceneCtr Make(SaveLoadMgr.DataWarpper data)
    {
        GameObject tBoardRes = Resources.Load("Prefabs/Board") as GameObject;
        if (tBoardRes != null)
        {
            GameObject tBoard = GameObject.Instantiate(tBoardRes);
            SceneCtr tSceneCtr = tBoard.GetComponent<SceneCtr>();
            tSceneCtr.Load(data, this, SceneBox);

            return tSceneCtr;
        }

        return null;
    }

    public bool NewLocalGame()
    {
        if (this.ActiveScene == null)
        {
            GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
            Player playerA = tPlayerA.GetComponent<Player>();
            playerA.id = 0;

            SceneCtr tScene = this.Make(SaveLoadMgr.Instance.LoadConfig());
            if (tScene != null)
            {
                tScene.InitializePlayer(playerA);
                tScene.Begin();
                this.ActiveScene = tScene;
                return true;
            }
        }
        this.ActiveScene = null;
        return false;
    }

    public bool ContinueLocalGame()
    {
        if (this.ActiveScene == null)
        {
            var data = SaveLoadMgr.Instance.Load();
            if (data != null)
            {
                GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
                Player playerA = tPlayerA.GetComponent<Player>();
                playerA.Load(data);

                SceneCtr tScene = this.Make(data);
                if (tScene != null)
                {
                    tScene.InitializePlayer(playerA);
                    tScene.Begin();
                    this.ActiveScene = tScene;

                    //UnityEditor.EditorApplication.isPaused = true;

                    return true;
                }
            }
        }
        this.ActiveScene = null;
        return false;
    }

    public void SaveLocalGame()
    {
        var scene = this.ActiveScene;
        if (scene != null)
        {
            SaveLoadMgr.Instance.Save(scene);
        }
    }

    public void Pause(bool pause)
    {
        SceneCtr scene = this.ActiveScene;
        if (scene != null)
        {
            scene.Pause(pause);
        }

    }

    public void Stop()
    {
        SceneCtr scene = this.ActiveScene;
        if (scene != null)
        {
            scene.Stop();
        }

        GameObject.Destroy(scene.gameObject);
        this.ActiveScene = null;
    }

    public void GameDead(SceneCtr scene)
    {
        this.Pause(true);
        UiMgr.GameDead(scene);
    }
}
