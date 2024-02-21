using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition instance;

    public Animator animator;
    [HideInInspector] public bool isFading = false;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start() {
        
    }
    // void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    //     animator.SetTrigger("FadeIn");
    // }

    public void FadeIn() {
        if (isFading) {
            return;
        }
        isFading = true;
        animator.SetTrigger("FadeIn");
    }
    public void FadeOut() {
        if (isFading) {
            return;
        }
        isFading = true;
        Debug.Log("Fade out is executed!");
        animator.SetTrigger("FadeOut");
    }

    public void FadeFinish() {
        isFading = false;
    }
}
