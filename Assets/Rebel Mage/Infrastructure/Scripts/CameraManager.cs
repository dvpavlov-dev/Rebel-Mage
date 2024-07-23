using System.Collections.Generic;
using UnityEngine;

namespace Vanguard_Drone.Infrastructure 
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject CameraPlayer;
        public GameObject CameraEnvironment;

        private List<GameObject> _cameras;

        private void Awake()
        {
            _cameras = new List<GameObject>
            {
                CameraPlayer,
                CameraEnvironment
            };
        }
        
        public void SwitchCamera(TypeCamera typeCamera)
        {
            foreach (GameObject cam in _cameras)
            {
                cam.SetActive(false);
            }
            
            switch (typeCamera)
            {
                case TypeCamera.PLAYER_CAMERA:
                    try
                    {
                        CameraPlayer.SetActive(true);
                    }
                    catch
                    {
                        Debug.LogError("Камеры для игрока нет, установите ее в CameraManager");
                    }
                    break;
            
                case TypeCamera.ENVIRONMENT_CAMERA:
                    CameraEnvironment.SetActive(true);
                    break;
            }
        }
    }

    public enum TypeCamera
    {
        PLAYER_CAMERA,
        ENVIRONMENT_CAMERA,
    }
}