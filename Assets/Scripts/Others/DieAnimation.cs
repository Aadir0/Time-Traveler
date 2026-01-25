using UnityEngine;

public class DieAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    void Start()
    {
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
}
