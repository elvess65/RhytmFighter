using System.Collections.Generic;
using RhytmFighter.Animation;
using UnityEngine;

namespace RhytmFighter.OtherScenes.MenuScene
{
    public class CharactersController : MonoBehaviour
    {
        [SerializeField] private List<AbstractAnimationController> Characters;

        public AbstractAnimationController SelectedCharacterAnimationController { get; private set; }

        public void Initialize()
        {
            foreach (AbstractAnimationController animationController in Characters)
                animationController.Initialize();
            
            SelectedCharacterAnimationController = Characters[0];
        }
    }
}
