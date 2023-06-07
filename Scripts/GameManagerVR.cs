using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class GameManagerVR : MonoBehaviour {
    // public GameObject titleCamera;
    // public GameObject mainCamera;
    // public GameObject gameOverCamera;
    public Canvas titleCanvas;
    public Canvas mainCanvas;
    public Canvas gameOverCanvas;

    // void Start () {
    // }

    void LateUpdate () {
        if(BikeControllerVR.isGameOver) {
            Invoke("GameOver", 0.5f);
        // } else if(BikeControllerVR.click) {
        //     if(titleCamera.activeSelf) {
        //         GameStart();
        //     } else if(gameOverCamera.activeSelf) {
        //         Return();
        //         BikeControllerVR.click = false;
        //     } 
        }
    }
    
    public void GameStart() {
        // タイトル画面を消す
        // titleCamera.SetActive(false);
        titleCanvas.gameObject.SetActive(false);
        // プレイ画面に移行
        // mainCamera.SetActive(true);
        mainCanvas.gameObject.SetActive(true);
        // SceneManager.LoadScene("MainforVR");
        BikeControllerVR.active = true;
        BikeControllerVR.click = false;
    }

    void GameOver() {
        // プレイ画面を消す
        // mainCamera.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        // ゲームオーバー画面に移行
        // gameOverCamera.SetActive(true);
        gameOverCanvas.gameObject.SetActive(true);
        // SceneManager.LoadScene("GameOver");
        BikeControllerVR.isGameOver = false;
        BikeControllerVR.click = false;
    }

    public void Return() {
        // ゲームオーバー画面を消す
        // gameOverCamera.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        // タイトル画面に移行
        // titleCamera.SetActive(true);
        titleCanvas.gameObject.SetActive(true);
        // SceneManager.LoadScene("Title");
        // オブジェクトの初期化
        StageGenerator.reset = true;
        BikeControllerVR.reset = true;
        BikeControllerVR.click = false;
    }
}