using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Soul : MonoBehaviour
{
    private PlayerStateList stateList;
    

    // Start is called before the first frame update
    void Awake() {
        
    }
    void Start()
    {
        stateList = GameManager.instance.stateList;
    }

    void Update(){
        // if(Input.GetKeyDown("p")){
        //     GainSoul(20);
        // }
    }
    public void GainSoul(int gain){
        stateList.SetSoul(stateList.GetSoul() + gain);
    }

    public void CostSoul(int cost){
        stateList.SetSoul(stateList.GetSoul() - cost);
    }
}
