using Rebel_Mage.UI.Gameplay;
using Rebel_Mage.UI.Spell_Window;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class Game : MonoBehaviour
    {
        private GameStateMachine m_StateMachine;

        private IRoundProcess m_RoundProcess;
        private SpellWindowController m_SpellWindowController;
        private GameplayUI m_GameplayUI;

        [Inject]
        private void Constructor(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI)
        {
            m_RoundProcess = roundProcess;
            m_SpellWindowController = spellWindowController;
            m_GameplayUI = gameplayUI;
        }

        private void Start()
        {
            m_StateMachine = new GameStateMachine(m_RoundProcess, m_SpellWindowController, m_GameplayUI);
        }
    }
}
