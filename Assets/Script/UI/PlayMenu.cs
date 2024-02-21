using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    
    
    public void PlayGame(int recordNumber){
        GameManager.instance.Load(recordNumber);
    }
}
