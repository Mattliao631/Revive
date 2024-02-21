using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dialog : MonoBehaviour
{
    [Header("Source files")]
    [SerializeField] private TextAsset file;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("Chat hint")]
    public GameObject Button;

    [Header("UI display")]
    public GameObject talkUI;
    
    
    [Header("Text display")]
    public TextMeshProUGUI plotText;
    [Header("Image display")]
    public Image leftImage;
    public Image rightImage;
    
    

    [Header("Corpus data")]
    private List<string> plotContent = new List<string>();
    private List<string> speaker = new List<string>();

    [Header("Plot control")]
    private int contentCount = 0;
    private bool isTalking = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Button.SetActive(false);
        }
        
    }

    void GetTextFromFile(TextAsset file) {
        speaker.Clear();
        plotContent.Clear();
        var lineData = file.text.Split('\n');



        for(int i = 0; i < lineData.Length; i++) {
            if (i % 2 == 0) {
                speaker.Add(lineData[i]);
            } else {
                plotContent.Add(lineData[i]);
            }
        }

    }
    private void Awake() {
        GetTextFromFile(file);
        
    }
    private void Start() {

    }
    private void Update()
    {
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.R) && !isTalking)
        {
            isTalking = true;
            // talkUI.SetActive(isTalking);
            if (UIManager.instance.IsGamePaused()) {
                return;
            }
            UIManager.instance.pushMenu(talkUI);
            contentCount = 0;
            Debug.Log(plotText);
            Debug.Log(plotText.text);
            plotText.text=plotContent[contentCount];
            leftImage.sprite = leftSprite;
            rightImage.sprite = rightSprite;
            if (speaker[contentCount] == "A") {
                leftImage.color = Color.white;
            }
            rightImage.color = Color.gray;
        } else if (isTalking && Input.GetKeyDown(KeyCode.R)) {

            contentCount++;
            if (contentCount == plotContent.Count) {
                // Colse UI text part
                isTalking = false;
                UIManager.instance.popMenu();
                contentCount=0;
                // Update plotContent if needed
            }
            Debug.Log(plotText);
            Debug.Log(plotText.text);
            plotText.text=plotContent[contentCount];
            if (speaker[contentCount] == "A") {
                leftImage.color = Color.white;
                rightImage.color = Color.gray;
            } else {
                leftImage.color = Color.gray;
                rightImage.color = Color.white;
            }
            
            // Update the speaker graphic with graph[speaker[i]]
        }
    }

}
