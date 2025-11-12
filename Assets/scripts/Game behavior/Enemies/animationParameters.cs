using UnityEngine;
using System.Collections;

public class animationParameters : MonoBehaviour
{
    private Animator animator;
    public enemyMovement goblin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null) 
        {
            animator.SetBool("isStopped", goblin.isStopped);
        }
    }
}
