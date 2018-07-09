using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : MonoBehaviour {
    // Playerオブジェクト取得
    private GameObject player;

    // 爆発のPrefab
    public GameObject explosion;

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
        }
    }

    // 敵車(Enemy)が何かに接触した場合
    private void OnCollisionEnter(Collision collision) {
        // 道路の両サイドに接触した場合
        if ((collision.gameObject.tag == "LeftSideFenceTag") || (collision.gameObject.tag == "RightSideFenceTag"))
        {
            // 爆発エフェクト
            Explosion();

            // オブジェクトの消去
            Destroy(gameObject);
        }
    }

    // 爆発エフェクト
    private void Explosion() {
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
