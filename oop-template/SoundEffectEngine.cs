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
            _moveSound = new SoundPlayer("whoosh.wav");
            _mergeSound = new SoundPlayer("pop.wav");
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
