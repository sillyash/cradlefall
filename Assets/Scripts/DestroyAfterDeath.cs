using UnityEngine;

public class DestroyAfterDeath : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IDamageable entity = animator.gameObject.GetComponent<IDamageable>();
        if (entity != null)
        {
            entity.isDead = true;
            Destroy(animator.gameObject);
        }
    }
}
