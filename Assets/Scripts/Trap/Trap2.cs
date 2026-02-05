using UnityEngine;

public class Trap2 : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("On", true);
        }
    }
}
