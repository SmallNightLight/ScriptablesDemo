using UnityEngine;

public class GameManager : MonoBehaviour
{
    ISetupManager[] _setupManagers;
    IUpdateManager[] _updateManagers;

    private void Awake()
    {
        _setupManagers = GetComponentsInChildren<ISetupManager>();
        _updateManagers = GetComponentsInChildren<IUpdateManager>();

        foreach (ISetupManager setupManager in _setupManagers)
            setupManager.Setup();
    }

    private void Update()
    {
        foreach (IUpdateManager updateManager in _updateManagers)
            updateManager.Update();
    }
}

public interface ISetupManager
{
    public void Setup(); //only the interfaces attachted to this GameObject are being called
}

public interface IUpdateManager
{
    public void Update(); //only the interfaces attachted to this GameObject are being called
}