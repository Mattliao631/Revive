using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaItem : CollectableObject
{
    // Start is called before the first frame update

    protected override void TouchedPlayer() {
        int currentMana = stateList.GetCurrentMana();
        if (currentMana < stateList.GetMaxMana()) {
            stateList.SetCurrentMana(currentMana + 1);
        }
    }
}
