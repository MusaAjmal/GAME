using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    ParticleSystem ps;

    private void Start()
    {
       ps = GameObject.Find("CheckpointYellow").GetComponent<ParticleSystem>();
       
        animator = GetComponent<Animator>();
        Chest.Instance.SPcallback += animate;
    }
    public void Update()
    {
        if (!Chest.Instance.isPlayerClose())
        {
            animator.SetBool("PlayerInteracted", false);
        }
    }
    public void animate()
    { 
        
        ps.Play();
        animator.SetBool("PlayerInteracted",Chest.Instance.isPlayerClose());
       
    }

}
