using UnityEngine;

public class AnimatorPlayspeedController : MonoBehaviour
{
    //Value from the slider, and it converts to speed level
    [SerializeField]
    [Range(0.01f, 3f)]
    private float animationSpeed;

    void Awake()
    {
        //Get the animator, attached to the GameObject you are intending to animate.
        GetComponent<Animator>().speed = animationSpeed;
    }
}
