using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

using dataconfig;

public class SceneCtr : MonoBehaviour
{
    public Camera Camera;
    public GameObject BorderPanel;
    public Transform FoodsCntr;

    public List<GameObject> Foods;
    public List<Player> Players;

    private int mSize = 0;
    private bool IsPaused = false;
    private float mLastMovTimeStamp = 0f;
    private const float SPEED = 0.1f;

    public void Intialize(int size, float density /*[0f,1f]*/)
    {
        this.mSize = size;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;


        this.Camera.orthographicSize = size / 2;
        this.BorderPanel.transform.localScale = new Vector3(size, size, size);

        System.Random tRandomer = new System.Random((int)DateTime.Now.Ticks);

        for (int i = 1; i < size; ++i)
        {
            for (int j = 1; j < size; ++j)
            {
                if ((i == 1) && (j == 1)) continue;
                if ((i == size - 1) && (j == size - 1)) continue;

                float tNum = (tRandomer.Next(0, 100)) * 1.0f / 100.0f;
                if (tNum < density)
                {
                    int tX = i - size / 2;
                    int tY = j - size / 2;

                    GameObject tFoodGo = this.CreateFood(new Vector3(tX, tY, 1f), 1);
                }
            }
        }
    }

    public void InitializePlayer(Player playerA, Player playerB = null)
    {
        this.IsPaused = false;
        int size = this.mSize;

        if (playerA != null)
        {
            playerA.transform.parent = this.transform;
            playerA.transform.localPosition = Vector3.zero;
            playerA.SetBeginPos(new Vector3(1 - size / 2, 1 - size / 2, 1f)); //左下角
            this.Players.Add(playerA);
        }
        if (playerB != null)
        {
            playerB.transform.parent = this.transform;
            playerB.transform.localPosition = Vector3.zero;
            playerB.SetBeginPos(new Vector3((size - 1) - size / 2, (size - 1) - size / 2, 1f)); //右上角
            this.Players.Add(playerB);
        }
    }

    public void Load(SaveLoadMgr.DataWarpper datas)
    {
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

        datas.FoodsInf.ForEach((item) => 
        {
            Vector3 tPos = new Vector3(item.position.x, item.position.y, item.position.z);
            GameObject tFoodGo = this.CreateFood(tPos, item.score);
        });

        GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
        Player playerA = tPlayerA.GetComponent<Player>();
        playerA.id = 1;

        this.InitializePlayer(playerA);
        playerA.Load(datas);
    }

    private GameObject CreateFood(Vector3 pos, int score)
    {
        GameObject tFood = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tFood.AddComponent<FoodCellCtr>();
        tFood.transform.parent = this.FoodsCntr;
        tFood.transform.localScale = Vector3.one;
        tFood.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        Foods.Add(tFood);
        return tFood;
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

    private void OnTick()
    {
        float tVar = Time.realtimeSinceStartup - mLastMovTimeStamp;

        if (tVar >= SPEED)
        {
            if (!this.IsPaused)
            {
                for (int i = 0, tMax = Players.Count; i < tMax; ++i)
                {
                    Player player = this.Players[i];
                    player.Move();

                    FoodCellCtr tHadEatedFood = this.CheckEat(player.Head.transform);
                    if (tHadEatedFood != null)
                    {
                        bool tEatted = player.OnEat(tHadEatedFood);

                        if (tEatted)
                        {
                            //do clear
                            this.Foods.Remove(tHadEatedFood.gameObject);
                            GameObject.Destroy(tHadEatedFood.gameObject);
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
                                }
                            }
                        }
                    }
                }
            }

            mLastMovTimeStamp = Time.realtimeSinceStartup;
        }
    }

    public void Begin()
    {
        StopAllCoroutines();
        InvokeRepeating("OnTick", 0, 0.001f);

        mLastMovTimeStamp = Time.realtimeSinceStartup;
    }

    public void Pause(bool pause)
    {
        this.IsPaused = pause;
    }

    public void Stop()
    {
        StopAllCoroutines();
    }
}
