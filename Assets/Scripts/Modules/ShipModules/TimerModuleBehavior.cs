using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Modules/Behaviours/Timer")]
public class TimerModuleBehavior : ModuleBehavior
{
    BaseModule module;

    private bool isTimerRunning = false;

    public override void OnBreak(BaseModule module)
    {
        this.module = module;
        module.StartCoroutine(CountDown());
    }

    public override void OnRepair(BaseModule module)
    {
        CancelTimer();
    }

    private IEnumerator CountDown()
    {
        if (module.stats is TimerModuleStats timerStats)
        {
            float t = timerStats.timerDuration;

            isTimerRunning = true;

            while (t > 0 && isTimerRunning)
            {
                t -= Time.deltaTime;
                // timerText.text = Mathf.Ceil(t).ToString();  Pour implémenter le texte a l'écran
                yield return null;
            }

            if (isTimerRunning)
            {
                isTimerRunning = false;
                Debug.Log("GAME OVER");
                // GameManager.instance.GameOver(); // TODO Replace with network GameOver
            } 
            else
            {
                yield break;
            }
        }
        else
        {
            Debug.Log("Le ShipModule n'a pas un SO stats de ShipModule");
        }
    }

    private void CancelTimer()
    {
        if (!isTimerRunning) return;
        isTimerRunning = false;
    }
}
