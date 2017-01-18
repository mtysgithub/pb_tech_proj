using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    public SceneMgr SceneMgr;

    void Start()
    {
        //Single Test
        GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
        Player playerA = tPlayerA.GetComponent<Player>();
        playerA.id = 1;

        SceneCtr tScene = SceneMgr.Make(0.05f);
        tScene.InitializePlayer(playerA);

        tScene.Begin();
    }
}
