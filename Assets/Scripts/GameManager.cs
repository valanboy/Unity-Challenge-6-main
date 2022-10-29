using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
    {
    public static GameManager singleton;
    private GroundPiece[] groundPieces;
    private TextMeshProUGUI levelText;
    private int maxIndex = 9;


    private void Start()
        {
        SetNewLevel();
        levelText = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
        }

    void SetNewLevel()
        {
        groundPieces = FindObjectsOfType<GroundPiece>();
        }

    private void Awake()
        {
        if (singleton == null)
            {
            singleton = this;
            }
        else if (singleton != null)
            {
            Destroy(singleton);
            DontDestroyOnLoad(gameObject);
            }
        }

    private void OnEnable()
        {
        SceneManager.sceneLoaded += OnLevelFinished;
        }

    void OnLevelFinished(Scene scene, LoadSceneMode mode)
        {
        SetNewLevel();
        }

    public void CheckComplete()
        {
        bool isFinished = false;
        int coloured = 0;
        foreach (GroundPiece ground in groundPieces)
            {
            if (ground.isColored)
                {
                coloured++;
                }
            }
        if (coloured == groundPieces.Length)
            {
            isFinished = true;
            }
        if (isFinished)
            {

            NextLevel();
            }
        }

    void NextLevel()
        {
        if (SceneManager.GetActiveScene().buildIndex == maxIndex)
            {
            SceneManager.LoadScene(0);
            }
        else
            {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }

    }
