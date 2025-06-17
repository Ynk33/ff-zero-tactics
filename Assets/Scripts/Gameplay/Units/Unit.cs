using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Unit : TileObject
{
    public delegate void MoveCallback();

    [SerializeField]
    bool canMove = true;

    [SerializeField, ShowIf("canMove")]
    int maxDistance = 5;

    [SerializeField]
    List<Action> actions = default;

    public int MaxDistance => canMove ? maxDistance : 0;
    public List<Action> Actions => actions;

    public float GetCost(Vector3Int from, Vector3Int to)
    {
        // Increase cost if move is forward
        float forwardCost = to.x - from.x;
        if (forwardCost > 0) forwardCost *= 2;

        return Mathf.Abs(forwardCost) + Mathf.Abs(from.y - to.y) + Mathf.Abs(from.z - to.z);
    }

    public void ShowMovementRange()
    {
        HideMovementRange();
        GridManager.HighlightCells(GridManager.GetCellsWithinRange(Position, maxDistance, GetCost));
    }

    public void HideMovementRange()
    {
        GridManager.RemoveHighlights();
        GridManager.RemovePath();
    }

    public List<Vector3Int> FindPathTo(Vector3 position)
    {
        return GridManager.FindPath(transform.position, position, maxDistance, GetCost);
    }

    public void MoveTo(Vector3 position, float movementDuration, MoveCallback onMoveComplete)
    {
        List<Vector3Int> path = FindPathTo(position);
        if (path.Count > 0)
        {
            // Move the unit along the path
            Sequence moveSequence = DOTween.Sequence();
            foreach (Vector3Int cell in path)
            {
                Vector3 worldPosition = GridManager.GetCellCenterWorldPosition(cell);
                moveSequence.Append(transform.DOMove(worldPosition, movementDuration).SetEase(Ease.Linear));
            }
            moveSequence.OnComplete(() =>
            {
                UpdateTile();
                onMoveComplete();
            });
        }
        else
        {
            onMoveComplete();
        }
    }
}
