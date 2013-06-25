using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace DBG48
{
    public class SoundEngine
    {
        private static SoundEngine instance;

        private static Dictionary<string, SoundEffect> SFX_Dictionary;
        
        private SoundEngine() 
        {
            SFX_Dictionary = new Dictionary<string, SoundEffect>();
        }

        public void RegisterSoundEffect(string key, SoundEffect sfx)
        {
            SFX_Dictionary[key] = sfx;
        }

        public void PlaySoundEffect(string key, float volume = 1.0f)
        {
            if(SFX_Dictionary.ContainsKey(key))
            {
                SFX_Dictionary[key].Play(volume, 0.0f, 0.0f);
            }
        }

        public static SoundEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundEngine();
                }
                return instance;
            }
        }
    }
}
