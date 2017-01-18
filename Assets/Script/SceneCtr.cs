using UnityEngine;
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

    private Dictionary<int, UnityEngine.Object> ResCache = new Dictionary<int, UnityEngine.Object>();

    private int mRateCnt = 0;
    private bool mIsPaused = false;

    public void Intialize(float density /*[0f,1f]*/, UIWidget box)
    {
        this.transform.parent = box.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;

        var tSizeVec = box.localSize;
        this.WidthSize = (int)tSizeVec.x;
        this.HeightSize = (int)tSizeVec.y;

        System.Random tRandomer = new System.Random((int)DateTime.Now.Ticks);

        int tCellCntOnWidth = this.WidthSize / CELL_SIZE;
        int tCellCntOnHeight = this.HeightSize / CELL_SIZE;
        for (int i = 0; i < tCellCntOnWidth; ++i)
        {
            for (int j = 0; j < tCellCntOnHeight; ++j)
            {
                if (i == 0 && j == 0) continue;

                float tNum = (tRandomer.Next(0, 100)) * 1.0f / 100.0f;
                if (tNum < density)
                {
                    int tX = i * CELL_SIZE - (tCellCntOnWidth / 2) * CELL_SIZE;
                    int tY = j * CELL_SIZE - (tCellCntOnHeight / 2) * CELL_SIZE;

                    GameObject tFoodGo = this.CreateFood(new Vector3(tX, tY, 1f), 1);
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
            playerA.SetBeginPos(new Vector3(0f, 0f, 1f));
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
        //using default
        GameObject tFood = this.DoMakeFood(1);
        if (tFood != null)
        {
            tFood.AddComponent<FoodCellCtr>();
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
    }

    protected void Update()
    {
        this.OnTick();
    }
}
