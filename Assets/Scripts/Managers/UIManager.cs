using SharedDefs;
using System;
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

            var algorithmDropdown = root.Q<EnumField>("AlgorithmEnum");
            algorithmDropdown.RegisterValueChangedCallback(_ => terrainGenerator.SelectedAlgorithmType = _.newValue.ToString());
            algorithmDropdown.value = (AlgorithmType)Enum.Parse(typeof(AlgorithmType), terrainGenerator.SelectedAlgorithmType); //? Looks too complex. Is there a way to change it?

            var sizeDropdown = root.Q<EnumField>("SizeEnum");
            sizeDropdown.RegisterValueChangedCallback(evt => terrainGenerator.SelectedSize = (int)(Size)evt.newValue);
            sizeDropdown.value = (Size)terrainGenerator.SelectedSize; //? using SharedDefs; is bad. Needs to be more generic and robust.
        }

        void OnDisable()
        {
            root.Q<Button>("Generate").clicked -= () => terrainGenerator.DisplayResult();
            root.Q<Button>("Save").clicked -= () => saveManager.SaveFloatArray();
            root.Q<Button>("Load").clicked -= () => saveManager.LoadFloatArray();
        }
    }
}
