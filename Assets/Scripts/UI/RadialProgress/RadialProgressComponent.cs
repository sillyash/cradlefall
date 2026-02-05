using UnityEngine;
using UnityEngine.UIElements;

namespace ScrumbagsUI
{
    [RequireComponent(typeof(UIDocument))]
    public class RadialProgressComponent : MonoBehaviour
    {
        RadialProgress m_RadialProgress;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            m_RadialProgress = new RadialProgress
            {
                style = {
                    position = Position.Absolute,
                    left = Length.Percent(50),
                    top = Length.Percent(50),
                    width = 200,
                    height = 200,
                    translate = new Translate(-100, -100, 0)
                }
            };

            root.Add(m_RadialProgress);
        }

        void Update()
        {
            m_RadialProgress.progress = ((Mathf.Sin(Time.time) + 1.0f) / 2.0f) * 60.0f + 10.0f;
        }
    }
}
