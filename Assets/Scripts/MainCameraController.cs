using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;  // Playerのオブジェクト
    private GameObject goal;    // Goalのオブジェクト

    // 各種属性
    private float goalPos;      // Goal地点
    private float difference;   // Playerとカメラの距離

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");

        // Goalオブジェクトの取得
        goal = GameObject.Find("GoalPrefab");
        goalPos = goal.transform.position.z;

        //Playerとカメラの位置(z座標)の差を求める
        difference = player.transform.position.z - transform.position.z;
    }
	
	// Update is called once per frame
	void Update() {
        // Playerがゴールしたらカメラもストップ
        if (player.transform.position.z < goalPos)
        {
            // Playerの位置に合わせてカメラの位置を移動
            transform.position = new Vector3(0, transform.position.y, player.transform.position.z - difference);
        }
    }
}
