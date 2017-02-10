using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamingViewCtr : MonoBehaviour
{
    public SceneMgr SceneMgr;
    public PauseViewCtr PauseView;
    public List<UILabel> ScoreLabels;

    public void Initialize()
    {
        SceneCtr scene = SceneMgr.ActiveScene;
        if (scene != null)
        {
            scene.OnPlayerScoreChanged.Add(OnPlayerScoreChanged);

            scene.Players.ForEach((player) => 
            {
                if (player.id < ScoreLabels.Count)
                {
                    ScoreLabels[player.id].text = player.score.ToString();
                }
            });
        }
    }

    public void UnInitialize()
    {
        SceneCtr scene = SceneMgr.ActiveScene;
        if (scene != null)
        {
            scene.OnPlayerScoreChanged.Remove(OnPlayerScoreChanged);
        }

        ScoreLabels.ForEach((label) => 
        {
            label.text = string.Empty;
        });
    }

    public void PauseClick()
    {
        Debug.Log("PauseClick");

        PauseView.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

        SceneMgr.Pause(true);
    }

    public void OnPlayerScoreChanged(Player player, int score)
    {
        if (player.id < ScoreLabels.Count)
        {
            ScoreLabels[player.id].text = score.ToString();
        }
    }
}
