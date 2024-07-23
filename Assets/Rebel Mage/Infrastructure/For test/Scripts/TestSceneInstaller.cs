using UnityEngine.Serialization;
using Vanguard_Drone.Infrastructure;
using Zenject;

public class TestSceneInstaller : MonoInstaller
{
    [FormerlySerializedAs("_cameraManager")]
    public CameraManager m_CameraManager;

    public override void InstallBindings()
    {
        BindCameraManager();
    }
    
    private void BindCameraManager()
    {
        Container
            .Bind<CameraManager>()
            .FromInstance(m_CameraManager)
            .AsSingle();
    }
}
