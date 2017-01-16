using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerCtor
{
    private static PlayerCtor _instance = null;
    public static PlayerCtor Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new PlayerCtor();
            }
            return _instance;
        }
    }

    public GameObject CreatLocalPlayer()
    {
        GameObject tPlayerA = GameObject.Instantiate(Resources.Load("Prefabs/LocalPlayer") as GameObject);
        GameObject tHead = GameObject.Instantiate(Resources.Load("Prefabs/Body") as GameObject);
        tPlayerA.AddComponent<LocalPlayerImpl>().SetHead(tHead.GetComponent<HeadCtr>());

        Player tPlayerACtr = tPlayerA.GetComponent<Player>();
        tPlayerACtr.id = 1;

        return tPlayerA;
    }
}
