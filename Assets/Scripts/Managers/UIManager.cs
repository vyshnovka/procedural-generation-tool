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
        private TerrainGenerationManager terrainManager;
        [SerializeField]
        private SaveManager saveManager;

        private VisualElement root;

        void OnEnable()
        {
            TerrainGenerationManager.TerrainSizeChanged += UpdateValueOfSizeDropdown;

            root = document.rootVisualElement;
            root.Q<Button>("Generate").clicked += () => terrainManager.DisplayResult();
            root.Q<Button>("Success").clicked += () =>
            {
                terrainManager.DisplayResult();
                root.Q<VisualElement>("Popup").style.opacity = 0;
            };
            root.Q<Button>("Save").clicked += () => saveManager.SaveFloatArray();
            root.Q<Button>("Load").clicked += () => 
            { 
                saveManager.LoadFloatArray(success => {
                    if (success)
                        root.Q<VisualElement>("Popup").style.opacity = 100;
                });
            };

            var algorithmDropdown = root.Q<EnumField>("AlgorithmEnum");
            algorithmDropdown.RegisterValueChangedCallback(_ => terrainManager.SelectedAlgorithmTypeAsName = _.newValue.ToString());
            algorithmDropdown.value = (Enum)Enum.Parse(algorithmDropdown.value.GetType(), terrainManager.SelectedAlgorithmTypeAsName);

            var sizeDropdown = root.Q<EnumField>("SizeEnum");
            sizeDropdown.RegisterValueChangedCallback(_ => terrainManager.SelectedSizeAsNumber = Convert.ToInt32(_.newValue));
            sizeDropdown.value = (Enum)Enum.ToObject(sizeDropdown.value.GetType(), terrainManager.SelectedSizeAsNumber);

            var gradientDropdown = root.Q<EnumField>("GradientEnum");
            gradientDropdown.RegisterValueChangedCallback(_ => terrainManager.SelectedColorSchemeAsNumber = Convert.ToInt32(_.newValue));
            gradientDropdown.value = (Enum)Enum.ToObject(gradientDropdown.value.GetType(), terrainManager.SelectedColorSchemeAsNumber);
        }

        void OnDisable()
        {
            TerrainGenerationManager.TerrainSizeChanged -= UpdateValueOfSizeDropdown;
        }

        private void UpdateValueOfSizeDropdown(int value)
        {
            var sizeDropdown = root.Q<EnumField>("SizeEnum");
            sizeDropdown.value = (Enum)Enum.ToObject(sizeDropdown.value.GetType(), value);
        }
    }
}
