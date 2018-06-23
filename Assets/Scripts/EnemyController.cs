using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    // 敵車(Enemy)を移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;

    // 敵車(Enemy)のZ軸方向の速度
    private float spdPosZ = 75.0f;

    // Use this for initialization
    void Start() {
        // Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update() {
        // 敵車(Enemy)を移動させる。
        this.myRigidbody.velocity = new Vector3(0, 0, spdPosZ);
    }
}
