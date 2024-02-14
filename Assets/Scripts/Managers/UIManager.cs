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
            root.Q<Button>("Success").clicked += () =>
            {
                terrainGenerator.DisplayResult();
                root.Q<VisualElement>("Popup").style.opacity = 0;
            };
            root.Q<Button>("Save").clicked += () => saveManager.SaveFloatArray();
            root.Q<Button>("Load").clicked += () => 
            { 
                if (saveManager.LoadFloatArray())
                    root.Q<VisualElement>("Popup").style.opacity = 100;
            };

            var algorithmDropdown = root.Q<EnumField>("AlgorithmEnum");
            algorithmDropdown.RegisterValueChangedCallback(_ => terrainGenerator.SelectedAlgorithmTypeAsName = _.newValue.ToString());
            algorithmDropdown.value = (Enum)Enum.Parse(algorithmDropdown.value.GetType(), terrainGenerator.SelectedAlgorithmTypeAsName);

            var sizeDropdown = root.Q<EnumField>("SizeEnum");
            sizeDropdown.RegisterValueChangedCallback(_ => terrainGenerator.SelectedSizeAsNumber = Convert.ToInt32(_.newValue));
            sizeDropdown.value = (Enum)Enum.ToObject(sizeDropdown.value.GetType(), terrainGenerator.SelectedSizeAsNumber);

            var gradientDropdown = root.Q<EnumField>("GradientEnum");
            gradientDropdown.RegisterValueChangedCallback(_ => terrainGenerator.SelectedGradientTypeAsNumber = Convert.ToInt32(_.newValue));
            gradientDropdown.value = (Enum)Enum.ToObject(gradientDropdown.value.GetType(), terrainGenerator.SelectedGradientTypeAsNumber);
        }

        void OnDisable()
        {
            //? This is not working. Need to unsubscribe.
            //root.Q<Button>("Generate").clicked -= () => terrainGenerator.DisplayResult();
            //root.Q<Button>("Success").clicked -= () => terrainGenerator.DisplayResult();
            //root.Q<Button>("Save").clicked -= () => saveManager.SaveFloatArray();
            //root.Q<Button>("Load").clicked -= () => saveManager.LoadFloatArray();
        }
    }
}
