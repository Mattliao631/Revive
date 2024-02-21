using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_InputSystem : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button resumeButton;
    [SerializeField] GameObject abilityMenu;
    [SerializeField] GameObject abilityTreeContent;
    [SerializeField] GameObject abilityDescription;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (UIManager.instance.IsGameRunning()) {
                UIManager.instance.pushMenu(pauseMenu);
                resumeButton.Select();
            }
        } else if (Input.GetKeyDown(KeyCode.I)) {
            if (UIManager.instance.IsGameRunning()) {
                UIManager.instance.pushMenu(abilityMenu);
                abilityTreeContent.GetComponent<RectTransform>().localPosition = new Vector3(-400f,660f,0f);
                
            } else if (abilityMenu.activeSelf) {
                abilityDescription.SetActive(false);
                AbilityPageManager.GetInstance.currentAbilityName = "";
                UIManager.instance.popMenu();
            }
        }
    }
}
