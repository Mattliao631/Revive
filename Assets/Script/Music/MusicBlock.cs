using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBlock : MonoBehaviour
{
    [SerializeField] private float fadeInRate = 0.1f;
    [SerializeField] private float fadeOutRate = 0.5f;
    private AudioSource music;
    private float volume = 0f;
    private bool inSide = false;

    public AudioSource GetMusic() {
        return music;
    }
    void Start() {
        music = gameObject.GetComponent<AudioSource>();
    }
    void Update() {
        if (inSide && volume < 1f) {
            volume += Time.deltaTime * fadeInRate;
            if (volume > 1f) volume = 1f;
        } else if (!inSide && volume > 0f) {
            volume -= Time.deltaTime * fadeOutRate;
            if (volume < 0f) volume = 0f;
        }

        music.spatialBlend = 1f - volume;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            inSide = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            inSide = false;
        }
    }
    public void Cut() {
        volume = 0;
    }
}
