using SharedDefs;
using System;
using System.Collections.Generic;
using System.Linq;
using TerrainGeneration;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument document;
    [SerializeField]
    private TerrainGenerationManager terrainGenerator;

    void Start()
    {
        var root = document.rootVisualElement;

        var algorithmDropdown = root.Q<DropdownField>("AlgorithmDropdown");
        algorithmDropdown.choices = Enum.GetValues(typeof(AlgorithmType))
                                        .Cast<AlgorithmType>()
                                        .Select(v => v.ToString())
                                        .ToList();
        algorithmDropdown.value = algorithmDropdown.choices[0];

        var sizeDropdown = root.Q<DropdownField>("SizeDropdown");
        sizeDropdown.choices = Enum.GetValues(typeof(Size))
                                   .Cast<Size>()
                                   .Select(v => v.ToString())
                                   .ToList();
        sizeDropdown.value = sizeDropdown.choices[0];

        root.Q<Button>("Generate").clicked += () => terrainGenerator.DisplayResult();
    }
}
