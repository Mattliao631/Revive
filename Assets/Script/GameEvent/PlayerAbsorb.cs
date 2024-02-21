using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbsorb : MonoBehaviour
{
    private float latestPress = 0f;
    public float particle_start_time;

    private float start_time = 3f;
    [SerializeField] GameObject particle;
    [SerializeField] GameObject absorb;

    [SerializeField] GameObject light1;
    [SerializeField] GameObject light2;
    [SerializeField] GameObject light3;

    [SerializeField] GameObject bat_particle;
    [SerializeField] GameObject bat_particle_2;
    private ParticleSystem ps1;
    private ParticleSystem ps2;
    private ParticleSystem.EmissionModule em1;
    private ParticleSystem.EmissionModule em2;

    private int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        ps1 = bat_particle.GetComponent<ParticleSystem>();
        ps2 = bat_particle_2.GetComponent<ParticleSystem>();

        em1 = ps1.emission;
        em2 = ps2.emission;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("u")) {
            latestPress = Time.time;
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            
        }
        if (Input.GetKey("u")) {
            // zoom in the camera FOV
            
            if (Time.time > latestPress + start_time && state == -1) {
                CameraManager.instance.shake(0);
                state = 0;
            }
            // animate sword effect
            if (Time.time > latestPress + start_time+ particle_start_time && state == 0) {
                particle.SetActive(true);
                state = 1;
            }

            if (Time.time > latestPress + start_time+ particle_start_time + 3f && state == 1) {
                absorb.SetActive(true);
                state = 2;
            }

            if (Time.time > latestPress + start_time+ particle_start_time + 3f + 4f && state == 2) {
                CameraManager.instance.shake(1);
                state = 3;
            }

            if (Time.time > latestPress + start_time+ particle_start_time + 7f + 1f && state == 3) {
                bat_particle.SetActive(true);
                bat_particle_2.SetActive(true);
                state = 4;
            }
            if(Time.time > latestPress + start_time+ particle_start_time + 8f + 10f && state == 4){
                em1.enabled = false;
                em2.enabled = false;
                state = 5;
            }
            if(Time.time > latestPress + start_time+ particle_start_time + 18f + 5f && state == 5){
                light1.SetActive(true);
                light2.SetActive(true);
                light3.SetActive(true);
                state = 6;
            }

            

        }
        if (Input.GetKeyUp("u")) {
            state = -1;
            particle.SetActive(false);
            absorb.SetActive(false);
            bat_particle.SetActive(false);
            bat_particle_2.SetActive(false);
            em1.enabled = true;
            em2.enabled = true;
            light1.SetActive(true);
            light2.SetActive(true);
            light3.SetActive(true);
        } 
    }
}
