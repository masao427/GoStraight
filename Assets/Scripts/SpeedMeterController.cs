using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeterController : MonoBehaviour {
    // 各種オブジェクト
    private GameObject player;      // Playerのオブジェクト
    private GameObject speed;       // デジタルスピードメータのオブジェクト
    private Image spdMeter;         // アナログスピードメータのオブジェクト

    // 各種属性
    private float maxSpeed = 300.0f;// 最高速設定
    private float spdnum = 0.0f;    // スピードの表示

    // Use this for initialization
    void Start() {
        // Playerのオブジェクトを取得
        player = GameObject.Find("Player");
        Debug.Log("Player Position: " + (player.transform.position.z + 240));

        // デジタルスピードメータのオブジェクト
        speed = GameObject.Find("SpeedValue");

        // アナログスピードメータのオブジェクト
        spdMeter = GameObject.Find("SpeedMeter").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update() {
        // 現在の速度を取得
        spdnum = player.GetComponent<Rigidbody>().velocity.magnitude * 3;

        // デジタルスピードメータに現在の速度を表示
        speed.GetComponent<Text>().text = spdnum.ToString("f0");

        // アナログスピードメータに現在の速度を表示
        spdMeter.fillAmount = (spdnum / maxSpeed) * 0.8f;
    }
}
