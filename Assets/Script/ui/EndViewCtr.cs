using UnityEngine;
using System.Collections.Generic;

public class EndViewCtr : MonoBehaviour
{
    public SceneMgr SceneMgr;
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

    public void FinishClick()
    {
        SceneMgr.Stop();
        this.gameObject.SetActive(false);
        this.MainView.gameObject.SetActive(true);
    }

}
