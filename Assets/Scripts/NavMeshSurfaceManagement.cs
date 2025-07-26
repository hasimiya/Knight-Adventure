using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfaceManagement : MonoBehaviour
{
    // Singleton Instance
    public static NavMeshSurfaceManagement Instance { get; private set; }

    // Variables Components
    private NavMeshSurface _navMeshSurface;
    private void Awake()
    {
        Instance = this;
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.hideEditorLogs = true; // эта строка отключает логи в редакторе
    }

    // Public Methods
    public void RebakeNavMashSurface()
    {
        if (_navMeshSurface != null)
        {
            _navMeshSurface.BuildNavMesh(); // перестраивает NavMesh
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found on this GameObject.");
        }
    }
}
