using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int recordNumber {get; private set;}
    private GameObject tempCamera;
    private GameObject player;
    private string filePath;
    public PlayerStateList stateList;
    public bool dead = false;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        stateList = new PlayerStateList("Test", SceneManager.GetActiveScene().name, this.transform.position.x, this.transform.position.y);
        SetRecordNumber(999);
        SetFilePath($"/Record{999}.json");
        DontDestroyOnLoad(gameObject);
    }


    public void SetRecordNumber(int number) {
        recordNumber = number;
    }
    public void SetFilePath(string fileName) {
        filePath = Application.persistentDataPath + fileName;
    }

    public void Save() {
        string data = JsonConvert.SerializeObject(stateList);
        Debug.Log(filePath);
        Debug.Log(data);
        System.IO.File.WriteAllText(filePath, data);
    }

    public void Load(int recordNumber) {
        tempCamera = GameObject.Find("Main Camera");
        DontDestroyOnLoad(tempCamera);
        tempCamera.GetComponent<DelayUnload>().LateUnload();
        Transition.instance.FadeOut();

        this.recordNumber = recordNumber;
        filePath = Application.persistentDataPath + $"/Record{this.recordNumber}.json";
        Debug.Log(filePath);
        Debug.Log(recordNumber);

        if (System.IO.File.Exists(filePath)) {
            // read string from .json file
            string data = System.IO.File.ReadAllText(filePath);
            // deserialize from string to PlayerStateList
            stateList = JsonConvert.DeserializeObject<PlayerStateList>(data);
        } else {
            // new game
            stateList = new PlayerStateList();
            Save();
        }
        StartCoroutine(LoadSceneAsyncCoroutine(stateList.sceneCode));

    }
    IEnumerator LoadSceneAsyncCoroutine(string sceneCode) {
        while(Transition.instance.isFading) {
            yield return null;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneCode);

        while(!asyncLoad.isDone) {
            yield return null;
        }
        Destroy(tempCamera);
        Transition.instance.FadeIn();
    }
    public void LoadInScene() {
        player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().SetAcceptInput(false);
        dead = true;
        Transition.instance.FadeOut();
        Debug.Log("Load in scene");
        
        
        
        StartCoroutine(WaitForFadOutFinish());
        
    }
    IEnumerator WaitForFadOutFinish() {
        while(Transition.instance.isFading) {
            yield return null;
        }
        string data = System.IO.File.ReadAllText(filePath);
        stateList.CopyFrom(JsonConvert.DeserializeObject<PlayerStateList>(data));
        
        player.transform.position = stateList.GetPosition();
        Mana.instance.updateMana();
        Health.instance.updateHealth();
        Soul.instance.updateSoul();
        
        OneTimeUsageManager.instance.UpdateUsages();
        
        dead = false;
        player.GetComponent<PlayerController>().SetAcceptInput(true);
        Transition.instance.FadeIn();
    }
}
