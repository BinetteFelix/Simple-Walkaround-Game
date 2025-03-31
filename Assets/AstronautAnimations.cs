using UnityEngine;

public class AstronautAnimations : MonoBehaviour
{
    Animator a_Animator;
    float a_Float;

    private void Awake()
    {
        a_Animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void AnimatePlayer()
    {
        if (Input.GetButton("Horizontal") && !Player.Instance.p_IsDead)
        {
            a_Float = 0.6f;
            a_Animator.SetFloat("Speed", a_Float);
        }
        else if (Input.GetButtonDown("Jump") && !Player.Instance.p_IsDead)
        {
            a_Animator.SetTrigger("Jump");
            a_Animator.SetFloat("Speed", a_Float);
            a_Float = 0.4f;
        }
        else if (Player.Instance.p_IsDead)
        {
            a_Animator.SetTrigger("Death");
            Player.Instance.p_IsDead = true;
        }
        else
        {
            a_Float = 0.4f;
            a_Animator.SetFloat("Speed", a_Float);
        }
    }
}
