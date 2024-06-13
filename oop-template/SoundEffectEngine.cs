using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace oop_template
{
    public class SoundEffectEngine
    {
        private readonly SoundPlayer _moveSound;
        private readonly SoundPlayer _mergeSound;

        public SoundEffectEngine()
        {
            _moveSound = new SoundPlayer("C:\\Users\\Korisnik\\2048\\oop-template\\whoosh.wav");
            _mergeSound = new SoundPlayer("C:\\Users\\Korisnik\\2048\\oop-template\\pop.wav");
        }

        public void PlayMoveSound()
        {
            _moveSound.Play();
        }

        public void PlayMergeSound()
        {
            _mergeSound.Play();
        }
    }
}
