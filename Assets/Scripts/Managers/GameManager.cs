using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DraftManager dm_DraftManager; 
    public static GameManager instance;

    //Teams
    public List<Team> teams = new List<Team>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (dm_DraftManager == null)
        {
            dm_DraftManager = FindObjectOfType<DraftManager>();
            print(dm_DraftManager.name);
        }
    }

    public void StartDraft()
    {
        if (dm_DraftManager != null)
        {
            dm_DraftManager.BeginDraft();
        }
    }
}
