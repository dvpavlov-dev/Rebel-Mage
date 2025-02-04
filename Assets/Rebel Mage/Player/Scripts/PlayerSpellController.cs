using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Player
{
    [RequireComponent(typeof(ZenAutoInjecter))]
    public class PlayerSpellController : MonoBehaviour
    {
        public Transform SpellPoint;
        public Animator AnimatorCastSpell;
    
        private Camera _camera;
        private Spells _spells;

        [Inject]
        private void Constructor(Spells spells, CameraManager cameraManager)
        {
            _spells = spells;
            _camera = cameraManager.CameraPlayer.GetComponent<Camera>();
        }

        void Update()
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float _))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) ||
                    Input.GetKeyDown(KeyCode.Mouse1) ||
                    Input.GetKeyDown(KeyCode.Q) ||
                    Input.GetKeyDown(KeyCode.E) ||
                    Input.GetKeyDown(KeyCode.R))
                {
                    if (Physics.Raycast(_camera.transform.position, ray.direction, out RaycastHit _, Mathf.Infinity))
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            UseSpell(TypeSpell.BASE_ATTACK);
                        }

                        if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            UseSpell(TypeSpell.SUPPORT_ATTACK);
                        }

                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            UseSpell(TypeSpell.FIRST_SPELL);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            UseSpell(TypeSpell.SECOND_SPELL);
                        }

                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            UseSpell(TypeSpell.THIRD_SPELL);
                        }
                    }
                }
            }
        }
    
        private void UseSpell(TypeSpell typeSpell)
        {
            _spells.CastSpell(typeSpell, AnimatorCastSpell, SpellPoint, gameObject);
        }
    }
}