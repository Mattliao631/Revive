using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{

    public Sprite img;
    public TextAsset txt;
    void Awake() {
    }
    void Start()
    {
        
        GetComponent<Button>().onClick.AddListener(Wrapper);
    }
    void OnEnable() {
        GetComponent<Image>().color = (GameManager.instance.stateList.GetAbilityLearned(gameObject.name)) ? Color.white : Color.gray;
    }

    void Wrapper() {
        AbilityPageManager.GetInstance.ClickCurrentAbility(gameObject);
    }
}
