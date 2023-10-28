using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public int mana;
    public int max_mana;
    [SerializeField] private GameObject player_mana;

    public Image[] manas;
    private Color empty_mana = new Color(1f,1f,1f,0.1f);

    void Start()
    {
        mana = player_mana.GetComponent<Player_Mana>().mana;
        max_mana = player_mana.GetComponent<Player_Mana>().max_mana;
        update_mana();
    }

    // Update is called once per frame
    void Update()
    {
        update_mana();
    }

    public void update_mana(){

        mana = player_mana.GetComponent<Player_Mana>().mana;
        max_mana = player_mana.GetComponent<Player_Mana>().max_mana;
        
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
