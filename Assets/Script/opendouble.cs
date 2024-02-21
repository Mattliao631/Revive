using System.Collections;
using System.Collections.Generic;
using Pathfinding.Ionic.Zip;
using UnityEngine;

public class opendouble : MonoBehaviour
{
    private int state;
    [SerializeField] GameObject double_guide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("p")){
            switch(state){
                case 0:
                    double_guide.SetActive(true);
                    state = 1;
                    break;
                case 1:
                    double_guide.SetActive(false);
                    state = 0;
                    break;
            }
                
        }
    }
}
