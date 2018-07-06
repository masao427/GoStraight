using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    // 各種オブジェクト
    private Rigidbody myRigidbody;          // Playerを移動させるコンポーネントを入れる
    private GameObject messageDisplay;      // 各種メッセージ表示
    private GameObject scoreDisplay;        // スコア表示
    private GameObject stage0Button;        // Titleへ遷移するボタン
    private GameObject stage1Button;        // Stage1へ遷移するボタン
    private GameObject stage2Button;        // Stage2へ遷移するボタン
    private GameObject stage3Button;        // Stage3へ遷移するボタン
    public GameObject explosion;            // 爆発のPrefab

    // 各種属性
    private float defaultPosY;              // PlayerのY軸(ゲームを通じて変更なし)

    // 前進するための力(加速力)
    private float forwardForce = 0.5f;      // アクセルオン
    private float reduceForce = 1.0f;       // エンジンブレーキ
    private float movePosZ = 0f;

    // 左右に移動するための力
    private float turnForce = 10.0f;        // 左右に動く時のスピード
    private float movePosX = 0f;

    // 動きを減速させる係数(強制停止)
    private float coefficient = 500.0f;

    // Playerの状態
    private bool isEnd = false;             // ゲーム終了の判定
    private bool isDriftLeftway = false;    // ドリフト状態(左に流れている)
    private bool isDriftRightway = false;   // ドリフト状態(右に流れている)
    private bool isSpin = false;            // スピン状態
    private bool isStop = false;            // 停止状態
    private bool isGameOver = false;        // ゲームオーバ状態
    private float driftForce = 6.0f;        // ドリフト時にX軸にかかる力(どのくらいの速度で滑るか)
    private int dftTimeOut = 25;            // ドリフトしている時間
    private int dftCount = 0;               // ドリフト中の時間カウント
    private float spnNum = 15.0f;           // スピン状態の時の回転数
    private float expPosZ;                  // 爆発したときの位置
    private int expTimeOut = 100;           // 爆発している時間
    private int expCount = 0;               // 爆発中の時間カウント

    // ボタン操作
    private bool isLButtonDown = false;     // 左ボタン押下の判定
    private bool isRButtonDown = false;     // 右ボタン押下の判定
    private bool isAButtonDown = false;     // アクセルボタン押下の判定

    // スコア
    private int scoreNum = 0;               // スコア計算
    private int missNum = 3;                // ミスの回数

    // スピードの表示
    public float playerMaxSpeed;            // 最高速設定(ステージ毎に選択できる)
    private float spdnum = 0.0f;

    // ステージセレクトボタンの位置
    private float st0PosX;
    private float st1PosX;
    private float st2PosX;
    private float st3PosX;

    // Use this for initialization
    void Start() {
        // Rigidbodyコンポーネントを取得
        myRigidbody = GetComponent<Rigidbody>();

        // スコア、メッセージ表示用のオブジェクトを取得
        messageDisplay = GameObject.Find("Message");
        scoreDisplay = GameObject.Find("Score");

        // ステージセレクトボタン
        Vector3 workPos;
        stage1Button = GameObject.Find("GoStage1");
        workPos = stage1Button.transform.position;
        st1PosX = workPos.x;
        workPos.x = st1PosX + 1000;
        stage1Button.transform.position = workPos;

        stage2Button = GameObject.Find("GoStage2");
        workPos = stage2Button.transform.position;
        st2PosX = workPos.x;
        workPos.x = st2PosX + 1000;
        stage2Button.transform.position = workPos;

        stage3Button = GameObject.Find("GoStage3");
        workPos = stage3Button.transform.position;
        st3PosX = workPos.x;
        workPos.x = st3PosX + 1000;
        stage3Button.transform.position = workPos;

        stage0Button = GameObject.Find("GoTitle");
        workPos = stage0Button.transform.position;
        st0PosX = workPos.x;
        workPos.x = st0PosX + 1000;
        stage0Button.transform.position = workPos;

        // PlayerのY軸
        defaultPosY = transform.position.y;

        //現在読み込まれているシーン数だけループ
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            //読み込まれているシーンを取得し、その名前をログに表示
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name;
            Debug.Log(sceneName);
        }
    }

    // Update is called once per frame
    void Update() {
        // ゲーム終了ならPlayerの動きを減衰する
        if (isEnd == true)
        {
            if (movePosZ > 0)
            {
                // 強制減速
                movePosZ -= coefficient;

                // Playerの左右の動きを固定
                movePosX = 0;
            }
            else
            {
                // 速度0km/h
                movePosZ = 0;
            }

            // ゲームオーバ状態
            isGameOver = true;
        }

        // ミスした後からの再スタート
        if (isStop == true)
        {
            // 爆発エフェクト終了まで待つ
            if (expCount < expTimeOut)
            {
                expCount++;
            }
            else
            {
                // Playerを表示(画面外にいるオブジェクトを再設定)
                transform.position = new Vector3(0, defaultPosY, expPosZ);

                // 状態復帰
                isStop = false;
                Debug.Log("[Player]Let's ReStart!");

                // ミスの回数をカウント
                missNum -= 1;
                Debug.Log("[Player]Miss " + missNum);
            }

            // 残機が無くなったらゲームオーバ状態
            if (missNum == 0)
            {
                // 画面にゲームオーバと表示
                messageDisplay.GetComponent<Text>().text = "Game Over";

                // ゲームオーバ状態
                isGameOver = true;
            }
        }

        // ゲームオーバになった
        if (isGameOver == true)
        {
            // キー入力は無効
            if (((Input.GetKey(KeyCode.LeftArrow))  || (isLButtonDown))
             || ((Input.GetKey(KeyCode.RightArrow)) || (isRButtonDown))
             || ((Input.GetKey(KeyCode.UpArrow))    || (isAButtonDown)))
            {
                // 制御できない(何もしない)
                Debug.Log("[Player]Cannot Control!!!");
            }

            // シーン選択ボタンを表示する
            Vector3 tmp;
            tmp = stage1Button.transform.position;
            tmp.x = st1PosX;
            stage1Button.transform.position = tmp;

            tmp = stage2Button.transform.position;
            tmp.x = st2PosX;
            stage2Button.transform.position = tmp;

            tmp = stage3Button.transform.position;
            tmp.x = st3PosX;
            stage3Button.transform.position = tmp;

            tmp = stage0Button.transform.position;
            tmp.x = st0PosX;
            stage0Button.transform.position = tmp;
        }

        // ドリフト状態の時は強制的に左右どちらかにドリフトしている。
        if ((isDriftLeftway == true)
         && (isSpin == false))
        {
            // Playerを左方向にドリフトさせる。
            transform.rotation = Quaternion.Euler(0, -150, 0);
            myRigidbody.velocity = new Vector3(-driftForce, 0, movePosZ);

            // 流れている方向にキーを入力すると復帰する。
            if ((Input.GetKey(KeyCode.LeftArrow))
             || (isLButtonDown))
            {
                isDriftLeftway = false;
                movePosX = 0;
                transform.rotation = Quaternion.Euler(0, -180, 0);

                // スコア加算
                scoreNum += 100;
                scoreDisplay.GetComponent<Text>().text = "Score " + scoreNum.ToString("D6");
                Debug.Log("[Player]Recover!!!");
            }
            else if ((Input.GetKey(KeyCode.RightArrow))
                  || (isRButtonDown))
            {
                // 制御できない(何もしない)
                Debug.Log("[Player]Cannot Control!!!");
            }

            // ドリフトからスピン状態へ遷移
            if (dftCount < dftTimeOut)
            {
                // ドリフト中しばらくは滑る
                dftCount++;
                Debug.Log("[Player]Drifting to Left side!!!");
            }
            else
            {
                // ドリフト状態からスピン状態へ遷移
                isSpin = true;
                movePosX = driftForce;

                // Y軸で回転できるようにする。
                myRigidbody.constraints = RigidbodyConstraints.None;
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                Debug.Log("[Player]Spinning!!!");
            }
        }
        else if ((isDriftRightway == true)
              && (isSpin == false))
        {
            // Playerを右方向にドリフトさせる。
            transform.rotation = Quaternion.Euler(0, -210, 0);
            myRigidbody.velocity = new Vector3(driftForce, 0, movePosZ);

            // 流れている方向にキーを入力すると復帰する。
            if ((Input.GetKey(KeyCode.LeftArrow))
             || (isLButtonDown))
            {
                // 制御できない(何もしない)
                Debug.Log("[Player]Cannot Control!!!");
            }
            else if ((Input.GetKey(KeyCode.RightArrow))
                  || (isRButtonDown))
            {
                isDriftRightway = false;
                movePosX = 0;
                transform.rotation = Quaternion.Euler(0, -180, 0);

                // スコア加算
                scoreNum += 100;
                scoreDisplay.GetComponent<Text>().text = "Score " + scoreNum.ToString("D6");
                Debug.Log("[Player]Recover!!!");
            }

            // ドリフトからスピン状態へ遷移
            if (dftCount < dftTimeOut)
            {
                dftCount++;
                Debug.Log("[Player]Drifting to Right side!!!");
            }
            else
            {
                // ドリフト状態からスピン状態へ遷移
                isSpin = true;
                movePosX = driftForce;

                // Y軸で回転できるようにする。
                // 一旦全軸の制限を解除してからX, Z軸をFreeze
                myRigidbody.constraints = RigidbodyConstraints.None;
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                Debug.Log("[Player]Spinning!!!");
            }
        }

        // スピン状態
        if (isSpin == true)
        {
            // ドリフト方向とスピンの回転方向を決定
            if (isDriftLeftway == true)
            {
                myRigidbody.velocity = new Vector3(-movePosX, 0, movePosZ);
                transform.Rotate(0, -spnNum, 0);
            }
            else
            {
                myRigidbody.velocity = new Vector3(movePosX, 0, movePosZ);
                transform.Rotate(0, spnNum, 0);
            }

            if (((Input.GetKey(KeyCode.LeftArrow))  || (isLButtonDown))
             || ((Input.GetKey(KeyCode.RightArrow)) || (isRButtonDown)))
            {
                // 制御できない(何もしない)
                Debug.Log("[Player]Cannot Control!!!");
            }

            // スピン中アクセルオンの場合、Z軸とX軸の減少や緩やか
            // つまり、スピンしているために滑っているが、やがて停止する。
            if ((Input.GetKey(KeyCode.UpArrow)) 
             || (isAButtonDown))
            {
                if (movePosX > 0)
                {
                    movePosX -= 0.075f;
                }

                // スピンしているので、アクセルオンしていても緩やかに減速する。
                movePosZ -= (reduceForce * 0.80f);
                Debug.Log("[Player]Accsel ON in Spin status");
            }
            // スピン中アクセルオフの場合、Z軸とX軸の減少が早い。
            // つまり、スピンして滑ってはいるがエンジンブレーキがかかってすぐに減速する。
            else
            {
                if (movePosX > 0)
                {
                    movePosX -= 0.15f;
                }

                // エンジンブレーキがかかる
                movePosZ -= reduceForce;
                Debug.Log("[Player]Accsel OFF in Spin status");
            }

            // 車速が完全に停止した
            if (movePosZ <= 0)
            {
                // X軸の動きも停止する。
                movePosX = 0;

                // スピンが停止すれば通常停止状態
                isDriftLeftway = false;
                isDriftRightway = false;
                isSpin = false;

                // スピンを止める。
                myRigidbody.angularVelocity = Vector3.zero;
                Debug.Log("[Player]Stopped Spin status");

                // Playerの姿勢を元に戻す。
                transform.rotation = Quaternion.Euler(0, -180, 0);

                // 全軸のRotationをFreeze
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                Debug.Log("[Player]Recover from Spin status");
            }
        }

        // 通常走行状態(ドリフト無し、スピン無し)
        if ((isEnd == false)
         && (isGameOver == false)
         && (isDriftLeftway == false) 
         && (isDriftRightway == false) 
         && (isSpin == false)
         && (isStop == false))
        {
            // 現在の速度を取得
            spdnum = myRigidbody.velocity.magnitude * 3;

            // Playerに前方向の力を加える
            // アクセルオン時
            if ((Input.GetKey(KeyCode.UpArrow))
             || (isAButtonDown))
            {
                // Max Speed になるまで加速する。
                if (spdnum <= playerMaxSpeed)
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
            if ((Input.GetKey(KeyCode.LeftArrow))
             || (isLButtonDown))
            {
                //左に移動
                movePosX = -turnForce;
            }
            else if ((Input.GetKey(KeyCode.RightArrow))
                  || (isRButtonDown))
            {
                //右に移動
                movePosX = turnForce;
            }
            else
            {
                // 左右の入力がないときはその場から動かない
                movePosX = 0;
            }

            // Playerを移動させる。
            if (transform.position.y > defaultPosY)
            {
                // 混戦したときに車体が浮き上がるバグ対策
                Vector3 playerPos = transform.position;
                playerPos.y = defaultPosY;
                transform.position = playerPos;
            }
            myRigidbody.velocity = new Vector3(movePosX, 0, movePosZ);
        }
    }

    // Playerに何かが衝突した際の処理
    private void OnTriggerEnter(Collider other) {
        // ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            isEnd = true;
            messageDisplay.GetComponent<Text>().text = "Goal!";
            Debug.Log("[Player]Goal");
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // 道路の両サイドに接触した場合
        if ((collision.gameObject.tag == "LeftSideFenceTag")
         || (collision.gameObject.tag == "RightSideFenceTag"))
        {
            // 停止状態に遷移
            isStop = true;
            Debug.Log("[Player]Hit The Fence!");

            // 接触した瞬間の位置を記録
            expPosZ = transform.position.z;

            // その場で停止
            // ヒットする前はPlayerは動いていたので位置情報が残っている
            movePosX = 0;
            movePosZ = 0;
            myRigidbody.velocity = new Vector3(movePosX, 0, movePosZ);

            // 回転を止める
            if (isSpin == true)
            {
                // 回転を止める。
                myRigidbody.angularVelocity = new Vector3(0, 0, 0);
                
                // Playerの姿勢を元に戻す。
                transform.rotation = Quaternion.Euler(0, -180, 0);

                // 全軸のRotationをFreeze
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                // スピン状態の解除
                isSpin = false;
            }

            // 爆発エフェクト
            expCount = 0;
            Explosion();

            // Playerを一旦消す(画面外に移動させる)
            transform.position = new Vector3(0, 50, expPosZ);

            // 状態を戻す
            isDriftLeftway = false;
            isDriftRightway = false;
        }
        
        // 敵車と接触した場合
        if (collision.gameObject.tag == "EnemyTag")
        {
            // パーティクル再生
            GetComponent<ParticleSystem>().Play();

            // 衝突個所がPlayerから見て右なのか左なのか？
            foreach (ContactPoint contact in collision.contacts)
            {
                // ドリフトしているときは左右のどちらかに必ず流れているので、
                // 反対方向の判定は必ずfalseにする。
                // (カウンター当てる前に次の敵車に接触した場合を考慮)
                Debug.Log(contact.point);
                if (contact.point.x > 0)
                {
                    // Playerの右側に接触したので車体は左方向にドリフトする。
                    isDriftLeftway = true;  
                    isDriftRightway = false;
                    
                    Debug.Log("[Player]Hit to My Right Side!");
                    Debug.Log("[Player]Start Drifting Left Side!");
                }
                else
                {
                    // Playerの左側に接触したの車体は右方向にドリフトする。
                    isDriftLeftway = false;
                    isDriftRightway = true; 

                    Debug.Log("[Player]Hit to My Left Side!");
                    Debug.Log("[Player]Start Drifting Right Side!");
                }

                // ドリフト開始
                dftCount = 0;
            }
        }
        
        // 障害物に当たった時に加点
        if (collision.gameObject.tag == "ConeTag")
        {
            scoreNum += 10;
            scoreDisplay.GetComponent<Text>().text = "Score " + scoreNum.ToString("D6");
            Debug.Log("[Player]Hit a Cone");
        }
    }

    // 左ボタンを押した場合の処理
    public void GetMyLeftButtonDown()
    {
        isLButtonDown = true;
    }

    // 左ボタンを離した場合の処理
    public void GetMyLeftButtonUp()
    {
        isLButtonDown = false;
    }

    // 右ボタンを押した場合の処理
    public void GetMyRightButtonDown()
    {
        isRButtonDown = true;
    }

    // 右ボタンを離した場合の処理
    public void GetMyRightButtonUp()
    {
        isRButtonDown = false;
    }

    // アクセルボタンを押した場合の処理
    public void GetMyAccelButtonDown()
    {
        isAButtonDown = true;
    }

    // アクセルボタンを離した場合の処理
    public void GetMyAccelButtonUp()
    {
        isAButtonDown = false;
    }

    // ステージセレクト
    public void Stage1Start()
    {
        // 現在読み込まれているSceneを取得
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name;
        if (sceneName != "Stage1")
        {
            // 別のステージに移る場合は今のステージをUnloadしてから次のシーンをロードする。
            SceneManager.UnloadSceneAsync(sceneName);
        }
        SceneManager.LoadScene("Stage1");
    }

    public void Stage2Start()
    {
        // 現在読み込まれているSceneを取得
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name;
        if (sceneName != "Stage2")
        {
            // 別のステージに移る場合は今のステージをUnloadしてから次のシーンをロードする。
            SceneManager.UnloadSceneAsync(sceneName);
        }
        SceneManager.LoadScene("Stage2");
    }

    public void Stage3Start()
    {
        // 現在読み込まれているSceneを取得
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name;
        if (sceneName != "Stage3")
        {
            // 別のステージに移る場合は今のステージをUnloadしてから次のシーンをロードする。
            SceneManager.UnloadSceneAsync(sceneName);
        }
        SceneManager.LoadScene("Stage3");
    }

    public void GoTitleStart()
    {
        // 現在読み込まれているSceneを取得
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name;
        
        // 別のステージに移る場合は今のステージをUnloadしてから次のシーンをロードする。
        SceneManager.UnloadSceneAsync(sceneName);
        SceneManager.LoadScene("Title");
    }

    // 爆発エフェクト
    private void Explosion() {
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
