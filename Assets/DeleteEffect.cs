using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEffect : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(Dead(particle.main.duration));
    }

    IEnumerator Dead(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
