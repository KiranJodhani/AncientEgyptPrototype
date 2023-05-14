using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Inject]  CrystalFactory crystalFactory;

    public static GameManager Instance;

    public List<GameObject> SpawnPoints = new List<GameObject>();
    public Transform CrystalParent;
    
    public List<LevelConfigurations> LevelConfigurations = new List<LevelConfigurations>();

    public int CurrentLevel;
    public int Timer;
    public int Collection;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI CollectionText;
    public GameObject LevelFailedScreen;
    public GameObject LevelCompletedScreen;
    public GameObject[] PreLevelButton;
    public GameObject[] NextLevelButton;
    public ThirdPersonController thirdPersonController;
    public GameObject HUD;
    private void Awake()
    {
        
        Instance = this;
    }

    void Start()
    {
        StartLevel(0);
        
    }

  

    IEnumerator SpawnInitialCrystals()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnCrystal();
            yield return new WaitForSeconds(Random.Range(0, 5));
        }        
    }

    public void SpawnCrystal()
    {
        if(thirdPersonController.enabled)
        {
            Crystal crystal = crystalFactory.Create();
            crystal.gameObject.transform.localPosition = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position;
            crystal.transform.parent = CrystalParent.transform;
            crystal.SetModel();
        }
    }

    public void OnCrystalCollected()
    {
        Collection = Collection + 1;
        CollectionText.text="Crystals : "+Collection.ToString()+"/" + LevelConfigurations[CurrentLevel].CollectionTarget.ToString();
        if (Collection >= LevelConfigurations[CurrentLevel].CollectionTarget)
        {
            ManageLevelButtons();
            HUD.transform.DOMoveY(-400, 0.5f);
            LevelCompletedScreen.SetActive(true);
            thirdPersonController.enabled = false;
            thirdPersonController._animator.enabled = false;
        }
    }

    void CheckLevelStatus()
    {
        if (Collection >= LevelConfigurations[CurrentLevel].CollectionTarget)
        {
            ManageLevelButtons();
            LevelCompletedScreen.SetActive(true);
        }
        else
        {
            ManageLevelButtons();
            LevelFailedScreen.SetActive(true);
        }
    }

    public void StartLevel(int LevelIndex)
    {
        foreach(Transform CrystalTemp in CrystalParent)
        {
            Destroy(CrystalTemp.gameObject);
        }

        StartCoroutine(SpawnInitialCrystals());

        LevelCompletedScreen.SetActive(false);
        HUD.transform.DOMoveY(0, 0.5f);
        LevelFailedScreen.SetActive(false);

        thirdPersonController.enabled = true;
        thirdPersonController._animator.enabled = true;
       

        CurrentLevel = CurrentLevel + LevelIndex;
        LevelText.text="Level: "+ LevelConfigurations[CurrentLevel].LevelNumber.ToString();
        Timer = LevelConfigurations[CurrentLevel].TimeLimit;
        TimerText.text = "Timer :" + Timer.ToString();
        Collection = 0;
        CollectionText.text = "Crystals : " + Collection.ToString() + "/" + LevelConfigurations[CurrentLevel].CollectionTarget.ToString();
        Invoke("StartLevelTimer", 1);

    }

    void StartLevelTimer()
    {
        Timer = Timer - 1;
        if (Timer > 0)
        {
            TimerText.text = "Timer :" + Timer.ToString();
            Invoke("StartLevelTimer", 1);
        }
        else
        {
            thirdPersonController.enabled = false;
            thirdPersonController._animator.enabled = false;
            HUD.transform.DOMoveY(-400, 0.5f);
            CheckLevelStatus();
        }
        
    }

   

    public void ManageLevelButtons()
    {
        if (CurrentLevel == 0)
        {
            for (int i = 0; i < PreLevelButton.Length; i++)
            {
                PreLevelButton[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < PreLevelButton.Length; i++)
            {
                PreLevelButton[i].SetActive(true);
            }
        }
        
        if (CurrentLevel == LevelConfigurations.Count-1)
        {
            for (int i = 0; i < NextLevelButton.Length; i++)
            {
                NextLevelButton[i].SetActive(false);
            }
        }
        else 
        {
            for (int i = 0; i < NextLevelButton.Length; i++)
            {
                NextLevelButton[i].SetActive(true);
            }
        }
    }


}
