using Rebel_Mage.UI.Gameplay;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
public class PlayerUIController : MonoBehaviour
{
    private GameplayUI m_GameplayUI;

    [Inject]
    private void Constructor(GameplayUI gameplayUI)
    {
        m_GameplayUI = gameplayUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_GameplayUI.Menu.SetActive(!m_GameplayUI.Menu.activeSelf);
        }
    }
}
