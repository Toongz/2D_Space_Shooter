using System.Collections;
using UnityEngine;
public abstract class StatusEffect : MonoBehaviour
{
    public float duration;
    public float tickRate;
    public float value;
    private float nextTickTime;
    private bool isActive = false;
    public virtual void ApplyEffect(Asteroid target) { }
    public virtual void RemoveEffect(Asteroid target) { }

    public void Initialize(float duration, float tickRate, float value)
    {
        this.duration = duration;
        this.tickRate = tickRate;
        this.value = value;
    }
    public void StartEffect(Asteroid target)
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(EffectCoroutine(target));
        }
    }

    protected IEnumerator EffectCoroutine(Asteroid target)
    {
        float endTime = Time.time + duration;
        nextTickTime = Time.time;

        while (Time.time < endTime && target != null && target.gameObject.activeInHierarchy)
        {
            if (Time.time >= nextTickTime)
            {
                ApplyEffect(target);
                nextTickTime = Time.time + tickRate;
            }
            yield return null;
        }

        if (target != null && target.gameObject.activeInHierarchy)
        {
            RemoveEffect(target);
        }
        Destroy(this);
    }
}