using UnityEngine;

public class BurnEffect : StatusEffect
{
    private GameObject fireVisualInstance;

    public override void ApplyEffect(Asteroid target)
    {
        target.ApplyStatusEffectDamage((int)value);
        if (fireVisualInstance == null || !fireVisualInstance.activeInHierarchy)
        {
            fireVisualInstance = ObjectPool.Instance.GetFireEffectVisual();
            if (fireVisualInstance != null)
            {
                fireVisualInstance.SetActive(true);
                FireEffectVisual visualComponent = fireVisualInstance.GetComponent<FireEffectVisual>();
                if (visualComponent != null)
                {
                    visualComponent.Initialize(target, duration);
                }
            }
        }
    }

    public override void RemoveEffect(Asteroid target)
    {
        if (fireVisualInstance != null && fireVisualInstance.activeInHierarchy)
        {
            fireVisualInstance.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (fireVisualInstance != null && fireVisualInstance.activeInHierarchy)
        {
            fireVisualInstance.SetActive(false);
        }
    }
}
