using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    // 各種オブジェクト
    private Rigidbody myRigidbody;          // 敵車(Enemy)を移動させるコンポーネントを入れる

    // 各種属性
    public float enemyMaxSpeed;             // 敵車(Enemy)のZ軸方向の速度
    private float spdPosX = 0;              // 敵車(Enemy)のX軸方向の速度
    private float driftForce = 10.0f;       // ドリフト時のX軸の速度
    private bool isDriftLeftway = false;    // 敵車(Enemy)の状態
    private bool isDriftRightway = false;

    // Use this for initialization
    void Start() {
        // Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        // ドリフト状態の時は強制的に左右どちらかにドリフトしている。
        if (this.isDriftLeftway == true)
        {
            // Playerを左方向にドリフトさせる。
            spdPosX = -driftForce;
        }
        else if (this.isDriftRightway == true)
        {
            // Playerを右方向にドリフトさせる。
            spdPosX = driftForce;
        }

        // 敵車(Enemy)を移動させる。
        this.myRigidbody.velocity = new Vector3(spdPosX, 0, enemyMaxSpeed);
    }
    
    private void OnCollisionEnter(Collision collision) {
        // Playerと接触した場合
        if ((collision.gameObject.tag == "PlayerTag")
         || (collision.gameObject.tag == "EnemyTag"))
        {
            // 衝突個所が敵車(Enemy)から見て右なのか左なのか？
            foreach (ContactPoint contact in collision.contacts)
            {
                // ドリフトしているときは左右のどちらかに必ず流れているので、
                // 反対方向の判定は必ずfalseにする。
                if (contact.point.x > 0)
                {
                    // 敵車(Enemy)の右側に接触したので車体は左方向にドリフトする。
                    isDriftLeftway = false;  
                    isDriftRightway = true;
                    Debug.Log("[Enemy]Hit to My Right Side!");
                }
                else
                {
                    // 敵車(Enemy)の左側に接触したの車体は右方向にドリフトする。
                    isDriftLeftway = true;
                    isDriftRightway = false; 
                    Debug.Log("[Enemy]Hit to My Left Side!");
                }

                // ドリフト開始
                Debug.Log(contact.point);
                Debug.Log("[Enemy]Start Drifting!!!");
            }
        }
    }
}
