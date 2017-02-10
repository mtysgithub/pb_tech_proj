using UnityEngine;
using System.Collections;

public class MainViewCtr : MonoBehaviour
{
    public SceneMgr SceneMgr;
    public GamingViewCtr GamingView;

    public void NewClick()
    {
        Debug.Log("BeginClick");

        if (SceneMgr.NewLocalGame())
        {
            this.gameObject.SetActive(false);
            GamingView.gameObject.SetActive(true);
            GamingView.Initialize();
        }
    }

    public void ContinueClick()
    {
        Debug.Log("LoadClick");
        if (SceneMgr.ContinueLocalGame())
        {
            this.gameObject.SetActive(false);
            GamingView.gameObject.SetActive(true);
            GamingView.Initialize();
        }
    }

    public void QuitClick()
    {
        Debug.Log("QuitClick");
        UnityEngine.Application.Quit();
    }
}
