using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
public class VideoPlot : OneTimeUsage
{
    [SerializeField] private VideoClip videoClip;
    
    public override void Play() {
        Transition.instance.FadeOut();
        
        Debug.Log($"Accept input: {OneTimeUsageManager.instance.player.GetAcceptInput()}.");
        // for(int i = 0; i < OneTimeUsageManager.instance.enemies.Length; i++) {
        //     OneTimeUsageManager.instance.enemies[i].SetActive(false);
        // }
        StartCoroutine(FadeToVideoScreen());
    }
    IEnumerator FadeToVideoScreen() {
        while(Transition.instance.isFading) {
            yield return null;
        }
        MusicManager.instance.StopAllMusic();
        
        
        OneTimeUsageManager.instance.videoPlayer.clip = videoClip;

        

        OneTimeUsageManager.instance.videoPlayer.Play();
        UIManager.instance.pushMenu(OneTimeUsageManager.instance.plotVideoDisplay);
        Transition.instance.FadeIn();
        
        Debug.Log(OneTimeUsageManager.instance.plotVideoDisplay.activeSelf);

        StartCoroutine(WaitForVideoFinished());
        Debug.Log($"\"{usageName}\" is played");
    }

    IEnumerator WaitForVideoFinished() {
        VideoPlayer player = OneTimeUsageManager.instance.videoPlayer;
        while (player.frame < (long)(player.frameCount - 1)) {
            Debug.Log("Still playing.");
            yield return null;
        }
        Transition.instance.FadeOut();
        StartCoroutine(FadeOutVideoScreen());
        
        
        

        // for(int i = 0; i < OneTimeUsageManager.instance.enemies.Length; i++) {
        //     OneTimeUsageManager.instance.enemies[i].SetActive(true);
        // }
    }

    IEnumerator FadeOutVideoScreen() {
        while(Transition.instance.isFading) {
            Debug.Log("Still fading.");
            yield return null;
        }
        UIManager.instance.popMenu();
        OneTimeUsageManager.instance.videoPlayer.clip = null;
        MusicManager.instance.ResumeAllMusic();
        Transition.instance.FadeIn();
    }
}
