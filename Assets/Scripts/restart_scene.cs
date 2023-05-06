using UnityEngine.SceneManagement;
using UnityEngine;

public class restart_scene : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
