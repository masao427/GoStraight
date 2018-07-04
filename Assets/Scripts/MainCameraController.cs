using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    // Playerのオブジェクト
    private GameObject player;

    // Goalのオブジェクト
    private GameObject goal;

    // Goal地点
    private float goalPos;

    // Playerとカメラの距離
    private float difference;

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
