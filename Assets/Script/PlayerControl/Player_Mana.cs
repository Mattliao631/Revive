using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mana : MonoBehaviour
{
    public int mana;
    public int max_mana = 4;

    // Start is called before the first frame update
    void Start()
    {
        mana = max_mana;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool cost_mana(int cost){
        if (mana >= cost){
            mana -= cost;
            return true;
        }else{
            Debug.Log("No Mana");
            return false;
        }
    }

}
