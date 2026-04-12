using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Modules/Behaviours/Timer")]
public class TimerModuleBehavior : ModuleBehavior
{
    private BaseModule module;
    private bool isTimerRunning = false;
    private float t;

    public float remainingTime => t;
    public bool timerRunning => isTimerRunning;

    public override void OnBreak(BaseModule module)
    {
        this.module = module;
        LightManager.instance.MakeCapsulesBlink();
        module.StartCoroutine(CountDown());
    }

    public override void OnRepair(BaseModule module)
    {
        CancelTimer();
        LightManager.instance.StopCapsulesBlink();
    }

    private IEnumerator CountDown()
    {
        if (module.stats is TimerModuleStats timerStats)
        {
            t = timerStats.timerDuration; 
            isTimerRunning = true;
            while (t > 0 && isTimerRunning)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            if (isTimerRunning)
            {
                isTimerRunning = false;
                Debug.Log("GAME OVER");
                GameManager.instance.GameOver();
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