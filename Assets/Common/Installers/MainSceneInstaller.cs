using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public PickUpInteraction PickUpInteraction;
    public PotionRecipesDataBase PotionRecipesDataBase;


    public override void InstallBindings()
    {
        BindPickUpInteraction();
        BindPotionRecipesDataBase();
    }

    private void BindPickUpInteraction()
    {
        Container
            .Bind<PickUpInteraction>()
            .FromInstance(PickUpInteraction)
            .AsSingle();
    }
    private void BindPotionRecipesDataBase()
    {
        Container
            .Bind<PotionRecipesDataBase>()
            .FromInstance(PotionRecipesDataBase)
            .AsSingle();
    }
}
