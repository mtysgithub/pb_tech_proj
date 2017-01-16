using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public abstract class Player : MonoBehaviour
{
    public HeadCtr Head;
    public List<GameObject> Body = new List<GameObject>();
    public int id = 0;

    public Vector3 Speed = Vector3.zero;
    public abstract void Move();

    public virtual void Load(SaveLoadMgr.DataWarpper data) { }

    public void SetHead(HeadCtr head)
    {
        this.Head = head;

        head.gameObject.transform.parent = this.transform;
        head.transform.localPosition = Vector3.zero;
        head.transform.localScale = Vector3.one;

        this.Body.Add(head.gameObject);
    }

    public void SetBeginPos(Vector3 pos)
    {
        this.Head.transform.localPosition = pos;
    }

    public abstract bool OnEat(FoodCellCtr go);

    public virtual bool TouchedOtherPlayer(Player player)
    {
        for (int i = 0; i < player.Body.Count; ++i)
        {
            GameObject tBody = player.Body[i];
            Vector3 tLength = this.Head.transform.localPosition - tBody.transform.localPosition;
            if ((tLength.magnitude < 1f) && (this.Head.gameObject != tBody))
            {
                return true;
            }
        }
        return false;
    }

    public virtual void DoDestroy()
    {
        this.Head = null;
        this.Body.ForEach((GameObject item) => 
        {
            GameObject.Destroy(item);
        });
        this.Body.Clear();
    }

    public virtual void DoDead()
    {
        Debug.Log(string.Format("player_id:{0} dead.", this.id));
        if (UnityEngine.Application.isEditor)
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
    }

    protected GameObject CreateBody()
    {
        GameObject tBody = GameObject.Instantiate(Resources.Load("Prefabs/Body") as GameObject);
        tBody.AddComponent<BodyCellCtr>().id = this.id;
        tBody.transform.parent = this.transform;
        return tBody;
    }

    protected virtual void Update() { }
}
