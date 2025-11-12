using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Procedural Generation Method/TON NOM D'ALGO")]

public class MyAlgo : ProceduralGenerationMethod
{
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        // Declare variables here
        // ........
        int nbRoom = 0;
        for (int i = 0; i < _maxSteps; i++)
        {

            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();

            // Your algorithm here
            // .......







            // Waiting between steps to see the result.
            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }



    }
}



