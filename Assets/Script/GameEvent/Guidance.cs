using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guidance : MonoBehaviour
{
    [SerializeField] private GameObject guideUI;
    [SerializeField] private Sprite guideImage;
    [SerializeField] private GameObject hintButton;
    
    void Update() {
        if (hintButton.activeSelf && Input.GetKeyDown(KeyCode.R)) {
            if (guideUI.activeSelf) {
                UIManager.instance.popMenu();
            } else if (UIManager.instance.IsGameRunning()) {
                
                UIManager.instance.pushMenu(guideUI);
                guideUI.GetComponent<Image>().sprite = guideImage;
            }
        }
        // guideUI.SetActive((guideUI.activeSelf ^ Input.GetKeyDown("r")) && hintButton.activeSelf);
        // if (guideUI.activeSelf) {
        //     Time.timeScale = 0.0f;
        //     guideUI.GetComponent<Image>().sprite = guideImage;
        // } else {
        //     Time.timeScale = 1.0f;
        // }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            hintButton.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            hintButton.SetActive(false);
        }
    }
}
