using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using static FastNoiseLite;

namespace Components.ProceduralGeneration._2_CellularAutomata
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Noise Generator")]
    public class NoiseGenerator : ProceduralGenerationMethod
    {
        [SerializeField] private List<List<int>> StepList;
        private int Seed;


        [Header("Noise Parameters")]
        [SerializeField] private FastNoiseLite.NoiseType _noiseType;
        [SerializeField, Tooltip(""), Range(0.01f, 1f)] private float _frequency = 0.01f;
        [SerializeField, Tooltip(""), Range(0.5f, 1.5f)] private float _amplitude = 1.0f;

        [Header("Fractal Parameters")]
        [SerializeField] private FastNoiseLite.FractalType _fractalType;
        [SerializeField, Tooltip(""), Range(1, 5)] private int _octaves = 3;
        [SerializeField, Tooltip(""), Range(1f, 3f)] private float _lacunarity = 2.0f;
        [SerializeField, Tooltip(""), Range(0.5f, 1f)] private float _persistence = 0.5f;


        [Header("Heights")]
        [SerializeField, Tooltip("The percentage of water tile on the map."), Range(-1, 1)] private float WaterHeight = -0.6f;
        [SerializeField, Tooltip("The percentage of sand tile on the map."), Range(-1, 1)] private float SandHeight = -0.3f;
        [SerializeField, Tooltip("The percentage of grass tile on the map."), Range(-1, 1)] private float GrassHeight = 0.8f;
        [SerializeField, Tooltip("The percentage of rock tile on the map."), Range(-1, 1)] private float RockHeight = 1f;

        [Header("Debug")]
        [SerializeField] private bool _drawDebugTexture;
        [SerializeField] private float _DebugTextureAlpha = 0.6f;


        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            Seed = RandomService.Seed;
            StepList = new List<List<int>>();
            FastNoiseLite Noise = CreateNoise(Seed);
            SetUpNoise(Noise);
            DrawNoise(Noise);

        }

        FastNoiseLite CreateNoise(int seed)
        {
            FastNoiseLite noise = new FastNoiseLite(seed);
            return noise;
        }

        void SetUpNoise(FastNoiseLite noise)
        {
            noise.SetNoiseType(_noiseType);
            noise.SetFrequency(_frequency);
            noise.SetFractalType(_fractalType);
            noise.SetFractalGain(_persistence);
            noise.SetFractalOctaves(_octaves);
            noise.SetFractalLacunarity(_lacunarity);
            noise.SetDomainWarpAmp(_amplitude);
        }

        void DrawNoise(FastNoiseLite noise)
        {
            for (int i = 0; i < Grid.Width; i++)
            {
                for (int j = 0; j < Grid.Lenght; j++)
                {
                    if (!Grid.TryGetCellByCoordinates(i, j, out Cell cell))
                        continue;
                    if (noise.GetNoise(i, j) < WaterHeight)
                    {
                        AddTileToCell(cell, "Water", true);
                    }
                    else if (noise.GetNoise(i, j) >= WaterHeight && noise.GetNoise(i, j) < SandHeight)
                    {
                        AddTileToCell(cell, "Sand", true);
                    }
                    else if (noise.GetNoise(i, j) >= SandHeight && noise.GetNoise(i, j) < GrassHeight)
                    {
                        AddTileToCell(cell, "Grass", true);
                    }
                    else if (noise.GetNoise(i, j) >= RockHeight)
                    {
                        AddTileToCell(cell, "Rock", true);
                    }
                }
            }
        }


    }
}