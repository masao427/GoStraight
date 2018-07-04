using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonGenerator : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;  // Playerのオブジェクト
    private GameObject goal;    // Goalのオブジェクト
    public GameObject pylonObj;  // pylonのPrefab

    // 各種属性
    private float goalPos;              // Goal地点
    private float posXRange = 1.84f;    // 障害物(pylon)が生成されるx方向の範囲
    private float pylonPos;              // 障害物(pylon)が生成されるz方向の場所

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");

        // Goalオブジェクトの取得
        goal = GameObject.Find("GoalPrefab");
        goalPos = goal.transform.position.z;

        // 障害物(pylon)が生成されるのはPlayerの前方30m
        pylonPos = player.transform.position.z + 30;
	}
	
	// Update is called once per frame
	void Update() {
        // 一定の距離ごとに障害物(pylon)を生成
        if (pylonPos < goalPos)
        {
            if (pylonPos <= player.transform.position.z + 30)
            {
                // 障害物(pylon)を生成
                pylonCreation(pylonPos);

                // 次のItemの生成場所を決定(Playerの前方30mに50m毎生成)
                pylonPos += 50;
            }
        }
    }

    // 障害物(pylon)をランダムに生成する関数
    void pylonCreation(float PosZ)
    {
        // 障害物(pylon)オブジェクト
        GameObject pylon;

        // アイテムを置くX座標のオフセットをランダムに設定
        int offsetX = Random.Range(-2, 2);

        // 障害物(pylon)を生成
        pylon = Instantiate(pylonObj) as GameObject;
        pylon.transform.position = new Vector3(posXRange * offsetX, pylon.transform.position.y, PosZ);
    }
}
