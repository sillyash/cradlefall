using UnityEngine;

public class BlinkingLightController : MonoBehaviour
{
    public bool lit;
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
    private float TimePast;
    private float curveEnd;
    private float curveStart;

    private void Start()
    {
        Light = GetComponent<Light>();
    }

    private void Update()
    {
        if (lit)
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

    public void SwitchOn() { lit = true; }
    public void SwitchOff() { lit = false; }
    public void Switch() { lit = !lit; }
}
