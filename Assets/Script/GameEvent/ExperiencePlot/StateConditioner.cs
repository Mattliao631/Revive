using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateConditioner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string functionName;


    public void StateConditionCall() {
        OneTimeUsageManager.instance.gameObject.SendMessage(functionName, 1);
    }
}
