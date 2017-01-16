using UnityEngine;
using System.Collections;

public class Test0 : MonoBehaviour
{
    void Start()
    {
        //Single Test
        GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
        Player playerA = tPlayerA.GetComponent<Player>();
        playerA.id = 1;

        SceneCtr tScene = SceneMgr.Instance.Make(20 * 2, 0.05f);
        tScene.InitializePlayer(playerA);

        tScene.Begin();
    }
}
