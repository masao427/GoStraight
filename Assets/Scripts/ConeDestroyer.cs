using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDestroyer : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;  // Playerオブジェクト取得

    // 各種属性
    private int delPosZ = 20;   // Playerの通過ポイント
    private int scoreNum = 0;   // スコア計算

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update() {
        // playerの後方(画面外)にオブジェクトが移動したら消す。
        if (transform.position.z <= player.transform.position.z - delPosZ)
        {
            Destroy(gameObject);
            Debug.Log("[Cone]Deleted");
        }
    }
}
