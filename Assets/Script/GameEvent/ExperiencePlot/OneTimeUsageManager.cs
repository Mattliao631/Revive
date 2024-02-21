using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OneTimeUsageManager : MonoBehaviour
{
    
    public static OneTimeUsageManager instance;
    public GameObject plotVideoDisplay;
    public GameObject playingUI;
    public VideoPlayer videoPlayer;
    public PlayerController player;
    public GameObject[] enemies;
    private GameObject currentState = null;
    
    private PlayerStateList stateList;
    [SerializeField] private OneTimeUsage[] usageTable;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    void Start() {
        stateList = GameManager.instance.stateList;
        UpdateUsages();
    }
    public void UpdateUsages() {
        for (int i = 0; i < usageTable.Length; i++) {
            usageTable[i].gameObject.SetActive(!stateList.GetPlotFinished(usageTable[i].usageName));
            if (currentState != null) {
                currentState.SetActive(false);
                currentState = null;
            }
            player.GetComponent<PlayerSave>().canSave = true;
        }
    }
    
    public int playUsage(OneTimeUsage usage) {
        string usageName = usage.usageName;

        if (stateList.GetPlotFinished(usageName)) {
            return 0;
        }
        stateList.SetPlotFinished(usageName, true);
        usage.Play();
        return 0;
    }
    private int colosseumKnockDownCount;
    public void Colosseum(int mode) {
        if (mode == 0) {
            colosseumKnockDownCount = 0;
            currentState = transform.parent.Find("ColosseumState").gameObject;
            currentState.SetActive(true);
        } else if (mode == 1) {
            colosseumKnockDownCount++;
            if (colosseumKnockDownCount >= 1) {
                StartCoroutine(DeactivateStateInTheNextFrame());
                player.GetComponent<PlayerSave>().canSave = true;
            }
        }
    }
    public void Chasing(int mode) {
        if (mode == 0) {
            currentState = transform.parent.Find("ChasingState").gameObject;
            currentState.SetActive(true);
        } else if (mode == 1) {
            StartCoroutine(DeactivateStateInTheNextFrame());
            player.GetComponent<PlayerSave>().canSave = true;
        }
    }
    IEnumerator DeactivateStateInTheNextFrame() {
        yield return 0;
        if (currentState == null) {
            yield break;
        }
        currentState.SetActive(false);
    }
}
