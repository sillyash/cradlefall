using UnityEngine;

public class DestroyAfterDeath : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ICanDie entity = animator.gameObject.GetComponent<ICanDie>();
        entity.isDead = true;
        animator.SetBool("isDead", true);
        animator.Update(0);
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
