using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlower : MonoBehaviour
{
    [SerializeField] float coolTime = 1f;
    [SerializeField] FireBall fireballPrefab = null;
    [SerializeField] Transform launchPoint = null;

    float currentCoolTime = 0f;
    bool isLaunching = false;

    Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();    
    }

    private void Update() 
    {
        currentCoolTime += Time.deltaTime;

        if(currentCoolTime >= coolTime)
        {
            currentCoolTime = 0f;

            if(!isLaunching)
            {
                animator.SetBool("isLaunching", true);
                FireBall newFireBall = Instantiate(fireballPrefab, launchPoint.position, Quaternion.identity, transform);
                newFireBall.SetDirection(transform.eulerAngles.z);
            }
            else
            {
                animator.SetBool("isLaunching", false);
            }

            isLaunching = !isLaunching;
        }    
    }
}
