using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaNameManager : MonoBehaviour
{
    public static AreaNameManager instance;
    [SerializeField] GameObject[] areaNames;
    [SerializeField] private float increasingTime=1f;
    [SerializeField] private float holdTime=1f;
    [SerializeField] private float decreasingTime=1f;
    private int displaying = -1;

    void Awake() {
        if (instance==null) {
            instance = this;
        }
    }
    void Start()
    {

    }

    IEnumerator WaitForStartFinished() {
        while(!GameManager.instance.stateList.GetPlotFinished("Start")) {
            yield return new WaitForSeconds(1f);
        }

        areaNames[displaying].SetActive(true);
    }



    public void Display(int id) {
        if (displaying == id) {
            return;
        }
        if (displaying != -1) {
            areaNames[displaying].SetActive(false);
        }
        areaNames[id].SetActive(true);
        displaying = id;
        StartCoroutine(DoDisplay());
    }
    // public void Display(int id) {
    //     if (displaying == id) {
    //         return;
    //     }
    //     if (displaying != -1) {
    //         areaNames[displaying].SetActive(false);
    //     }
    //     areaNames[id].SetActive(true);
    //     displaying = id;
    //     StartCoroutine(DoDisplay());
    // }
    IEnumerator DoDisplay() {
        Image img = areaNames[displaying].GetComponent<Image>();
        img.fillOrigin = 0;
        float start = Time.time;
        while (Time.time <= start + increasingTime) {
            img.fillAmount = (Time.time - start) / increasingTime;
            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        img.fillOrigin = 1;
        start = Time.time;
        while (Time.time <= start + decreasingTime) {
            img.fillAmount = 1 - (Time.time - start) / decreasingTime;
            yield return null;
        }
        // displaying = -1;
        img.gameObject.SetActive(false);
    }
}