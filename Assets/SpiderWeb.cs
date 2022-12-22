using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public GameObject Particles;

    public void StartParticles()
    {
        StartCoroutine(nameof(PlayParticles));
    }

    private IEnumerator PlayParticles()
    {
        Particles.SetActive(true);

        yield return new WaitForSeconds(4f);
        Particles.SetActive(false);
    }
}
