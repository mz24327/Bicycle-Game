using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour {
    int StageSize = 100;
    public int StageIndex;
    public int count;
    public GameObject firstStage;//ステージのプレハブ
    public GameObject[] stagenum;//ステージのプレハブ
    public Transform Target;
    public int FirstStageIndex;//スタート時にどのインデックスからステージを生成するのか
    public int aheadStage; //事前に生成しておくステージ
    public List<GameObject> StageList = new List<GameObject>();//生成したステージのリスト

    public static bool reset;

    // Start is called before the first frame update
    void Start() {
        StageIndex = FirstStageIndex;
        StageManager(aheadStage);
        count = 0;
        reset = false;
    }

    // Update is called once per frame
    void Update() {
        int targetPosIndex = (int)(Target.position.z / StageSize);

        if(targetPosIndex + aheadStage > StageIndex) {
            StageManager(targetPosIndex + aheadStage);
        }

        if(reset){
            Reset();
            reset = false;
        }
    }

    void StageManager(int maps) {
        if (maps <= StageIndex) {
            return;
        }

        //指定したステージまで作成する
        for (int i = StageIndex + 1;i <= maps; i++) {
            GameObject stage = MakeStage(i);
            StageList.Add(stage);
        }

        //古いステージを削除する
        while (StageList.Count > aheadStage + 2) {
            DestroyStage();
        }

        StageIndex = maps;
    }

    //ステージを生成する
    GameObject MakeStage(int index) {
        int nextStage = Random.Range(0, stagenum.Length);
        count++;
        GameObject stageObject = (GameObject)Instantiate(stagenum[nextStage], new Vector3(0, -25, index * StageSize), Quaternion.identity);
        return stageObject;
    }

    void DestroyStage() {
        GameObject oldStage = StageList[1];
        StageList.RemoveAt(0);
        Destroy(oldStage);
    }

    void Reset() {
        while(StageList.Count > 1){
            DestroyStage();
        }
        // GameObject stage = (GameObject)Instantiate(firstStage, new Vector3(0, -5, 0), Quaternion.identity);
        // StageList.Add(stage);
        StageIndex = FirstStageIndex;
        StageManager(aheadStage);
        count = 0;
    }
    
}