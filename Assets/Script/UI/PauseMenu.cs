using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool game_is_paused = false;
    public GameObject pause_menu_UI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
    }

    public void Pause(){
        Time.timeScale = 0f;
        game_is_paused = true;
        pause_menu_UI.SetActive(true);  
    }

    public void Resume(){
        pause_menu_UI.SetActive(false);
        game_is_paused = false;
        Time.timeScale = 1f;
    }

    public void BacktoMenu(){
        SceneManager.LoadScene("MainMenu");
        Resume();
    }
}
