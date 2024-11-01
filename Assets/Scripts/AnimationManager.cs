using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public string IDLE = "Idle";
    public string WALK = "Walk";
    public string RUN = "Run";
    public string JUMP = "Jump";
    public string ATTACK = "Attack";
    public string DIE = "Die";


    public Animator animator;
    public string currentState;
    public static AnimationManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else

            Destroy(gameObject);
    }


    public void ChangeState(string newstate)
    {
        if (currentState == newstate)
            return;
        animator.Play(newstate);
        currentState = newstate;
    }
}
