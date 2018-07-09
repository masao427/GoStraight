using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeGenerator : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;      // Playerのオブジェクト
    private GameObject goal;        // Goalのオブジェクト
    public GameObject coneObj;      // coneのPrefab

    // 各種属性
    private float goalPos;          // Goal地点
    private float posXRange = 2.0f; // 障害物(cone)が生成されるx方向の範囲
    private float conePos;          // 障害物(cone)が生成されるz方向の場所

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");

        // Goalオブジェクトの取得
        goal = GameObject.Find("GoalPrefab");
        goalPos = goal.transform.position.z;

        // 障害物(cone)が生成されるのはPlayerの前方100mから
        conePos = player.transform.position.z + 100;
	}
	
	// Update is called once per frame
	void Update() {
        // 一定の距離ごとに障害物(cone)を生成
        if (conePos < goalPos)
        {
            // Playerの50m先よりも敵車(enemy)生成ポイントが近づいたとき
            if (conePos <= player.transform.position.z + 50)
            {
                // 障害物(cone)を生成
                coneCreation(conePos);

                // 次の障害物(cone)の生成ポイントを決定
                // Playerの前方10m毎に障害物(cone)の生成ポイントを生成
                conePos += 10;
            }
        }
    }

    // 障害物(cone)をランダムに生成する関数
    void coneCreation(float PosZ)
    {
        // 障害物(cone)オブジェクト
        GameObject cone;

        // アイテムを置くX座標のオフセットをランダムに設定
        int offsetX = Random.Range(-2, 2);

        // 障害物(cone)を生成
        cone = Instantiate(coneObj) as GameObject;
        cone.transform.position = new Vector3(posXRange * offsetX, cone.transform.position.y, PosZ);
    }
}
