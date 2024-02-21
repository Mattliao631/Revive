using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static public MusicManager instance;
    private float volume = 1f;
    [SerializeField] private MusicBlock[] allMusic;
    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVolume(int iVolume) {
        volume = iVolume / 100f;
    }
    public void StopAllMusic() {
        for (int i = 0; i < allMusic.Length; i++) {
            if (allMusic[i].gameObject.activeInHierarchy) {
                allMusic[i].GetMusic().Pause();
            }
            
        }
    }
    public void ResumeAllMusic() {
        for (int i = 0; i < allMusic.Length; i++) {
            if (allMusic[i].gameObject.activeInHierarchy) {
                allMusic[i].GetMusic().UnPause();
            }
        }
    }
}
