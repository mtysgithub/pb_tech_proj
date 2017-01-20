using UnityEngine;
using System.Collections;

public class UiMgr : MonoBehaviour
{
    public MainViewCtr MainView;
    public PauseViewCtr PauseView;
    public EndViewCtr EndView;
    public GamingViewCtr GamingView;

    void Awake()
    {
        MainView.gameObject.SetActive(true);
        PauseView.gameObject.SetActive(false);
        EndView.gameObject.SetActive(false);
        GamingView.gameObject.SetActive(false);
    }
}
