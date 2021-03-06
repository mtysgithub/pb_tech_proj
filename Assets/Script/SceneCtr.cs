﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

using dataconfig;

public class SceneCtr : MonoBehaviour
{
    public const int CELL_SIZE = 20;

    public Transform FoodsCntr;

    public List<GameObject> Foods;
    public List<Player> Players;

    public int WidthSize = 0;
    public int HeightSize = 0;

    public SceneMgr SceneMgr;

    public List<Action<Player, int>> OnPlayerScoreChanged = new List<Action<Player, int>>();

    private Dictionary<int, UnityEngine.Object> ResCache = new Dictionary<int, UnityEngine.Object>();

    private int mRateCnt = 0;
    private bool mIsPaused = false;

    public void New(SceneMgr sceneMgr, dataconfig.GAMEPARAMSCONFIG_CONF_ARRAY tConfig, /*[0f,1f]*/ UIWidget box)
    {
        List<int> tScoreDefList = new List<int>() { 1 };
        var tDensity = 0.05f;
        if ((tConfig != null) && (tConfig.items.Count > 0))
        {
            var tConfItem = tConfig.items[0];
            tDensity = tConfItem.food_density;
            tScoreDefList = tConfItem.foods_socre_def;
        }

        this.InitalizeBoard(sceneMgr, box);

        System.Random tRandomer = new System.Random((int)DateTime.Now.Ticks);

        int tCellCntOnWidth = this.WidthSize / CELL_SIZE;
        int tCellCntOnHeight = this.HeightSize / CELL_SIZE;
        for (int i = 0; i < tCellCntOnWidth; ++i)
        {
            for (int j = 0; j < tCellCntOnHeight; ++j)
            {
                if (i == 0 && j == 0) continue;

                float tNum = (tRandomer.Next(0, 100)) * 1.0f / 100.0f;
                if (tNum < tDensity)
                {
                    int tX = i * CELL_SIZE - (tCellCntOnWidth / 2) * CELL_SIZE;
                    int tY = j * CELL_SIZE - (tCellCntOnHeight / 2) * CELL_SIZE;

                    var tScoreDefIdx = tRandomer.Next(0, tScoreDefList.Count - 1);
                    GameObject tFoodGo = this.CreateFood(new Vector3(tX, tY, 1f), tScoreDefList[tScoreDefIdx]);
                }
            }
        }
    }

    public void InitializePlayer(Player playerA, Player playerB = null)
    {
        this.mIsPaused = false;

        if (playerB == null)
        {
            playerA.transform.parent = this.transform;
            playerA.transform.localPosition = Vector3.zero;
            playerA.transform.localScale = Vector3.one;
            this.Players.Add(playerA);
        }
        else
        {
            throw new NotImplementedException();
        }

        //if (playerA != null)
        //{
        //    playerA.transform.parent = this.transform;
        //    playerA.transform.localPosition = Vector3.zero;
        //    playerA.SetBeginPos(new Vector3(1 - size / 2, 1 - size / 2, 1f)); //左下角
        //    this.Players.Add(playerA);
        //}
        //if (playerB != null)
        //{
        //    playerB.transform.parent = this.transform;
        //    playerB.transform.localPosition = Vector3.zero;
        //    playerB.SetBeginPos(new Vector3((size - 1) - size / 2, (size - 1) - size / 2, 1f)); //右上角
        //    this.Players.Add(playerB);
        //}
    }

    public void Load(SaveLoadMgr.DataWarpper datas, SceneMgr sceneMgr, UIWidget box)
    {
        this.Clear();
        this.InitalizeBoard(sceneMgr, box);

        datas.FoodsInf.ForEach((item) => 
        {
            Vector3 tPos = new Vector3(item.position.x, item.position.y, item.position.z);
            GameObject tFoodGo = this.CreateFood(tPos, item.score);
        });
    }

    private void InitalizeBoard(SceneMgr sceneMgr, UIWidget box)
    {
        this.SceneMgr = sceneMgr;

        this.transform.parent = box.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;

        var tSizeVec = box.localSize;
        this.WidthSize = (int)tSizeVec.x;
        this.HeightSize = (int)tSizeVec.y;
    }

    private GameObject CreateFood(Vector3 pos, int score)
    {
        //using default
        GameObject tFood = this.DoMakeFood(1);
        if (tFood != null)
        {
            var ctr = tFood.GetComponent<FoodCellCtr>();
            ctr.Score = score;
            tFood.transform.parent = this.FoodsCntr;
            tFood.transform.localScale = Vector3.one;
            tFood.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
            Foods.Add(tFood);
        }
        return tFood;
    }

