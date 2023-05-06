using UnityEngine.SceneManagement;
using UnityEngine;

public class new_level : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int idx = SceneManager.GetActiveScene().buildIndex + 1;
        if (idx == SceneManager.sceneCountInBuildSettings)
            idx = 0;
        SceneManager.LoadScene(idx);
    }
}
