using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;


    private void Start()
    {
       
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
                animator.SetBool("PlayerInteracted",Chest.Instance.isPlayerClose());
       
    }

}
