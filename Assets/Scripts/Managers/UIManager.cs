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

        root.Q<Button>("Generate").clicked += () => terrainGenerator.DisplayResult();
    }
}
