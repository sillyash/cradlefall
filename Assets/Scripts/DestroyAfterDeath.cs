using UnityEngine;

public class DestroyAfterDeath : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ICanDie entity = animator.gameObject.GetComponent<ICanDie>();
        if (entity != null)
        {
            entity.isDead = true;
            Destroy(animator.gameObject);
        }
    }
}
