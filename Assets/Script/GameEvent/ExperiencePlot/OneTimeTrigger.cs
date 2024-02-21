using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTrigger : OneTimeUsage
{
    [SerializeField] private string functionName;
    public override void Play() {
        OneTimeUsageManager.instance.gameObject.SendMessage(functionName, value: 0);
        OneTimeUsageManager.instance.player.GetComponent<PlayerSave>().canSave = false;
        gameObject.SetActive(false);
    }
}
