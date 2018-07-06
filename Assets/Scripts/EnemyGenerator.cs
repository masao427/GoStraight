using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;      // Playerのオブジェクト
    private GameObject goal;        // Goalのオブジェクト

    public GameObject enemy1;       // 敵車(enemy)のPrefabを入れる
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    // 各種属性
    private float goalPos;          // Goal地点
    private float posXRange = 1.84f;// 敵車(enemy)が生成されるx方向の範囲
    private float enemyPos;         // 敵車(enemy)が生成されるz方向の場所

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");

        // Goalオブジェクトの取得
        goal = GameObject.Find("GoalPrefab");
        goalPos = goal.transform.position.z;

        // 敵車(enemy)が生成されるのはPlayerの前方30m
        enemyPos = player.transform.position.z + 30;
    }
	
	// Update is called once per frame
	void Update() {
        // 一定の距離ごとに敵車(enemy)を生成
        if (enemyPos < goalPos)
        {
            if (enemyPos <= player.transform.position.z + 30)
            {
                // 敵車(enemy)を生成
                EnemyCreation(enemyPos);

                // 次のItemの生成場所を決定(Playerの前方30mに50m毎生成)
                enemyPos += 50;
            }
        }
    }

    // Enemyをランダムに生成する関数
    void EnemyCreation(float PosZ) {
        // 敵車オブジェクト
        GameObject car;

        //どの種類の敵を生成するのかをランダムに設定
        int num = Random.Range(0, 10);
        switch (num) 
        {
            case 0:
            case 1:
            case 2:
                // 敵車(enemy)を生成
                car = Instantiate(enemy1) as GameObject;
                break;

            case 3:
            case 4:
            case 5:
                // 敵車(enemy)を生成
                car = Instantiate(enemy2) as GameObject;
                break;

            case 6:
            case 7:
            case 8:
                // 敵車(enemy)を生成
                car = Instantiate(enemy3) as GameObject;
                break;

            case 9:
            case 10:
                // 敵車(enemy)を生成
                car = Instantiate(enemy4) as GameObject;
                break;

            default:
                car = Instantiate(enemy4) as GameObject;
                break;
        }

        // アイテムを置くX座標のオフセットをランダムに設定
 //       int offsetX = Random.Range(-2, 2);
        int offsetX = -2;
        car.transform.position = new Vector3(posXRange * offsetX, car.transform.position.y, PosZ);
    }
}
