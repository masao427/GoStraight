using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {
    public void Stage1Start()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void Stage2Start()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void Stage3Start()
    {
        SceneManager.LoadScene("Stage3");
    }
}
