using UnityEngine;

namespace RhytmFighter.Enviroment.Effects
{
    public class ParticlesVisualEffect : SimpleVisualEffect
    {
        public ParticleSystem[] Particles;

        public void ShowEffect()
        {
            for (int i = 0; i < Particles.Length; i++)
                Particles[i].gameObject.SetActive(true);
        }

        public void HideEffect()
        {
            for (int i = 0; i < Particles.Length; i++)
                Particles[i].gameObject.SetActive(false);
        }
    }
}
