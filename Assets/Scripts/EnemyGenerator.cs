using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
    // Playerのオブジェクト
    private GameObject player;

    // 敵車(enemy)のPrefabを入れる
    public GameObject enemyPrefab;

    // ゴール地点
    private int goalPos = 2000;

    // 敵車(enemy)が生成されるx方向の範囲
    private float posRange = 3.4f;

    // 敵車(enemy)が生成されるz方向の場所
    private float enemyPos;

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        this.player = GameObject.Find("Player");

        // 敵車(enemy)が生成されるのはPlayerの前方100m
        enemyPos = player.transform.position.z + 100;
    }
	
	// Update is called once per frame
	void Update() {
        // 一定の距離ごとに敵車(enemy)を生成
        if (enemyPos < goalPos)
        {
            if (enemyPos <= player.transform.position.z + 200)
            {
                // 敵車(enemy)を生成
                EnemyCreation(enemyPos);

                // 次のItemの生成場所を決定(Playerの前方100mに150m毎生成)
                enemyPos += 50;
            }
        }
    }

    // Enemyをランダムに生成する関数
    void EnemyCreation(float PosZ)
    {
        // アイテムを置くX座標のオフセットをランダムに設定
        int offsetX = Random.Range(-1, 1);

        // 敵車(enemy)を生成
        GameObject car = Instantiate(enemyPrefab) as GameObject;
        car.transform.position = new Vector3(posRange * offsetX, car.transform.position.y, PosZ);

        // 敵車(enemy)の速度を決定する。
        float speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed + 50);
    }
}
