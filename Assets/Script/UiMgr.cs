using UnityEngine;
using System.Collections;

public class UiMgr : MonoBehaviour
{
    public SceneMgr SceneMgr;
    public GameObject MainView;
    public GameObject PauseView;
    public GameObject EndView;
    public GameObject GamingView;

    void Awake()
    {
        MainView.SetActive(true);
        PauseView.SetActive(false);
        EndView.SetActive(false);
        GamingView.SetActive(false);
    }

    public void BeginClick()
    {
        Debug.Log("BeginClick");

        if (SceneMgr.ActiveScene == null)
        {
            GameObject tPlayerA = PlayerCtor.Instance.CreatLocalPlayer();
            Player playerA = tPlayerA.GetComponent<Player>();
            playerA.id = 1;

            SceneCtr tScene = SceneMgr.Make(0.05f);
            tScene.InitializePlayer(playerA);

            tScene.Begin();

            MainView.SetActive(false);
            GamingView.SetActive(true);
        }
    }

    public void LoadClick()
    {
        Debug.Log("LoadClick");
    }

    public void QuitClick()
    {
        Debug.Log("ExistClick");
    }
}
