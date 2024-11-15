using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public AudioClip[] audioClips; // Tableau pour stocker les fichiers audio
    private AudioSource audioSource;
    private int currentClipIndex = 0;
    public float baseTempo = 0.5f;
    public float minTempo = 0.15f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioClips.Length > 0)
        {
            StartCoroutine(PlayBackgroundSound());
        }
    }

    private IEnumerator PlayBackgroundSound()
    {
        while (true)
        {
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
            float adjustedTempo = baseTempo;
            if (Invaders.instance != null)
            {
                // Ajuster le tempo en fonction de la progression des ennemis tués
                adjustedTempo = Mathf.Max(minTempo, baseTempo * (1.0f - Invaders.instance.progressInvadersKilled));
                Debug.Log("Tempo: " + adjustedTempo);
            }
            yield return new WaitForSeconds(audioSource.clip.length + adjustedTempo); // Attendre la fin du clip + tempo ajusté
            currentClipIndex = (currentClipIndex + 1) % audioClips.Length; // Passer au clip suivant
        }
    }

}
