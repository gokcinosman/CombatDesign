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
    private float defaultBlendSpeed = 0.1f;
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

    public void SetAnimationBlend(string animationState, float blendValue = 1f, float transitionDuration = 0.25f, float blendSpeed = -1f, string blendParam = "Blend")
    {
        blendSpeed = blendSpeed > 0 ? blendSpeed : defaultBlendSpeed;
        float currentBlend = animator.GetFloat(blendParam);
        animator.SetFloat(blendParam, Mathf.Lerp(currentBlend, blendValue, blendSpeed * Time.deltaTime));

        if (!IsInCurrentState(animationState))
        {
            animator.CrossFade(animationState, transitionDuration);
        }
    }

    private bool IsInCurrentState(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public void ResetBlend(string blendParam = "Blend")
    {
        animator.SetFloat(blendParam, 0f);
    }

    // Enable or disable layers for specific actions
    public void SetLayerWeight(string layerName, bool enabled)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        animator.SetLayerWeight(layerIndex, enabled ? 1 : 0);
    }


    public void StartRunning()
    {
        SetLayerWeight("Running", true);
        animator.CrossFade(RUN, 0.001f);
    }

    public void StopRunning()
    {
        SetLayerWeight("Running", false);
        animator.CrossFade(IDLE, 0.05f);
    }
}

