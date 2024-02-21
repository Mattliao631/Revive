using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soul : MonoBehaviour
{
    public static Soul instance;
    private Image soul_image;
    public float soul;
    private PlayerStateList stateList;

    

    void Awake(){
        if (instance == null) {
            instance = this;
        }
    }

    void Start(){
        stateList = GameManager.instance.stateList;
        soul_image = GetComponent<Image>();
        updateSoul();
    }

    void Update(){
        
    }

    public void updateSoul(){
        soul = stateList.GetSoul();
        soul_image.fillAmount = soul/100;
        CanUpgrade();
    }
    void CanUpgrade(){
        if(soul >= 100){
            Debug.Log("canupgrade");
            // shinning
        }else{
            // disshinning
        }
    }
    
}
