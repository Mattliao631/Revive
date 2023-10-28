using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int max_health;
    [SerializeField] private GameObject player_hurt;

    public Image[] hearts;
    private Color empty_health = new Color(1f,1f,1f,0.1f);

    void Start()
    {
        health = player_hurt.GetComponent<Player_Hurt>().health;
        max_health = player_hurt.GetComponent<Player_Hurt>().max_health;
    }

    // Update is called once per frame
    void Update()
    {   
        health = player_hurt.GetComponent<Player_Hurt>().health;
        max_health = player_hurt.GetComponent<Player_Hurt>().max_health;

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
