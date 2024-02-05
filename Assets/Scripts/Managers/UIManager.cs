using TerrainGeneration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private UIDocument document;
        [SerializeField]
        private TerrainGenerationManager terrainGenerator;
        [SerializeField]
        private SaveManager saveManager;

        private VisualElement root;

        void OnEnable()
        {
            root = document.rootVisualElement;
            root.Q<Button>("Generate").clicked += () => terrainGenerator.DisplayResult();
            root.Q<Button>("Save").clicked += () => saveManager.SaveFloatArray();
            root.Q<Button>("Load").clicked += () => saveManager.LoadFloatArray();
        }

        void OnDisable()
        {
            root.Q<Button>("Generate").clicked -= () => terrainGenerator.DisplayResult();
            root.Q<Button>("Save").clicked -= () => saveManager.SaveFloatArray();
            root.Q<Button>("Load").clicked -= () => saveManager.LoadFloatArray();
        }
    }
}
