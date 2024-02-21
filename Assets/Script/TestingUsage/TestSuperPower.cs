using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSuperPower : MonoBehaviour
{
    public GameObject player;
    public GameObject player_hurt;

    public float dashVelocity = 10;
    public float floatVelocity = 1;

    private PlayerStateList stateList;

    // Start is called before the first frame update
    void Start()
    {
        stateList = GameManager.instance.stateList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h")) {
            stateList.SetCurrentHealth(stateList.GetCurrentHealth() + 1);
        } else if (Input.GetKeyDown("m")) {
            stateList.SetCurrentMana(stateList.GetCurrentMana() + 1);
        } else if (Input.GetKeyDown("v")) {
            player_hurt.GetComponent<Player_Hurt>().super = true;
        }
        if (Input.GetKeyDown("x")) {
            Debug.Log("Dash!");
            player.transform.position += new Vector3(dashVelocity * player.GetComponent<PlayerController>().face_Dir, 0, 0);
        }
        if (Input.GetKey("c")) {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, floatVelocity);
        }
    }
}
