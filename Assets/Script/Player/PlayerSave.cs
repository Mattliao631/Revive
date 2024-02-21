using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSave : MonoBehaviour
{
    private PlayerStateList stateList;
    // private float latestPress = 0f;
    private PlayerController controller;
    private bool notSaved = true;
    [HideInInspector] public bool canSave = true;

    [SerializeField] GameObject Gate;
    [SerializeField] GameObject Sword_Effect;
    [SerializeField] GameObject Absorb_Effect;

    [SerializeField] private Player_Mana player_mana;

    // public float start_time;
    [SerializeField] private float saveWaitTime = 3f;
    private float spaceDownTime;
    private bool spaceDown = false;
    private bool lastSpaceDown = false;

    void Start() {
        stateList = GameManager.instance.stateList;
        controller = GetComponent<PlayerController>();
    }
    void SaveDetection() {
        if (Input.GetKey("space")) {
            spaceDown = true;
        }
        if (Input.GetKeyUp("space")) {
            spaceDown = false;
        }
    }
    void SaveAction() {
        // space up
        if (!spaceDown && lastSpaceDown) {
            notSaved = true;
            controller.SetAcceptInput(true);
            spaceDownTime = 0f;

            controller.Set_Save_Animator(false);
            Sword_Effect.SetActive(false);
            Absorb_Effect.SetActive(false);
        }
        lastSpaceDown = spaceDown;

        // condition that you can't save
        if (!canSave || stateList.GetCurrentMana() < 1 || !controller.lastGrounded) {
            return;
        }

        if (spaceDown && notSaved) {
            if (spaceDownTime == 0f) {
                controller.SetAcceptInput(false);
                controller.Set_Save_Animator(true);
                Sword_Effect.SetActive(true);
                Absorb_Effect.SetActive(true);
            }
            
            spaceDownTime += Time.deltaTime;
            controller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, controller.GetComponent<Rigidbody2D>().velocity.y);

            if (spaceDownTime >= saveWaitTime) {
                notSaved = false;
                spaceDownTime = 0f;
                

                if(player_mana.cost_mana(1)){
                    Reorganize();

                    stateList.SetPosition(transform.position);
                    GameManager.instance.Save();

                    Debug.Log("Saved!");
                    notSaved = false;
                    
                    controller.Set_Save_Animator(false);
                    Sword_Effect.SetActive(false);
                    Absorb_Effect.SetActive(false);
                    
                    // reposition the gate
                    Gate.transform.position = transform.position;
                }
            }
        }
    }

    private void Reorganize() {
        if(stateList.GetAbilityLearned("Reorganize")){
            stateList.AddCurrentHealth(1);
        }
    }
    void Update() {
        SaveDetection();
        SaveAction();
    }
}
