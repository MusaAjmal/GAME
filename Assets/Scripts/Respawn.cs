using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public static Respawn instance {  get; private set; }
    public Player player;
    private void Awake()
    {
        instance = this; 
       
    }
    private void Start()
    {
        player = Player.Instance;
    }
    public void RespawnPlayer()
    {
        player.transform.position = transform.position;
    }
}
