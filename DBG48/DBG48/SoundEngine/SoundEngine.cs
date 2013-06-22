using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


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

        public void PlaySoundEffect(string key)
        {
            if(SFX_Dictionary.ContainsKey(key))
            {
                SFX_Dictionary[key].Play();
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
