using UnityEngine;
using System.Collections;
using System;

public class LocalPlayerImpl : Player
{
    protected bool mTouching = false;
    protected Vector2 mLastTouchPos = Vector2.zero;
    protected const float MOV_SUCC_TOUCH_LENG = 3;
    protected const float DETA_MOV_STEP = 1.0f;

    public override void Move()
    {
        Vector3 tOldPos = this.Head.transform.localPosition;
        Vector3 tNewVector3 = new Vector3(tOldPos.x + Speed.x, tOldPos.y + Speed.y, tOldPos.z);
        this.Head.transform.localPosition = tNewVector3;

        if (this.Body.Count > 1)
        {
            for (int i = 1; i < this.Body.Count; ++i)
            {
                Vector3 tmpVar = this.Body[i].transform.localPosition;
                this.Body[i].transform.localPosition = tOldPos;
                tOldPos = tmpVar;
            }
        }
    }

    public override void Load(SaveLoadMgr.DataWarpper data)
    {
        base.Load(data);

        this.id = data.PlayerInf.id;

        if (this.Body.Count > 1)
        {
            for (int i = 1, max = this.Body.Count; i < max; ++i)
            {
                GameObject.Destroy(this.Body[i]);
            }
            this.Body.Clear();
            this.Body.Add(this.Head.gameObject);
        }

        data.PlayerInf.bodys.ForEach((item) =>
        {
            GameObject tBodyGo = this.CreateBody();
            tBodyGo.transform.localPosition = new Vector3(item.x, item.y, item.z);
        });
    }

    public override bool OnEat(FoodCellCtr food)
    {
        int tScore = food.Score;
        for (int i = 0; i < tScore; ++i)
        {
            //append body cell
            GameObject tBody = this.CreateBody();

            if (this.Body.Count == 1)
            {
                Vector3 tPos = -this.Speed;
                Vector3 tHeadPos = this.Head.transform.localPosition;
                tBody.transform.localPosition = new Vector3(tHeadPos.x + tPos.x,
                    tHeadPos.y + tPos.y,
                    tHeadPos.z + tPos.z);
            }
            else
            {
                GameObject tBodyA = this.Body[this.Body.Count - 2];
                GameObject tBodyB = this.Body[this.Body.Count - 1];

                Vector3 tPos = tBodyB.transform.localPosition - tBodyA.transform.localPosition;
                Vector3 tOffset = Vector3.zero;
                if (Mathf.Abs(tPos.x) >= Mathf.Abs(tPos.y))
                {
                    tOffset = (tPos.x > 0) ? (new Vector3(1, 0, tPos.z)) : (new Vector3(-1, 0, tPos.z));
                }
                if (Mathf.Abs(tPos.y) >= Mathf.Abs(tPos.x))
                {
                    tOffset = (tPos.y > 0) ? (new Vector3(0, 1, tPos.z)) : (new Vector3(0, -1, tPos.z));
                }

                tBody.transform.localPosition = new Vector3(tBodyB.transform.localPosition.x + tOffset.x,
                    tBodyB.transform.localPosition.y + tOffset.y, tBodyB.transform.localPosition.z + tOffset.z);
            }
            this.Body.Add(tBody);
        }

        return true;
    }

    protected override void Update()
    {
        base.Update();
        Vector2 tDir = this.MoveGestureJuedge();
        //Debug.Log(string.Format("touch:{0}, {1}", tDir.x, tDir.y));

        if (!tDir.Equals(Vector2.zero))
        {
            Vector3 tNormalized =  new Vector3(tDir.x, tDir.y, 0);
            if (!(-1 * tNormalized).Equals(this.Speed))
            {
                this.Speed = tNormalized;
            }
        }
    }

    protected Vector2 MoveGestureJuedge()
    {
        if (UnityEngine.Application.platform == RuntimePlatform.Android || 
            UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) && !mTouching)
            {
                mLastTouchPos = Input.GetTouch(0).position;
                mTouching = true;
            }
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                mTouching = false;
            }

            if (mTouching)
            {
                Vector2 tCurTouchPos = Input.GetTouch(0).position;
                Vector2 tDir = tCurTouchPos - mLastTouchPos;
                float tLength = (tDir).magnitude;

                if (tLength >= MOV_SUCC_TOUCH_LENG)
                {
                    mTouching = false;

                    tDir = -1 * tDir;
                    if (Mathf.Abs(tDir.x) >= Mathf.Abs(tDir.y))
                    {
                        return (tDir.x > 0) ? (new Vector2(1, 0)) : (new Vector2(-1, 0));
                    }
                    else
                    {
                        return (tDir.y > 0) ? (new Vector2(0, 1)) : (new Vector2(0, -1));
                    }
                }
            }
        }
        else
        {
            //use keyboard
            if (Input.GetKeyDown(KeyCode.W))
            {
                return new Vector2(0, DETA_MOV_STEP);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                return new Vector2(0, -DETA_MOV_STEP);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                return new Vector2(-DETA_MOV_STEP, 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                return new Vector2(DETA_MOV_STEP, 0);
            }
        }

        return Vector2.zero;
    }
}
