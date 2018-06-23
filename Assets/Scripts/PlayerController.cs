using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    // Playerを移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;

    //前進するための力(加速力)
    private float forwardForce = 0.5f;  // アクセルオン
    private float reduceForce = 1.0f;   // エンジンブレーキ
    private float movePosZ = 0f;

    //左右に移動するための力
    private float turnForce = 10.0f;    // 左右に動く時のスピード
    private float movePosX = 0f;

    //動きを減速させる係数(強制停止)
    private float coefficient = 4.0f;

    //ゲーム終了の判定
    private bool isEnd = false;

    // Playerの状態
    private bool isDriftLeftway = false;    // ドリフト状態(左に流れている)
    private bool isDriftRightway = false;   // ドリフト状態(右に流れている)
    private bool isSpin = false;            // スピン状態
    private float driftForce = 10.0f;       // ドリフト時にかかる力

    // 距離の表示
    private GameObject distance;
    private float distnum;

    // スピードの表示
    private GameObject speed;
    private float spdnum = 0.0f;
    private float maxSpeed = 100f;

    // Use this for initialization
    void Start() {
        // Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();

        // 距離表示のオブジェクト
        this.distance = GameObject.Find("Distance");

        // スピード表示のオブジェクト
        this.speed = GameObject.Find("Speed");
    }

    // Update is called once per frame
    void Update()
    {
        // 距離を計測
        distnum = this.transform.position.z + 240;
        this.distance.GetComponent<Text>().text = "Dist:" + distnum.ToString("f0") + "m";

        // 速度を表示
        this.spdnum = this.myRigidbody.velocity.magnitude;
        this.speed.GetComponent<Text>().text = "Speed:" + spdnum.ToString("f0") + "km/h";

        // ゲーム終了ならPlayerの動きを減衰する
        if (this.isEnd == true)
        {
            if (movePosZ > 0)
            {
                // 強制減速
                movePosZ -= this.coefficient;
                movePosX = 0;
            }
            else
            {
                movePosZ = 0;
            }
        }

        // ドリフト状態の時は強制的に左右どちらかにドリフトしている。
        if (this.isDriftLeftway == true)
        {
            // Playerを左方向にドリフトさせる。
//            this.myRigidbody.AddForce(-(this.turnForce * driftForce), 0, 0);
            this.myRigidbody.velocity = new Vector3(-driftForce, 0, movePosZ);

            // 流れている方向にキーを入力すると復帰する。
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.isDriftLeftway = false;
            }
        }
        else if (this.isDriftRightway == true)
        {
            // Playerを右方向にドリフトさせる。
//            this.myRigidbody.AddForce((this.turnForce * driftForce), 0, 0);
            this.myRigidbody.velocity = new Vector3(driftForce, 0, movePosZ);

            // 流れている方向にキーを入力すると復帰する。
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.isDriftRightway = false;
            }
        }

        // 通常走行状態
        if ((this.isEnd == false) && (this.isDriftLeftway == false) && (this.isDriftRightway == false))
        {
            // Playerに前方向の力を加える
            // アクセルオン時
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // Max Speed になるまで加速する。
                if (spdnum <= maxSpeed)
                {
                    movePosZ += forwardForce;
                }
            }
            // アクセルオフ時
            else
            {
                // エンジンブレーキがかかる
                if (movePosZ > 0)
                {
                    movePosZ -= reduceForce;
                }
                else
                {
                    movePosZ = 0;
                }
            }

            // Playerを矢印キーまたはボタンに応じて左右に移動させる
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //左に移動
                movePosX = -this.turnForce;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                //右に移動
                movePosX = this.turnForce;
            }
            else
            {
                // 左右の入力がないときはその場から動かない
                movePosX = 0;
            }

            // Playerを移動させる。
            this.myRigidbody.velocity = new Vector3(movePosX, 0, movePosZ);
        }
    }

    // Playerに何かが衝突した際の処理
    private void OnTriggerEnter(Collider other)
    {
        // ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            Debug.Log("Goal");
        }

        if (other.gameObject.tag == "CornTag")
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 道路の両サイドに接触した場合
        if ((collision.gameObject.tag == "LeftSideFenceTag") || (collision.gameObject.tag == "RightSideFenceTag"))
        {
            // ミス
            movePosZ = 0; // その場で停車
            movePosX = 0; // その場で停車
            this.isEnd = true;
            Debug.Log("Hit the fence!");
        }
        
        // 敵車と接触した場合
        if (collision.gameObject.tag == "EnemyTag")
        {
            // 衝突個所がPlayerから見て右なのか左なのか？
            foreach (ContactPoint contact in collision.contacts)
            {
                // ドリフトしているときは左右のどちらかに必ず流れているので、
                // 反対方向の判定は必ずfalseにする。
                // (カウンター当てる前に次の敵車に接触した場合を考慮)
                if (contact.point.x > 0)
                {
                    // 右側に接触
                    isDriftLeftway = true;  // Playerの右側に接触したの車体は左方向にドリフトする。
                    isDriftRightway = false;
                    Debug.Log("Hit to my right side!");
                }
                else
                {
                    // 左側に接触
                    isDriftLeftway = false;
                    isDriftRightway = true; // Playerの左側に接触したの車体は右方向にドリフトする。
                    Debug.Log("Hit to my left side!");
                }
                Debug.Log(contact.point);
            }
        }
    }
}
