using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public AudioListener listener;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject playUI;
    private Stack<GameObject> menuStack = new Stack<GameObject>();
    // Start is called before the first frame update
    private bool gameIsPaused = false;
    private object lockObject = new object();
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public bool IsGamePaused() {
        return gameIsPaused;
    }
    public bool IsGameRunning() {
        return !gameIsPaused;
    }
    private void TimeStop() {
        player.SetAcceptInput(false);
        playUI.SetActive(false);
        gameIsPaused = true;
        Time.timeScale = 0.0f;
    }
    private void TimeResume() {
        player.SetAcceptInput(true);
        playUI.SetActive(true);
        gameIsPaused = false;
        Time.timeScale = 1.0f;
    }
    public void pushMenu(GameObject menu) {
        Debug.Log("Push menu is called");
        menu.SetActive(true);
        lock(lockObject) {
            if (menuStack.Count != 0) {
                menuStack.Peek().SetActive(false);
            } else {
                
                TimeStop();
            }
            menuStack.Push(menu);
            // player.CallRun();
        }
    }
    public void popMenu() {
        lock(lockObject) {
            if (menuStack.Count == 0) {
                Debug.Log("Error! Can't pop from empty menu stack.");
                return;
            }

            GameObject menu = menuStack.Pop();
            menu.SetActive(false);
            if (menuStack.Count == 0) {
                TimeResume();
                return;
            }
            menuStack.Peek().SetActive(true);
        }
    }

    public void QuitGame() {
        TimeResume();
        SceneManager.LoadScene("MainMenu");
    }
}
