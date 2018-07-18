using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeterController : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;          // Playerのオブジェクト
    private Rigidbody pRigidbody;            
    private GameObject goal;            // Goalのオブジェクト
    private GameObject speed;           // デジタルスピードメータのオブジェクト
    private Image spdMeter;             // アナログスピードメータのオブジェクト
    private Text spdtxt;                // スピード表示

    // 各種属性
    private float goalPos;              // Goal地点
    private float maxSpeed = 300.0f;    // 最高速設定
    private float spdnum = 0.0f;        // スピードの値

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");
        pRigidbody = player.GetComponent<Rigidbody>();

        // Goalオブジェクトの取得
        goal = GameObject.Find("GoalPrefab");
        goalPos = goal.transform.position.z;
        
        // デジタルスピードメータのオブジェクト
        speed = GameObject.Find("SpeedValue");
        spdtxt = speed.GetComponent<Text>();

        // アナログスピードメータのオブジェクト
        spdMeter = GameObject.Find("SpeedMeter").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update()
    {
        // ゴールする前
        if (player.transform.position.z < goalPos)
        {
            // アナログスピードメータに現在の速度を表示
            spdnum = pRigidbody.velocity.magnitude * 3;
            spdMeter.fillAmount = (spdnum / maxSpeed) * 0.8f;

            // デジタルスピードメータに現在の速度を表示
            spdtxt.text = spdnum.ToString("f0");
        }
        // ゴールした後
        else
        {
            // ゴールした時の速度でメータをストップ
            spdMeter.fillAmount = (spdnum / maxSpeed) * 0.8f;
            spdtxt.text = spdnum.ToString("f0");
        }
    }
}
