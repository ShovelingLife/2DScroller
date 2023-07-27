using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonGlobal<LevelManager>
{
    [SerializeField]
    Player player;

    Vector3 playerStartPos;


    private void Awake()
    {
        playerStartPos = GameObject.Find("Player Start").transform.position;        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerOnStart() => player.transform.position = playerStartPos;
}
