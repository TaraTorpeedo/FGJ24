using System.Collections;
using System.Collections.Generic;
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
    public GameObject[] goblins;
    public GameObject goblinManager;
    public GameObject playerVirtualCamera;

    // Start is called before the first frame update
    public static GameManager Instance;

    void Start()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        StartCanvas.SetActive(true);
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
                    foreach (var goblin in goblins)
                    {
                        goblin.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void StartGame()
    {
        startPlayableDirector.Play();
    }
}
