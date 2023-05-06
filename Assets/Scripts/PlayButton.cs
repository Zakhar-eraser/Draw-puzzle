using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private Animator truck_anim;
    [SerializeField]
    private Animator fade_anim;

    public void OnClick()
    {
        truck_anim.SetTrigger("Ride");
        new WaitForSeconds(1);
        fade_anim.SetTrigger("New level");
    }
}
