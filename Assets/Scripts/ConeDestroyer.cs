using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDestroyer : MonoBehaviour {
    // Playerオブジェクト取得
    private GameObject player;

    // Playerの通過ポイント
    private int delPosZ = 20;

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
