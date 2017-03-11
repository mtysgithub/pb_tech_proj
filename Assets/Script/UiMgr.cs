using UnityEngine;
using System.Collections;

public class UiMgr : MonoBehaviour
{
    public MainViewCtr MainView;
    public PauseViewCtr PauseView;
    public EndViewCtr EndView;
    public GamingViewCtr GamingView;

    public SceneMgr SceneMgr;

    void Awake()
    {
        MainView.gameObject.SetActive(true);
        PauseView.gameObject.SetActive(false);
        EndView.gameObject.SetActive(false);
        GamingView.gameObject.SetActive(false);
    }

    public void Finish(SceneCtr scene)
    {
        this.GamingView.UnInitialize();
        this.GamingView.gameObject.SetActive(false);
        this.EndView.gameObject.SetActive(true);
    }

}
