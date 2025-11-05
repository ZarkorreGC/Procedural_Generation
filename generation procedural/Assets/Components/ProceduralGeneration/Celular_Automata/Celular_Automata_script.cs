using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;


[CreateAssetMenu(menuName = "Procedural Generation Method/Celular_Automata")]

public class Celular_Automata_script : ProceduralGenerationMethod
{
    [SerializeField] public List<int> noise;
    [SerializeField] public int noiseIntensity;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        // Declare variables here
        // ........
        //Debug.Log("test algo");
        noise = new List<int>();

        

        GenerateNoise(noiseIntensity);
        BuildNoise();


        for (int i = 0; i < _maxSteps; i++)
        {
            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();

            // Your algorithm here
            // .......


            // Waiting between steps to see the result.
            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }


        // Final ground building.
        //BuildGround();
    }



    private void GenerateNoise(int noiseIntensity)
    {
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                if (noiseIntensity == 100)
                {
                    noise.Add(1);
                    
                }
                else
                {

                    int stepvalue = RandomService.Range(0, 100);

                    if (stepvalue < noiseIntensity)
                    {
                        noise.Add(1);
                    }
                    else
                    {
                        noise.Add(0);
                    }
                }
            }
        }
    }


    private void BuildNoise()
    {
        var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Room");
        int i = 0;

        // Instantiate ground blocks
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                switch(noise[i]){

                    case 1:
                        if (!Grid.TryGetCellByCoordinates(x, z, out Cell cell))
                            continue;
                        AddTileToCell(cell, GRASS_TILE_NAME, true);

                        break;

                    case 0:
                        if (!Grid.TryGetCellByCoordinates(x, z, out Cell cell0))
                            continue;
                        AddTileToCell(cell0, WATER_TILE_NAME, true);

                        break;

                }
                i++;
            }
        }
    }

    private void BuildGround()
    {
        var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");

        // Instantiate ground blocks
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                {
                    UnityEngine.Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                    continue;
                }

                GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
            }
        }
    }
}
