using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mana : MonoBehaviour
{
    private PlayerStateList stateList;
    


    // Start is called before the first frame update
    void Awake() {
        
    }
    void Start()
    {
        stateList = GameManager.instance.stateList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetMana() {
        return stateList.GetCurrentMana();
    }
    public bool cost_mana(int cost){
        
        if (stateList.GetCurrentMana() >= cost){
            stateList.SetCurrentMana(stateList.GetCurrentMana() - cost);
            return true;
        }else{
            Debug.Log("No Mana");
            // play a sound to let player know they can't use the skill or save without mana
            // (optional) display a visual effect(like quick flash) on mana UI

            return false;
        }
    }

}
