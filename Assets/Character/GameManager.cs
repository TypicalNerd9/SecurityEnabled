using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public double WorthCollected = 0;
    public double MaxWorth = 100;
    public GameObject WorthLabelObj;
    public GameObject GameOverPanel, WinPanel;
    public AudioClip Music, FastMusic;
    private TextMeshProUGUI WorthLabel;
    public Player player;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        WorthLabel = WorthLabelObj.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWorth(double Amount)
    {
        WorthCollected += Amount;
        WorthLabel.text = WorthCollected + "/" + MaxWorth;
        if (WorthCollected > MaxWorth)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool won)
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0;
        if (won)
        {
            WinPanel.SetActive(true);
        } else
        {
            GameOverPanel.SetActive(true);
        }
    }
}
