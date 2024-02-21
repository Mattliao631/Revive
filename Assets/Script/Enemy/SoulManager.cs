using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    public static SoulManager instance;
    [SerializeField] private GameObject soulPrefab;
    public GameObject[] Souls;
    private Queue<int> availableSouls = new Queue<int>();
    private object lockObject = new object();
    
    private void GenerateSouls() {
        int currentLength = Souls.Length;
        int newLength = 2 * currentLength;

        GameObject[] tempSouls = new GameObject[newLength];
        Array.Copy(Souls, tempSouls, currentLength);

        for (int i = currentLength; i < newLength; i++) {
            tempSouls[i] = Instantiate(soulPrefab, transform);
            tempSouls[i].GetComponent<MonsterSoul>().index = i;
            tempSouls[i].SetActive(false);
            availableSouls.Enqueue(i);
        }
        Souls = tempSouls;
    }
    public int GetSoul() {
        lock (lockObject) {
            if (availableSouls.Count == 0) {
                GenerateSouls();
            }
            Debug.Log("Get Soul");
            int index = availableSouls.Dequeue();
            Souls[index].SetActive(true);
            Debug.Log($"Get Soul #{index}");
            return index;
        }
    }
    
    public void SoulReturn(int index) {
        lock (lockObject) {
            if (!Souls[index].activeSelf) {
                return;
            }
            Debug.Log($"Release Soul #{index}");
            Souls[index].SetActive(false);
            availableSouls.Enqueue(index);
        }
    }
    // Start is called before the first frame update
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    void Start()
    {
        for(int i = 0; i < Souls.Length; i++) {
            availableSouls.Enqueue(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
