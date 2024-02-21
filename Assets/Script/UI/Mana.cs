using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public static Mana instance;
    public int mana;
    public int max_mana;
    private PlayerStateList stateList;
    public Image[] manas;
    private Color empty_mana = new Color(1f,1f,1f,0.1f);

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    void Start()
    {
        stateList = GameManager.instance.stateList;
        updateMana();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateMana(){

        mana = stateList.GetCurrentMana();
        max_mana = stateList.GetMaxMana();
        
        for(int i = 0;i<manas.Length;i++){
            if(i < mana){
                manas[i].GetComponent<Image>().color = Color.white;
            }else{
                manas[i].GetComponent<Image>().color = empty_mana;
            }
            
            if(i < max_mana){
                manas[i].enabled = true;
            }else{
                manas[i].enabled = false;
            }
        }
    }
}
