using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    public string IDLE = "Idle";
    public string WALK = "Walk";
    public string RUN = "Run";
    public string JUMP = "Jump";
    public string ATTACK = "Attack";

    public Animator animator;
    public string currentState;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeState(string newState, float transitionDuration = 0.1f)
    {
        if (currentState == newState)
            return;

        // Check if the state exists in the Animator
        if (animator.HasState(0, Animator.StringToHash(newState)))
        {
            animator.CrossFade(newState, transitionDuration);
            currentState = newState;
        }
        else
        {
            Debug.LogWarning($"State '{newState}' does not exist in the Animator.");
        }
    }
}