using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateList
{
    public int currentHealth = 4;
    public int maxHealth = 4;

    public int currentMana = 0;
    public int maxMana = 2;
    
    public int soul = 0;

    public float x;
    public float y;
    public float z;
    public string sceneCode;
    
    public Dictionary<string, bool> skillLearned = new Dictionary<string, bool>() {};
    public Dictionary<string, bool> plotFinished = new Dictionary<string, bool>() {};
    public Dictionary<string, bool> abilityLearned = new Dictionary<string, bool>() {};

    public void CopyFrom(PlayerStateList source) {
        this.currentHealth = source.currentHealth;
        this.maxHealth = source.maxHealth;
        this.currentMana = source.currentMana;
        this.maxMana = source.maxMana;
        this.soul = source.soul;
        this.x = source.x;
        this.y = source.y;
        this.z = source.z;
        this.sceneCode = source.sceneCode;
        this.skillLearned = source.skillLearned;
        this.plotFinished = source.plotFinished;
        this.abilityLearned = source.abilityLearned;
    }
    public PlayerStateList() {
        x = -182; y = 172; z = 0;
        soul = 0;

        sceneCode = "Chapter1_1";

        skillLearned.Add("Double Jump", false);
        skillLearned.Add("Down Smash", false);
        skillLearned.Add("Attack Extra Jump", false);
        skillLearned.Add("Bat Transmit", false);

        abilityLearned.Add("Polished", false);
        abilityLearned.Add("Blade Extend", false);

        abilityLearned.Add("Reorganize", false);
        abilityLearned.Add("Fearless", false);

        abilityLearned.Add("Soul Utilization", false);
        abilityLearned.Add("Knock Back", false);

        plotFinished.Add("Start", false);
        plotFinished.Add("Elevator", false);
        plotFinished.Add("Colosseum", false);
        plotFinished.Add("RepairElevator", false);
    }
    public PlayerStateList(string mode, string sceneCode, float x, float y) {
        if (mode == "Test") {
            this.x = x; this.y = y; this.z = 0;

            this.sceneCode = sceneCode;

            skillLearned.Add("Double Jump", true);
            skillLearned.Add("Down Smash", true);
            skillLearned.Add("Attack Extra Jump", true);
            skillLearned.Add("Bat Transmit", true);

            abilityLearned.Add("Polished", false);
            abilityLearned.Add("Blade Extend", true);

            abilityLearned.Add("Reorganize", true);
            abilityLearned.Add("Fearless", false);

            abilityLearned.Add("Soul Utilization", true);
            abilityLearned.Add("Knock Back", false);

            plotFinished.Add("Start", false);
            plotFinished.Add("Elevator", false);
            plotFinished.Add("Colosseum", false);
            plotFinished.Add("RepairElevator", false);
        }
    }
    public void SetCurrentMana(int amount) {
        if (amount > maxMana) {
            amount = maxMana;
        }
        currentMana = amount;
        Mana.instance.updateMana();
    }
    public int GetCurrentMana() {
        return currentMana;
    }
    public void SetMaxMana(int amount) {
        maxMana = amount;
        Mana.instance.updateMana();
    }
    public int GetMaxMana() {
        return maxMana;
    }
    public void SetCurrentHealth(int amount) {
        if (amount > maxHealth) {
            amount = maxHealth;
        }
        currentHealth = amount;
        Health.instance.updateHealth();
    }
    public int GetCurrentHealth() {
        return currentHealth;
    }
    public void SetMaxHealth(int amount) {
        maxHealth = amount;
        Health.instance.updateHealth();
    }
    public int GetMaxHealth() {
        return maxHealth;
    }
    public void SetSoul(int amount) {
        soul = amount;
        Soul.instance.updateSoul();
    }

    public int GetSoul() {
        return soul;
    }
    public void AddCurrentHealth(int amount) {
        SetCurrentHealth(currentHealth + amount);
    }
    public void AddCurrentMana(int amount) {
        SetCurrentMana(currentMana + amount);
    }
    public void AddSoul(int amount) {
        SetSoul(soul + amount);
    }

    public void SetPosition(Vector3 position) {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }

    public Vector3 GetPosition() {
        return new Vector3(x, y, z);
    }
    public void SetSceneCode(string code) {
        sceneCode = code;
    }

    public bool GetAbilityLearned(string abilityName) {
        if (abilityLearned.ContainsKey(abilityName)) {
            return abilityLearned[abilityName];
        } else {
            return false;
        }
    }
    public void SetAbilityLearned(string abilityName, bool learned) {
        if (abilityLearned.ContainsKey(abilityName)) {
            abilityLearned[abilityName] = learned;

        } else {
            abilityLearned.Add(abilityName, learned);
        }
    }

    public bool GetPlotFinished(string plotName) {
        if (plotFinished.ContainsKey(plotName)) {
            return plotFinished[plotName];
        } else {
            return false;
        }
    }
    public void SetPlotFinished(string plotName, bool finished) {
        if (plotFinished.ContainsKey(plotName)) {
            plotFinished[plotName] = finished;
        } else {
            plotFinished.Add(plotName, finished);
        }
    }

    public bool GetSkillLearned(string skillName) {
        if (skillLearned.ContainsKey(skillName)) {
            return skillLearned[skillName];
        } else {
            return false;
        }
    }
    public void SetSkillLearned(string skillName, bool learned) {
        if (skillLearned.ContainsKey(skillName)) {
            skillLearned[skillName] = learned;
        } else {
            skillLearned.Add(skillName, learned);
        }
    }

    public void RecoverMaxHMp() {
        SetCurrentHealth(GetMaxHealth());
        SetCurrentMana(GetMaxMana());
    }
}
