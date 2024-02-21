using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Down_Smash : MonoBehaviour
{
    [SerializeField] int downSmashDamageFactor = 3;
    [SerializeField] Player_Attack attack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            GameObject otherGameObj = other.gameObject;
            GameObject otherParent = otherGameObj.transform.parent.gameObject;
            otherParent.GetComponent<Enemy>().Take_Damage(attack.GetDamage() * downSmashDamageFactor);
        }
    }
}
