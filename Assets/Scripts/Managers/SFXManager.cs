using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

    [System.Serializable]
    public class SFX
    {
                      public string name;
                      public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;
                      public AudioMixerGroup mixerGroup;
    }

    public class SFXManager : Singleton<SFXManager>
    {

        public List<SFX> sounds;

        public GameObject sfxPrefab;

        private List<GameObject> sfxHandles = new List<GameObject>();

 

        public void PlaySound(SFX snd)
        {

            if (snd == null) return;


            GameObject obj = Instantiate(sfxPrefab, this.transform);

            sfxHandles.Add(obj);
            AudioSource source = obj.GetComponent<AudioSource>();

            source.clip = snd.clip;
            source.volume = snd.volume;
            source.outputAudioMixerGroup = snd.mixerGroup;

            source.Play();


            return;  // muted

            StartCoroutine(DestroyAfterTime(obj, source.clip.length));

        }
        
        public void PlaySound(string nm)
        { 
            PlaySound(GetSFXByName(nm));
        }

        IEnumerator DestroyAfterTime(GameObject obj, float time)
        {

            yield return new WaitForSecondsRealtime(time);

            sfxHandles.Remove(obj); 
            Destroy(obj);

            yield return null;
        }

        public SFX GetSFXByName(string soundName)
        {
            SFX snd = null;

            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].name.Equals(soundName))
                {
                    try
                    {
                        snd = sounds[i];
                    } catch { };
                    }
            }  
            return snd;
        }
    }