using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
public class LearnSkill : VideoPlot
{
    private PlayerStateList stateList;
    [SerializeField] private string skillName;
    void Awake() {
        
    }
    void Start() {
        stateList = GameManager.instance.stateList;
    }
    
    public override void Play() {
        stateList.SetSkillLearned(skillName, true);
        stateList.RecoverMaxHMp();
        stateList.SetPosition(transform.position);
        GameManager.instance.Save();
        base.Play();
    }
}
