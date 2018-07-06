using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDestroyer : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;  // Playerオブジェクト取得

    // 各種属性
    private int delPosZ = 260;  // Playerの通過ポイント
    private float goalPos;      // Goal地点

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        this.player = GameObject.Find("Player");

        // GoalのZ軸ポジション
        goalPos = GameObject.Find("GoalPrefab").transform.position.z;
	}
	
	// Update is called once per frame
	void Update() {
        // PlayerがGoalするまでは道路を撤去し続ける。
        if (this.transform.position.z <= goalPos - delPosZ)
        {
            // playerの後方(画面外)に道路オブジェクトが移動したら消す。
            if (this.transform.position.z <= player.transform.position.z - delPosZ)
            {
                Destroy(this.gameObject);
                Debug.Log("道路撤去");
            }
        }
    }
}