    private GameObject DoMakeFood(int id = 1)
    {
        GameObject tRet = null;
        UnityEngine.Object tRes = null;

        //load cache first
        if (this.ResCache.ContainsKey(id))
        {
            tRes = this.ResCache[id];
        }

        // load from device
        if (tRes == null)
        {
            tRes = Resources.Load("Prefabs/Food/Food_" + id.ToString());
            if (tRes != null)
            {
                this.ResCache.Add(id, tRes);
            }
        }

        if (tRes != null)
        {
            tRet = GameObject.Instantiate(tRes as GameObject);
        }
        return tRet;
    }

    private FoodCellCtr CheckEat(Transform head)
    {
        FoodCellCtr tRet = null;
        for (int i = 0, max = this.Foods.Count; i < max; ++i)
        {
            GameObject tFoodGo = this.Foods[i];
            Vector3 tV1 = tFoodGo.transform.localPosition;
            Vector3 tV2 = head.transform.localPosition;
            Vector3 tLeng = tV1 - tV2;
            if (tLeng.magnitude < 1f)
            {
                tRet = tFoodGo.GetComponent<FoodCellCtr>();
                //Debug.Log(string.Format("吃到_{2}:{0} {1}", tRet.transform.localPosition.x,
                //    tRet.transform.localPosition.y, i));
            }
        }
        return tRet;
    }

    private bool CheckOut(Transform head)
    {
        if ((Mathf.Abs(head.localPosition.x ) > (this.WidthSize / 2)) || 
            (Mathf.Abs(head.localPosition.y) > (this.HeightSize / 2)))
        {
            return true;
        }
        return false;
    }

    private void OnTick()
    {
        bool dead = false;
        bool useup = false;

        ++mRateCnt;
        mRateCnt %= (10);

        if (!this.mIsPaused && (mRateCnt == 0))
        {
            for (int i = 0, tMax = Players.Count; i < tMax; ++i)
            {
                Player player = this.Players[i];
                player.Move();

                if (this.CheckOut(player.Head.transform))
                {
                    player.DoDead();
                    dead = true;
                }
                else
                {
                    FoodCellCtr tHadEatedFood = this.CheckEat(player.Head.transform);
                    if (tHadEatedFood != null)
                    {
                        bool tEatted = player.OnEat(tHadEatedFood);

                        if (tEatted)
                        {
                            //do clear
                            this.Foods.Remove(tHadEatedFood.gameObject);
                            GameObject.Destroy(tHadEatedFood.gameObject);

                            OnPlayerScoreChanged.ForEach((cbk) => 
                            {
                                if (cbk != null)
                                {
                                    cbk(player, player.score);
                                }
                            });
                        }
                    }
                    else
                    {
                        //kill check
                        for (int j = 0; j < this.Players.Count; ++j)
                        {
                            Player tPlayerA = this.Players[j];
                            for (int k = 0; k < this.Players.Count; ++k)
                            {
                                Player tPlayerB = this.Players[k];
                                if (tPlayerA.TouchedOtherPlayer(tPlayerB))
                                {
                                    tPlayerA.DoDead();
                                    dead = true;
                                }
                            }
                        }
                    }
                }

                //Debug.Log(string.Format("id{0} 得分{1}", player.id, player.score));
            }

            useup = this.Foods.Count == 0;
            Debug.Log("剩余 " + this.Foods.Count);
        }

        if (dead)
        {
            this.DoGameDead();
        }
        else if (useup)
        {
            this.DoGameFinish();
        }
    }

    public void Begin()
    {
        this.mIsPaused = false;
    }

    public void Pause(bool pause)
    {
        this.mIsPaused = pause;
    }

    public void Stop()
    {
        this.mIsPaused = true;
        this.Clear();
    }

    protected void Update()
    {
        this.OnTick();
    }

    protected virtual void DoGameDead()
    {
        Debug.Log("DoGameDead");
        SceneMgr.Finish(this);
    }

    protected virtual void DoGameFinish()
    {
        Debug.Log("Finish");
        SceneMgr.Finish(this);
    }

    protected virtual void Clear()
    {
        OnPlayerScoreChanged.Clear();
        mRateCnt = 0;

        this.Foods.ForEach((GameObject item) =>
        {
            GameObject.Destroy(item);
        });
        this.Foods.Clear();

        this.Players.ForEach((Player item) =>
        {
            item.DoDestroy();
        });
        this.Players.Clear();
    }
}
