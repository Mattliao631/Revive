using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health instance;
    public int health;
    public int max_health;
    private PlayerStateList stateList;

    public Image[] hearts;
    private Color empty_health = new Color(1f,1f,1f,0.1f);

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    void Start()
    {
        stateList = GameManager.instance.stateList;
        updateHealth();
    }

    // Update is called once per frame
    void Update()
    {   
        
    }
    public void updateHealth() {
        health = stateList.GetCurrentHealth();
        max_health = stateList.GetMaxHealth();

        for(int i = 0;i<hearts.Length;i++){
            if(i < health){
                hearts[i].GetComponent<Image>().color = Color.white;
            }else{
                hearts[i].GetComponent<Image>().color = empty_health;
            }
            
            if(i < max_health){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
    }
}
