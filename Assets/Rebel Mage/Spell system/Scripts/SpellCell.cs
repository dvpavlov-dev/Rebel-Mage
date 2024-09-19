using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rebel_Mage.Spell_system
{
    public class SpellCell : MonoBehaviour
    {
        public Image SpellImage;
        public Image BackgroundCooldown;

        private float fullTime;
        private float endTime;

        public void CooldownAnimation(float _startTime, float _endTime)
        {
            endTime = _endTime;
            fullTime = endTime - _startTime;

            StartCoroutine(nameof(CooldownAnim));
        }

        private IEnumerator CooldownAnim()
        {
            while (endTime - Time.time > 0)
            {
                float process = (endTime - Time.time) / fullTime;
                BackgroundCooldown.fillAmount = process;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
