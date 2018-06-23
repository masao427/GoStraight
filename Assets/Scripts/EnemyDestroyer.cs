using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : MonoBehaviour {
    // Playerオブジェクト取得
    private GameObject player;

    // Playerの通過ポイント
    private int delPosZ = 20;

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        this.player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update() {
        // playerの後方(画面外)にオブジェクトが移動したら消す。
        if (this.transform.position.z <= player.transform.position.z - delPosZ)
        {
            Destroy(this.gameObject);
            Debug.Log("Enemy Deleted");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 道路の両サイドに接触した場合
        if ((collision.gameObject.tag == "LeftSideFenceTag") || (collision.gameObject.tag == "RightSideFenceTag"))
        {
            // ミス
            Destroy(this.gameObject);
            Debug.Log("Enemy Crashed");
        }
    }
}
