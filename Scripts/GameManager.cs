using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject titleCamera;
    public GameObject mainCamera;
    public GameObject gameOverCamera;
    public Canvas titleCanvas;
    public Canvas mainCanvas;
    public Canvas gameOverCanvas;

    // void Start () {
    // }

    void LateUpdate () {
        if(BikeController.isGameOver) {
            Invoke("GameOver", 0.5f);
        } else if(BikeController.click) {
            if(titleCamera.activeSelf) {
                GameStart();
            } else if(gameOverCamera.activeSelf) {
                Return();
                BikeController.click = false;
            } 
        }
    }
    
    public void GameStart() {
        // タイトル画面を消す
        titleCamera.SetActive(false);
        titleCanvas.gameObject.SetActive(false);
        // プレイ画面に移行
        mainCamera.SetActive(true);
        mainCanvas.gameObject.SetActive(true);
        BikeController.active = true;
        BikeController.click = false;
    }

    void GameOver() {
        // プレイ画面を消す
        mainCamera.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        // ゲームオーバー画面に移行
        gameOverCamera.SetActive(true);
        gameOverCanvas.gameObject.SetActive(true);
        BikeController.isGameOver = false;
        BikeController.click = false;
    }

    public void Return() {
        // ゲームオーバー画面を消す
        gameOverCamera.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        // タイトル画面に移行
        titleCamera.SetActive(true);
        titleCanvas.gameObject.SetActive(true);
        // オブジェクトの初期化
        StageGenerator.reset = true;
        BikeController.reset = true;
        BikeController.click = false;
    }
}