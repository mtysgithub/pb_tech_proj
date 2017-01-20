using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamingViewCtr : MonoBehaviour
{
    public SceneMgr SceneMgr;
    public List<UILabel> ScoreLabels;

    public void Initialize()
    {
        SceneCtr scene = SceneMgr.ActiveScene;
        if (scene != null)
        {
            scene.OnPlayerScoreChanged.Add(OnPlayerScoreChanged);
        }
    }

    public void UnInitialize()
    {
        SceneCtr scene = SceneMgr.ActiveScene;
        if (scene != null)
        {
            scene.OnPlayerScoreChanged.Remove(OnPlayerScoreChanged);
        }
    }

    public void PauseClick()
    {
        Debug.Log("PauseClick");

    }

    public void OnPlayerScoreChanged(Player player, int score)
    {
        if (ScoreLabels.Count > player.id)
        {
            ScoreLabels[player.id].text = score.ToString();
        }
    }
}
