using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceBarController : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;          // Playerのオブジェクト
    private GameObject distance;        // 距離プログレスバーのオブジェクト
    private GameObject goal;            // Goalのオブジェクト
    private Slider progressBar;         // プログレスバーのオブジェクト

    // 各種属性
    private float distnum;              // 距離の表示
    private float goalPos;              // Goal地点
    private float playerOffset = 240;   // Playerの初期位置のオフセット

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");
        Debug.Log("Player Position: " + (player.transform.position.z + playerOffset));

        // 距離表示のオブジェクト
        distance = GameObject.Find("DistValue");

        // Goalオブジェクトの取得
        goal = GameObject.Find("GoalPrefab");
        goalPos = goal.transform.position.z;
        Debug.Log("Goal Position: " + (goalPos + playerOffset));

        // プログレスバーのオブジェクトを取得
        progressBar = GameObject.Find("DistanceBar").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update()
    {
        if (player.transform.position.z < goalPos)
        {
            // 距離を計測
            distnum = player.transform.position.z + playerOffset;
            distance.GetComponent<Text>().text = distnum.ToString("f0").PadLeft(6, '0') + "m";

            // ゴールまでの位置をプログレスバーに表示
            progressBar.value = (player.transform.position.z + playerOffset) / (goalPos + playerOffset);
        }
        else
        {
            // 
            distnum = goalPos + playerOffset;
            distance.GetComponent<Text>().text = distnum.ToString("f0").PadLeft(6, '0') + "m";
        }
	}
}