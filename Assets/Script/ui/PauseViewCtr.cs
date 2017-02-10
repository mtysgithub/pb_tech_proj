using UnityEngine;
using System.Collections.Generic;

public class PauseViewCtr : MonoBehaviour
{
    public SceneMgr SceneMgr;
    public GamingViewCtr GamingView;
    public MainViewCtr MainView;
    public List<UILabel> ScoreLabels;

    void OnEnable()
    {
        SceneCtr scene = SceneMgr.ActiveScene;
        if (scene != null)
        {
            scene.Players.ForEach((player) => 
            {
                var id = player.id;
                if (id < ScoreLabels.Count)
                {
                    ScoreLabels[id].text = player.score.ToString();
                }
            });
        }
    }

    void OnDisable()
    {
        ScoreLabels.ForEach((label) => { label.text = string.Empty; });
    }

    public void GoOnClick()
    {
        this.gameObject.SetActive(false);
        this.GamingView.gameObject.SetActive(true);

        SceneMgr.Pause(false);
    }

    public void FinishClick()
    {
        SceneMgr.SaveLocalGame();
        SceneMgr.Stop();

        this.GamingView.UnInitialize();
        this.gameObject.SetActive(false);
        this.MainView.gameObject.SetActive(true);

    }
}
