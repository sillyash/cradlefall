using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Image damageSprite;

    public float alphaTarget { get; set; }  = 0f;
    
    private Color color;
    private const float RESET_TOLERANCE = 0.01f;

    // Update is called once per frame
    void Update()
    {
        color = damageSprite.color;
        color.a = Mathf.Lerp(damageSprite.color.a, alphaTarget, 0.1f);

        if(alphaTarget != 0 && color.a >= alphaTarget - RESET_TOLERANCE)
        {
            alphaTarget = 0f;
        }

        damageSprite.color = color;
    }
}
