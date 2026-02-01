using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource; // <--- ¡Asegúrese de asignar esto en el Inspector!

    [System.Serializable]
    public class Sound
    {
        public string nombre;
        public AudioClip clip;
        [Range(0f, 1f)] public float volumen = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
    }

    [Header("Librería de Audio")]
    public List<Sound> listaSonidos; // SFX y Música mezclados aquí

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sobrevive al cambio de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string nombreSonido)
    {
        Sound s = listaSonidos.Find(sonido => sonido.nombre == nombreSonido);
        if (s != null && sfxSource != null)
        {
            sfxSource.pitch = s.pitch;
            sfxSource.PlayOneShot(s.clip, s.volumen);
        }
    }

    // --- NUEVO SISTEMA DE MÚSICA CON FADE ---
    public void PlayMusic(string nombreMusica, float duracionFade = 1.5f)
    {
        Sound s = listaSonidos.Find(sonido => sonido.nombre == nombreMusica);

        if (s == null)
        {
            Debug.LogWarning("Música no encontrada: " + nombreMusica);
            return;
        }

        // 1. Si ya está sonando esa canción, NO HACEMOS NADA (Para que no se corte al cambiar escena)
        if (musicSource.clip == s.clip && musicSource.isPlaying)
            return;

        // 2. Si hay un fade ocurriendo, lo paramos para empezar el nuevo
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        // 3. Iniciamos la transición
        fadeCoroutine = StartCoroutine(CrossfadeMusic(s.clip, s.volumen, duracionFade));
    }

    IEnumerator CrossfadeMusic(AudioClip nuevoClip, float volumenObjetivo, float duracion)
    {
        float timer = 0f;
        float startVolume = musicSource.volume;

        // FADE OUT (Si hay algo sonando)
        if (musicSource.isPlaying)
        {
            while (timer < duracion / 2)
            {
                timer += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0, timer / (duracion / 2));
                yield return null;
            }
        }

        musicSource.Stop();
        musicSource.clip = nuevoClip;
        musicSource.Play();
        
        // FADE IN
        timer = 0f;
        while (timer < duracion / 2)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, volumenObjetivo, timer / (duracion / 2));
            yield return null;
        }

        musicSource.volume = volumenObjetivo;
    }
}