using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    public string currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeState(string newstate)
    {
        if (currentState != newstate) return;
        animator.Play(newstate);
        currentState = newstate;
    }
}