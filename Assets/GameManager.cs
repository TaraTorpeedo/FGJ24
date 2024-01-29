using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public PlayableDirector startPlayableDirector;
    public PlayableDirector deadPlayableDirector;
    public PlayableDirector winPlayableDirector;

    public GameObject StartCanvas;
    public GameObject player;
    public GameObject king;
    public GameObject goblinManager;
    public GameObject playerVirtualCamera;

    public GameObject[] goblins;
    public GameObject[] foods;

    public GameObject loseCanvas;
    public GameObject winCanvas;

    public GameObject GameUI;


    void Start()
    {
        foreach (var goblin in goblins)
        {
            goblin.gameObject.SetActive(false);
        }

        foreach (var food in foods)
        {
            food.gameObject.SetActive(false);
        }

        StartCanvas.SetActive(true);
        InputManager.Instance.SetCurscorVisible(true);

   
        startPlayableDirector.gameObject.SetActive(true);
        deadPlayableDirector.gameObject.SetActive(false);
        winPlayableDirector.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartCanvas.gameObject.activeInHierarchy)
        {
            if (startPlayableDirector.gameObject.activeInHierarchy)
            {
                if (startPlayableDirector.state != PlayState.Playing)
                {
                    startPlayableDirector.gameObject.SetActive(false);
                    playerVirtualCamera.SetActive(true);   
                    player.SetActive(true);
                    king.SetActive(true);
                    goblinManager.SetActive(true);
                    GameUI.SetActive(true);

                    foreach (var goblin in goblins)
                    {
                        goblin.gameObject.SetActive(true);
                    }

                    foreach (var food in foods)
                    {
                        food.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void WinGame()
    {
        playerVirtualCamera.SetActive(false);
        player.SetActive(false);
        king.SetActive(false);
        goblinManager.SetActive(false);
        GameUI.SetActive(false);
        InputManager.Instance.SetCurscorVisible(true);

        foreach (var goblin in goblins)
        {
            goblin.gameObject.SetActive(false);
        }

        foreach (var food in foods)
        {
            food.gameObject.SetActive(false);
        }

        winCanvas.SetActive(true);
        winPlayableDirector.gameObject.SetActive(true);
        winPlayableDirector.Play();
    }

    public void LoseGame()
    {
        playerVirtualCamera.SetActive(false);
        player.SetActive(false);
        king.SetActive(false);
        goblinManager.SetActive(false);
        GameUI.SetActive(false);
        InputManager.Instance.SetCurscorVisible(true);

        foreach (var goblin in goblins)
        {
            goblin.gameObject.SetActive(false);
        }

        foreach (var food in foods)
        {
            food.gameObject.SetActive(false);
        }

        loseCanvas.SetActive(true);
        deadPlayableDirector.gameObject.SetActive(true);
        deadPlayableDirector.Play();
    }

    public void StartGame()
    {
        startPlayableDirector.Play();
        InputManager.Instance.SetCurscorVisible(false);
    }
}
