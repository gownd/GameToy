using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffBlock : MonoBehaviour
{
    [SerializeField] bool isReverse = true;
    [SerializeField] Switch mySwitch = null;

    bool isHiding;

    BoxCollider2D boxCollider;
    Animator animator;

    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();    
    }

    private void Start() 
    {
        isHiding = isReverse;
        animator.SetBool("isOn", isHiding);
        boxCollider.enabled = !isHiding;

        mySwitch.HandleSwitchOn += ToggleBlock;
        mySwitch.HandleSwitchOff += ToggleBlock;
    }

    void ToggleBlock()
    {
        isHiding = !isHiding;

        animator.SetBool("isOn", isHiding);
        boxCollider.enabled = !isHiding;
    }
}
