using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activated;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        activated = true;
        anim.SetBool("active", true);
    }
}
