using Rebel_Mage.UI;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Player
{
    [RequireComponent(typeof(ZenAutoInjecter))]
    public class PlayerUIController : MonoBehaviour
    {
        private GameplayUI _gameplayUI;

        [Inject]
        private void Constructor(GameplayUI gameplayUI)
        {
            _gameplayUI = gameplayUI;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gameplayUI.Menu.SetActive(!_gameplayUI.Menu.activeSelf);
            }
        }
    }
}