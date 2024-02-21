using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class AbilityPageManager : MonoBehaviour
{
    private static AbilityPageManager instance;
    private static readonly object lockObject = new object();


    private PlayerStateList stateList;
    [HideInInspector] public string currentAbilityName = "Name";
    [HideInInspector] public GameObject currentAbility;
    [SerializeField] private GameObject descriptionBlock;
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject learned_box;
    
    public static AbilityPageManager GetInstance{
        get {
            if (instance == null) {
                lock (lockObject) {
                    if (instance == null) {
                        instance = new AbilityPageManager();
                    }
                }
            }
            return instance;
        }
    }
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    void Start() {
        stateList = GameManager.instance.stateList;

    }

    public void ClickCurrentAbility(GameObject ability) {
        if (ability.name == currentAbilityName) {
            return;
        }
        if (!descriptionBlock.activeSelf) {
            descriptionBlock.SetActive(true);
        }
        lock(lockObject) {
            currentAbilityName = ability.name;
            currentAbility = ability;

            abilityName.text = currentAbilityName;
            abilityImage.sprite = currentAbility.GetComponent<Ability>().img;
            description.text = currentAbility.GetComponent<Ability>().txt.text;
            if(stateList.GetAbilityLearned(currentAbilityName)){
                learned_box.SetActive(false);
            }else{
                learned_box.SetActive(true);
                int soul = stateList.GetSoul();
                if (soul < 100) {
                    learned_box.GetComponent<Button>().interactable = false;
                    learned_box.GetComponent<Image>().color = Color.gray;
                } else {
                    learned_box.GetComponent<Button>().interactable = true;
                    learned_box.GetComponent<Image>().color = new Color(171, 0, 255);
                }
            }
        }
    }

    public void ClickLearn() {
        if (currentAbilityName == null) {
            return;
        }
        if (!stateList.GetAbilityLearned(currentAbilityName)) {
            int soul = stateList.GetSoul();
            if (soul >= 100) {
                stateList.SetSoul(soul - 100);
                stateList.SetAbilityLearned(currentAbilityName, true);
                currentAbility.GetComponent<Image>().color = Color.white;
                learned_box.SetActive(false);
            } else {
                Debug.Log("You don't have enough soul to acquire new ability");
            }
        } else {
            Debug.Log("Ability already acquired");
        }
        
    }
}
