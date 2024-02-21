using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCoolGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] checkPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable() {
        foreach (GameObject checkPoint in checkPoints) {
            checkPoint.SetActive(true);
            Witch[] witches = gameObject.GetComponentsInChildren<Witch>();
            foreach (Witch witch in witches) {
                witch.Reset();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
