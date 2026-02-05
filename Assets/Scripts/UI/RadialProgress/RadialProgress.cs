using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScrumbagsUI
{
    /// <summary>
    /// An element that displays progress inside a partially filled circle
    /// </summary>
    [UxmlElement]
    public partial class RadialProgress : VisualElement
    {
        // These are USS class names for the control overall and the label.
        public static readonly string ussClassName = "radial-progress";
        public static readonly string ussLabelClassName = "radial-progress__label";

        // These objects allow C# code to access custom USS properties.
        static CustomStyleProperty<Color> s_TrackColor = new CustomStyleProperty<Color>("--track-color");
        static CustomStyleProperty<Color> s_ProgressColor = new CustomStyleProperty<Color>("--progress-color");
        static CustomStyleProperty<bool> s_ShowLabel = new CustomStyleProperty<bool>("--show-label");

        // These are the meshes this control uses.
        EllipseMesh m_TrackMesh;
        EllipseMesh m_ProgressMesh;

        // This is the label that displays the percentage.
        Label m_Label;

        // This is the number of outer vertices to generate the circle.
        const int k_NumSteps = 200;

        // This is the number that the Label displays as a percentage.
        float m_Progress;

        // Flags to track if properties were set programmatically
        bool m_TrackColorSet = false;
        bool m_ProgressColorSet = false;
        bool m_ShowLabelSet = false;

        /// <summary>
        /// A value between 0 and 100
        /// </summary>
        [UxmlAttribute]
        public float progress
        {
            // The progress property is exposed in C#.
            get => m_Progress;
            set
            {
                // Whenever the progress property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                m_Progress = value;
                m_Label.text = Mathf.Clamp(Mathf.Round(value), 0, 100) + "%";
                MarkDirtyRepaint();
            }
        }

        /// <summary>
        /// The color of the track (background circle)
        /// </summary>
        public Color trackColor
        {
            get => m_TrackMesh.color;
            set
            {
                m_TrackColorSet = true;
                m_TrackMesh.color = value;
                MarkDirtyRepaint();
            }
        }

        /// <summary>
        /// The color of the progress (filled portion)
        /// </summary>
        public Color progressColor
        {
            get => m_ProgressMesh.color;
            set
            {
                m_ProgressColorSet = true;
                m_ProgressMesh.color = value;
                MarkDirtyRepaint();
            }
        }

        /// <summary>
        /// Whether to show the percentage label
        /// </summary>
        public bool showLabel
        {
            get => m_Label.style.display == DisplayStyle.Flex;
            set
            {
                m_ShowLabelSet = true;
                m_Label.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        // This default constructor is RadialProgress's only constructor.
        public RadialProgress()
        {
            // Create a Label, add a USS class name, and add it to this visual tree.
            m_Label = new Label();
            m_Label.AddToClassList(ussLabelClassName);
            Add(m_Label);

            // Create meshes for the track and the progress.
            m_ProgressMesh = new EllipseMesh(k_NumSteps);
            m_TrackMesh = new EllipseMesh(k_NumSteps);

            // Add the USS class name for the overall control.
            AddToClassList(ussClassName);

            // Register a callback after custom style resolution.
            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));

            // Register a callback to generate the visual content of the control.
            generateVisualContent += context => GenerateVisualContent(context);

            progress = 0.0f;
        }

        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            RadialProgress element = (RadialProgress)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        // After the custom colors are resolved, this method uses them to color the meshes and (if necessary) repaint
        // the control. Only apply USS values if properties haven't been set programmatically.
        void UpdateCustomStyles()
        {
            if (!m_ProgressColorSet && customStyle.TryGetValue(s_ProgressColor, out var progressColor))
            {
                m_ProgressMesh.color = progressColor;
            }

            if (!m_TrackColorSet && customStyle.TryGetValue(s_TrackColor, out var trackColor))
            {
                m_TrackMesh.color = trackColor;
            }

            if (!m_ShowLabelSet && customStyle.TryGetValue(s_ShowLabel, out var showLabel))
            {
                m_Label.style.display = showLabel ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (m_ProgressMesh.isDirty || m_TrackMesh.isDirty)
                MarkDirtyRepaint();
        }

        // The GenerateVisualContent() callback method calls DrawMeshes().
        static void GenerateVisualContent(MeshGenerationContext context)
        {
            RadialProgress element = (RadialProgress)context.visualElement;
            element.DrawMeshes(context);
        }

        // DrawMeshes() uses the EllipseMesh utility class to generate an array of vertices and indices, for both the
        // "track" ring (in grey) and the progress ring (in green). It then passes the geometry to the MeshWriteData
        // object, as returned by the MeshGenerationContext.Allocate() method. For the "progress", only a slice of
        // the index arrays is used to progressively reveal parts of the mesh.
        void DrawMeshes(MeshGenerationContext context)
        {
            float halfWidth = contentRect.width * 0.5f;
            float halfHeight = contentRect.height * 0.5f;

            if (halfWidth < 2.0f || halfHeight < 2.0f)
                return;

            m_ProgressMesh.width = halfWidth;
            m_ProgressMesh.height = halfHeight;
            m_ProgressMesh.borderSize = 10;
            m_ProgressMesh.UpdateMesh();

            m_TrackMesh.width = halfWidth;
            m_TrackMesh.height = halfHeight;
            m_TrackMesh.borderSize = 10;
            m_TrackMesh.UpdateMesh();

            // Draw track mesh first
            var trackMeshWriteData = context.Allocate(m_TrackMesh.vertices.Length, m_TrackMesh.indices.Length);
            trackMeshWriteData.SetAllVertices(m_TrackMesh.vertices);
            trackMeshWriteData.SetAllIndices(m_TrackMesh.indices);

            // Keep progress between 0 and 100
            float clampedProgress = Mathf.Clamp(m_Progress, 0.0f, 100.0f);

            // Determine how many triangle are used to depending on progress, to achieve a partially filled circle
            int sliceSize = Mathf.FloorToInt((k_NumSteps * clampedProgress) / 100.0f);

            if (sliceSize == 0)
                return;

            // Every step is 6 indices in the corresponding array
            sliceSize *= 6;

            var progressMeshWriteData = context.Allocate(m_ProgressMesh.vertices.Length, sliceSize);
            progressMeshWriteData.SetAllVertices(m_ProgressMesh.vertices);

            var tempIndicesArray = new NativeArray<ushort>(m_ProgressMesh.indices, Allocator.Temp);
            progressMeshWriteData.SetAllIndices(tempIndicesArray.Slice(0, sliceSize));
            tempIndicesArray.Dispose();
        }

    }
}
