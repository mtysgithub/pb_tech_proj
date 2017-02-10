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
        GameObject tPlayerA = GameObject.Instantiate(Resources.Load("Prefabs/Snake/LocalPlayer") as GameObject);

        Player tPlayerACtr = tPlayerA.GetComponent<Player>();
        tPlayerACtr.id = 1;
        tPlayerACtr.Head.transform.localPosition = new Vector3(0f, 0f, 1f);

        return tPlayerA;
    }
}
