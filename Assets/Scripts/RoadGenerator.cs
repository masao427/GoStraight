using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {
    // Playerのオブジェクト
    private GameObject player;

    // 新しい RoadPrefab を入れる
    public GameObject RoadPrefab;

    // 道路オブジェクトの中心のZ軸ポジション
    private float centerPosZ = 0;
    private float nextPosZ = 504;

    // 道路オブジェクトの生成回数
    private int i = 0;
    private int roadCreateNum = 50;

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update() {
        // 道路の設営(Playerが道路の半分を過ぎたら次の道路を設営)
        if ((player.transform.position.z >= centerPosZ)
         && (i <= roadCreateNum))
        {
            // 次の道路のセンターのZ軸ポジションを設定
            centerPosZ += nextPosZ;

            // 新しい道路オブジェクトの生成
            GameObject road = Instantiate(RoadPrefab) as GameObject;
            road.transform.position = new Vector3(0, 0, centerPosZ);

            // 生成された回数をカウント
            i++;

            // デバッグ情報
            Debug.Log("道路設営 " + i + " 回");
            Debug.Log("Center = " + centerPosZ);
            Debug.Log("Player = " + player.transform.position.z);
        }
    }
}
