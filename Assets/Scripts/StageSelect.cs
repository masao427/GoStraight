using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {
    public void Stage1Start()
    {
        SceneManager.UnloadSceneAsync("Title");
        SceneManager.LoadScene("Stage1");
    }

    public void Stage2Start()
    {
        SceneManager.UnloadSceneAsync("Title");
        SceneManager.LoadScene("Stage2");
    }

    public void Stage3Start()
    {
        SceneManager.UnloadSceneAsync("Title");
        SceneManager.LoadScene("Stage3");
    }
}
