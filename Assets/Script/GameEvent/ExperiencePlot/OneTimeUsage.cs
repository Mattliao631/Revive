using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
public class OneTimeUsage : MonoBehaviour
{
    public string usageName;
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            if (UIManager.instance.IsGameRunning()) {
                OneTimeUsageManager.instance.playUsage(this);
            }
        }
    }
    public virtual void Play() {}
}
