using UnityEngine;
using UnityEngine.UIElements;

namespace ScrumbagsUI
{
    [RequireComponent(typeof(UIDocument))]
    public class RadialProgressComponent : MonoBehaviour
    {
        RadialProgress m_RadialProgress;

        [SerializeField]
        private float minWidth = 26f;
        [SerializeField]
        private float minHeight = 20f;
        [SerializeField]
        private Color trackColor = new Color(0, 0, 0, 0);
        [SerializeField]
        private Color progressColor = new Color(1, 1, 1, 0.66f);
        [SerializeField]
        private Color percentageColor = Color.white;
        [SerializeField]
        private bool showLabel = false;

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
                    minWidth = minWidth,
                    minHeight = minHeight,
                    translate = new Translate(-100, -100, 0)
                }
            };

            // Set colors and label visibility
            m_RadialProgress.trackColor = trackColor;
            m_RadialProgress.progressColor = progressColor;
            m_RadialProgress.showLabel = showLabel;

            root.Add(m_RadialProgress);
            
            // Set label color after adding to hierarchy
            var label = m_RadialProgress.Q<Label>();
            if (label != null)
            {
                label.style.color = percentageColor;
            }
        }

        void Update()
        {
            m_RadialProgress.progress = ((Mathf.Sin(Time.time) + 1.0f) / 2.0f) * 60.0f + 10.0f;
        }
    }
}
