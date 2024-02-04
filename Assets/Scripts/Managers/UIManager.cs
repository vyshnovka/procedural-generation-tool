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

    private VisualElement root;

    void OnEnable()
    {
        root = document.rootVisualElement;
        root.Q<Button>("Generate").clicked += () => terrainGenerator.DisplayResult();
    }

    void OnDisable()
    {
        root.Q<Button>("Generate").clicked -= () => terrainGenerator.DisplayResult();
    }
}
