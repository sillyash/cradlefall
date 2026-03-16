using UnityEngine;

public class BlinkingLightController : MonoBehaviour
{
    public bool ShouldBlink = false;
    public AnimationCurve pattern;
    public float IntensityMultiplier = 10;
    public float SpeedModifier = 1;
    public bool ShouldLoop = true;
    public LoopStyle loopStyle = LoopStyle.PingPong;

    public enum LoopStyle
    {
        Repeat,
        PingPong,
    }


    private Light Light;
    private float BaseIntensity;
    private float TimePast;
    private float curveEnd;
    private float curveStart;

    private void Start()
    {
        Light = GetComponent<Light>();
        BaseIntensity = Light.intensity;
    }

    private void Update()
    {
        if (ShouldBlink)
        {
            Blink();
        }
    }

    public void Blink()
    {
        curveEnd = pattern.keys[pattern.length - 1].time;
        curveStart = pattern.keys[0].time;
        TimePast += Time.deltaTime * SpeedModifier;
        float EvaluationPoint = 0;

        if (ShouldLoop)
        {
            switch (loopStyle)
            {
                case LoopStyle.Repeat:
                    if (TimePast > curveEnd) { TimePast = curveStart; }
                    EvaluationPoint = TimePast;
                    break;
                case LoopStyle.PingPong:
                    if (TimePast > curveEnd * 2) { TimePast = curveStart; }
                    if (TimePast > curveEnd)
                    {
                        EvaluationPoint = curveEnd - (TimePast - curveEnd);
                    } else
                    {
                        EvaluationPoint = TimePast;
                    }
                    break;
            }
        }
        float intensity = pattern.Evaluate(EvaluationPoint) * IntensityMultiplier;
        Light.intensity = intensity;
    }

    public void SwitchOnBlinking() { ShouldBlink = true; }
    public void SwitchOffBlinking() { ShouldBlink = false; Light.intensity = BaseIntensity; }
}
