using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [SerializeField] Canvas MenuCanvas;
    [SerializeField] Canvas WinCanvas;
    public bool beginPlay = false;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 0f;

        audioSource = GetComponent<AudioSource>();

        PlayMusic(0, true, 1);
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BeginPlay()
    {
        MenuCanvas.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        beginPlay = true;
        PlayMusic(1, true, 1);

    }
    public void YouWin()
    {
        WinCanvas.gameObject.SetActive(true);
        var bh = FindObjectOfType<ChasingBlackHole>();
        bh.gameObject.GetComponent<AudioSource>().Stop();
        Destroy(bh);
        PlayMusic(2, true, 1);
        Time.timeScale = 0.0f;


    }

    public void PlayMusic(int index, bool loop, float pitch)
    {
        audioSource.clip = audioClips[index];
        audioSource.loop = loop;
        audioSource.pitch = pitch;
        audioSource.ignoreListenerPause = true;
        audioSource.ignoreListenerVolume = true;
        audioSource.Play();
    }
}