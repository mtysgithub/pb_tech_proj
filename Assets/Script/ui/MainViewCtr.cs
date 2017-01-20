using UnityEngine;
using System.Collections;

public class MainViewCtr : MonoBehaviour
{
    public SceneMgr SceneMgr;
    public GamingViewCtr GamingView;

    public void NewClick()
    {
        Debug.Log("BeginClick");

        if (SceneMgr.ActiveScene == null)
        {
            GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
            Player playerA = tPlayerA.GetComponent<Player>();
            playerA.id = 0;

            SceneCtr tScene = SceneMgr.Make(0.05f);
            tScene.InitializePlayer(playerA);

            tScene.Begin();

            this.gameObject.SetActive(false);
            GamingView.gameObject.SetActive(true);
            GamingView.Initialize();
        }
    }

    public void ContinueClick()
    {
        Debug.Log("LoadClick");
    }

    public void QuitClick()
    {
        Debug.Log("QuitClick");
        UnityEngine.Application.Quit();
    }
}
