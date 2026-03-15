using UnityEngine;

public class resetSecret : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BaseEnemy enemy = animator.gameObject.GetComponent<BaseEnemy>();

        animator.applyRootMotion = false;
        enemy.m_Agent.isStopped = false;
        enemy.isDead = true;
    }
}
