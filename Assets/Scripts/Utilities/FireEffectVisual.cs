using UnityEngine;

public class FireEffectVisual : MonoBehaviour
{
    public ParticleSystem fireParticleSystem;
    private float duration;
    private Asteroid targetAsteroid;
    public void Initialize(Asteroid target, float effectDuration)
    {
        targetAsteroid = target;
        duration = effectDuration;

       
        transform.SetParent(target.transform);
        transform.localPosition = Vector3.zero;

        if (fireParticleSystem != null)
        {
           
            fireParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = fireParticleSystem.main;
            main.duration = effectDuration;

            fireParticleSystem.Play();
        }

        StartCoroutine(DeactivateAfterDuration());
    }

   
    private System.Collections.IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (fireParticleSystem != null)
        {
            fireParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            yield return new WaitForSeconds(fireParticleSystem.main.startLifetime.constantMax);
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (targetAsteroid == null || !targetAsteroid.gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            if (fireParticleSystem != null)
            {
                fireParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            gameObject.SetActive(false);
        }
    }
}