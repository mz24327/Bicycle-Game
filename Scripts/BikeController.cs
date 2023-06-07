using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BikeController : MonoBehaviour {
    public float resistance;
    public float accelRate;
    public float jumpRate;
    public float rotationSpeed;
    public Text scoreText;
    public Text finalScoreText;
    public AudioClip hit_sound;
    public AudioClip jump_sound;
    public static bool isGameOver;
    public static bool reset;
    public static bool active;
    public static bool click;
    public static Vector3 firstPosition;
    
    private Rigidbody rb;
    private AudioSource audioSource;
    private long score;
    private long finalScore;
    private bool isGround;
    private bool isJumping;
    private bool notHit;
    // Joy-Con
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues( typeof( Joycon.Button ) ) as Joycon.Button[];
    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;

    void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        score = 0;
        reset = false;
        active = false;
        click = false;
        notHit = true;
        SetScoreText();
        firstPosition = transform.position;
        isGameOver = false;
        isGround = false;
        isJumping = false;

        // Joy-Conの情報を取得
        m_joycons = JoyconManager.Instance.j;
        if( m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );
    }

    void FixedUpdate() {
        if (m_joycons == null || m_joycons.Count <= 0) return;
        float accelL = (m_joyconL.GetAccel() - new Vector3 (0.7f, -0.3f, 0.5f)).magnitude;
        float accelR = (m_joyconR.GetAccel() - new Vector3 (0.0f, 0.5f, 0.9f)).magnitude;
        if(active){
            // ジャンプ状態解除
            if(isGround) {
                isJumping = false;
            }
            // 空気抵抗による減速
            rb.AddForce (new Vector3 (0.0f, 0.0f, -resistance * rb.velocity.z * rb.velocity.z));
            // アクセル
            if ((Input.GetKey(KeyCode.R) || accelR > 0.8) && rb.velocity.z < 20 && isGround) {
                Accel(accelL);
            }
            // ジャンプ
            if ((Input.GetKey(KeyCode.Space) || accelL > 0.7) && isGround) {
                Jump();
            }
            if(isJumping) {
                float step = rotationSpeed * Time.deltaTime;
                if(rb.velocity.y > 0) {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(-30, 0, 0), step);
                }
                if(rb.velocity.y < 0) {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), step);
                }
            }
            // スコアを記録
            score = (long)(transform.position.z - firstPosition.z);
            SetScoreText();
        }

        if(reset) {
            Reset();
            reset = false;
        }

        if(m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
            click = true;
        }
    }

    // 着地orゲームオーバー判定
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag ("Stage")) {
            isGround = true;
        }else if (collision.gameObject.CompareTag ("Wall")) {
            if(notHit){
                audioSource.PlayOneShot(hit_sound);
                notHit = false;
            }    
            active = false;
            rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
            finalScore = score;
            finalScoreText.text = "Final Score: " + finalScore + "m";
            isGameOver = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag ("Stage")) {
            isGround = false;
        }
    }

    // アクセル
    void Accel(float accel) {
        if(accel < 2){
            accel = 2;
        }
        rb.AddForce (new Vector3 (0.0f, 0.0f, accel * accelRate));
    }

    // ジャンプ
    void Jump() {
        isJumping = true;
        audioSource.PlayOneShot(jump_sound);
        rb.AddForce (new Vector3 (0.0f, jumpRate, 0.0f), ForceMode.Impulse);
    }

    // スコア表示
    void SetScoreText() {
        scoreText.text = "Score: " + score + "m";
    }

    void Reset() {
        score = 0;
        notHit = true;
        transform.position = firstPosition;
    }
}