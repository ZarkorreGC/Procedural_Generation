using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Components.ProceduralGeneration;

using Cysharp.Threading.Tasks;
using System.Threading;
using VTools.RandomService;



[CreateAssetMenu(menuName = "Procedural Generation Method/BSP")]
public class BSPAlgo : ProceduralGenerationMethod
{
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        Debug.Log("test algo");
        var allGrid = new RectInt(0, 0, Grid.Width, Grid.Lenght);
        var root = new BSPNode(allGrid, RandomService);
        AddChild(root);
        Debug.Log("root child 1 bounds = " + root._child1._bounds);
        Debug.Log("root child 2 bounds = " + root._child2._bounds);
        AddChild(root._child1);
        Debug.Log("C1 child 1 bounds = " + root._child1._child1._bounds);
        Debug.Log("C1 child 2 bounds = " + root._child1._child2._bounds);
        AddChild(root._child2);
        Debug.Log("C2 child 1 bounds = " + root._child2._child1._bounds);
        Debug.Log("C2 child 2 bounds = " + root._child2._child2._bounds);
    }





    void AddChild(BSPNode node)
    {
        Vector2Int first = new Vector2Int(RandomService.Range(node._bounds.width / 2, RandomService.Range(-5,5)), RandomService.Range(node._bounds.height / 2, RandomService.Range(-5, 5)));
        Vector2Int second = new Vector2Int(node._bounds.width - first.x, node._bounds.height - first.y);

        int RandomCut1 = RandomService.Range(0, 1);
        int RandomCut2 = RandomService.Range(0, 1);

        if (RandomCut1 == 0)
        {
            BSPNode child1 = new BSPNode(HorizontalCutGrid(first.x, first.y, 0), RandomService);
            node._child1 = child1;
        }
        else
        {
            BSPNode child1 = new BSPNode(VerticalCutGrid(first.x, first.y, 0), RandomService);
            node._child1 = child1;
        }

        if (RandomCut2 == 0)
        {
            BSPNode child2 = new BSPNode(HorizontalCutGrid(second.x, second.y, first.y), RandomService);
            node._child2 = child2;
        }
        else
        {
            BSPNode child2 = new BSPNode(VerticalCutGrid(second.x, second.y, first.x), RandomService);
            node._child2 = child2;
        }
    }

    RectInt HorizontalCutGrid(int ChildWidth, int ChildLength, int YStartPos)
    {
        RectInt NewChildGrid = new RectInt(0, YStartPos, ChildWidth, ChildLength);
        return NewChildGrid;
    }

    RectInt VerticalCutGrid(int ChildWidth, int ChildLength, int XStartPos)
    {
        RectInt NewChildGrid = new RectInt(XStartPos, 0, ChildWidth, ChildLength);
        return NewChildGrid;
    }

}

public class BSPNode
{
    private RandomService _randomService;
    public RectInt _bounds;
    public BSPNode _child1, _child2;

    public BSPNode(RectInt bounds, RandomService randomService)
    {
        _bounds = bounds;
        _randomService = randomService;
        
        if(false)
        {
            _child1 = new BSPNode(_bounds, _randomService);
            _child2 = new BSPNode(_bounds, _randomService);
        }

        

    }
}