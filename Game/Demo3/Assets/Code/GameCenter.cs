using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCenter : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1600, 900, false);

        Instance = this;
        _restartButton.onClick.AddListener(GameStart);
    }

    private void Start()
    {
        GameStart();
    }

    public void AddScore(int score)
    {
        Score += score;
        if (score % UPDIFFICULTY == 0)
        {
            _transmitter.UpgradeDifficulty();
        }
    }

    public void GameStart()
    {
        _core.Initialize();
        _transmitter.Initialize();
        Score = 0;
        _restartButton.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _transmitter.Shutdown();
        _restartButton.gameObject.SetActive(true);
    }

    private void SetBackGroundColor(Color color)
    {
        Camera.main.backgroundColor = color;
    }

    public static GameCenter Instance { get; private set; }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            _scoreText.text = string.Format("SCORE : {0}", _score);
        }
    }

    [SerializeField]
    private Core _core;

    [SerializeField]
    private Transmitter _transmitter;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Button _restartButton;

    private const int UPDIFFICULTY = 200;
}
