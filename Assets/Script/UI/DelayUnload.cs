using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayUnload : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;
    // Start is called before the first frame update
    public void LateUnload() {
        StartCoroutine(DelayUnloadCoroutine(delayTime));
    }
    public void LateUnloadForSeconds(float time) {
        StartCoroutine(DelayUnloadCoroutine(time));
    }

    IEnumerator DelayUnloadCoroutine(float time) {
        yield return new WaitForSeconds(time);
        
    }
}
