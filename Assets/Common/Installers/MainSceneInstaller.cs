using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public PickUpInteraction PickUpInteraction;

    public override void InstallBindings()
    {
        BindPickUpInteraction();
    }

    private void BindPickUpInteraction()
    {
        Container
            .Bind<PickUpInteraction>()
            .FromInstance(PickUpInteraction)
            .AsSingle();
    }
}
