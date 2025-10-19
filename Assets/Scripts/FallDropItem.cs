using UnityEngine;

public class FallDropItem : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] public float dropTime = 0.25f;

    [SerializeField] AudioClip audioClip;

    void Start()
    {
        Invoke("EndFall", dropTime);
    }

    void EndFall()
    {
        rb.gravityScale = 0f;
        anim.SetTrigger("BreakStuff");
        //audioSource.clip = audioClip;
        //audioSource.Play();
    }

    public void OnEndAnimation()
    {
        Destroy(this.gameObject);
    }
}
