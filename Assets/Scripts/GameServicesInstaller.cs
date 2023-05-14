using UnityEngine;
using Zenject;

public class GameServicesInstaller : MonoInstaller
{
    public GameObject CrystalPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<Crystal, CrystalFactory>().FromComponentInNewPrefab(CrystalPrefab);
    }
}