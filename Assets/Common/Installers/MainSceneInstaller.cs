using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public Liquid Liquid;
    public Cauldron Cauldron;

    public override void InstallBindings()
    {    
        BindLiquid();
        BindContainer();
    }

    private void BindLiquid()
    {
        Container
            .Bind<Liquid>()
            .FromInstance(Liquid)
            .AsSingle();
    }

    private void BindContainer()
    {
        Container
            .Bind<Cauldron>()
            .FromInstance(Cauldron)
            .AsSingle();

    }
}
