using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _maxRooms = 10;

        [Header("Room Size")]
        [SerializeField] public Vector2Int sizeMin;
        [SerializeField] public Vector2Int sizeMax;

        private List<RectInt> rooms = new List<RectInt>();


        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            // Declare variables here
            // ........
            int nbRoom = 0;
            for (int i = 0; i < _maxSteps; i++)
            {

                if(nbRoom>=_maxRooms)
                    break;


                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                // Your algorithm here
                // .......


                int x = RandomService.Range(0, Grid.Width);
                int y = RandomService.Range(0, Grid.Lenght);

                int xSize = RandomService.Range(sizeMin.x, sizeMax.x);
                int ySize = RandomService.Range(sizeMin.y, sizeMax.y);

                RectInt room = new RectInt(x, y, xSize, ySize);

                if (CanPlaceRoom(room))
                {
                    rooms.Add(room);
                    PlaceRoom(room);
                    nbRoom++;
                }



                // Waiting between steps to see the result.
                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken : cancellationToken);
            }

            PlaceCorridor(rooms);

            // Final ground building.
            BuildGround();
        }




        private void PlaceRoom(RectInt room)
        {
            for (int ix = room.xMin; ix < room.xMax; ix++)
            {
                for (int iy = room.yMin; iy < room.yMax; iy++)
                {
                    if (!Grid.TryGetCellByCoordinates(ix, iy, out Cell cell))
                        continue;
                    AddTileToCell(cell, ROOM_TILE_NAME, true);

                }
            }
        }

        private bool CanPlaceRoom(RectInt room)
        {
            for (int ix = room.xMin; ix < room.xMax; ix++)
            {
                for (int iy = room.yMin; iy < room.yMax; iy++)
                {
                    if (!Grid.TryGetCellByCoordinates(ix, iy, out Cell cell))
                        continue;
                    if(cell.ContainObject)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PlaceCorridor(List<RectInt> rooms)
        {

            for (int i = 0; i < rooms.Count - 1; i++) 
            {
                RectInt room1 = rooms[i];     
                RectInt room2 = rooms[i + 1]; 

                int ixRoom1 = room1.x + room1.xMax % 2;
                int iyRoom1 = room1.y + room1.yMax % 2;

                int ixRoom2 = room2.x + room2.xMax % 2;
                int iyRoom2 = room2.y + room2.yMax % 2;

                for (int ix = ixRoom1; ix != ixRoom2; )
                {

                    if (!Grid.TryGetCellByCoordinates(ix, iyRoom1, out Cell cell))
                        continue;
                    AddTileToCell(cell, CORRIDOR_TILE_NAME, true);

                    if (ix < ixRoom2)
                        ix++;
                    if (ix > ixRoom2)
                        ix--;
                }

                for (int iy = iyRoom1; iy != iyRoom2; )
                {
                    if (!Grid.TryGetCellByCoordinates(ixRoom2, iy, out Cell cell))
                        continue;
                    AddTileToCell(cell, CORRIDOR_TILE_NAME, true);

                    if (iy < iyRoom2)
                        iy++;
                    if (iy > iyRoom2)
                        iy--;
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
                        Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                        continue;
                    }
                    
                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }
    }
}