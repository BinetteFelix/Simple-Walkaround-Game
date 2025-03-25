using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator walkingAnimator;
    private void Awake()
    {
        walkingAnimator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            walkingAnimator.SetTrigger("StartSmearedWalk");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            walkingAnimator.SetTrigger("StartWalk");
        }
        else
        {
            walkingAnimator.StopPlayback();
            walkingAnimator.ResetTrigger("StartWalk");
            walkingAnimator.ResetTrigger("StartSmearedWalk");
        }
    }
}

