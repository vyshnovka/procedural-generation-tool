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
            algorithmDropdown.RegisterValueChangedCallback(_ => terrainGenerator.SelectedAlgorithmTypeAsName = _.newValue.ToString());
            algorithmDropdown.value = (Enum)Enum.Parse(algorithmDropdown.value.GetType(), terrainGenerator.SelectedAlgorithmTypeAsName);

            var sizeDropdown = root.Q<EnumField>("SizeEnum");
            sizeDropdown.RegisterValueChangedCallback(evt => terrainGenerator.SelectedSizeAsNumber = Convert.ToInt32(evt.newValue));
            sizeDropdown.value = (Enum)Enum.ToObject(sizeDropdown.value.GetType(), terrainGenerator.SelectedSizeAsNumber);

            //TODO Logic for gradient radio buttons.
        }

        void OnDisable()
        {
            root.Q<Button>("Generate").clicked -= () => terrainGenerator.DisplayResult();
            root.Q<Button>("Save").clicked -= () => saveManager.SaveFloatArray();
            root.Q<Button>("Load").clicked -= () => saveManager.LoadFloatArray();
        }
    }
}
