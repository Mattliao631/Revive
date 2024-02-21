using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : CollectableObject
{
    // Start is called before the first frame update
    protected override void TouchedPlayer() {
        int currentHealth = stateList.GetCurrentHealth();
        if (currentHealth < stateList.GetMaxHealth()) {
            stateList.SetCurrentHealth(currentHealth + 1);
        }
    }
}
