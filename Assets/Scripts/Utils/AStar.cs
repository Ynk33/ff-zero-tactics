using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class AStar
{
    static List<Vector3Int> DIRECTIONS = new List<Vector3Int> {
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.up,
        Vector3Int.down,
    };

    private GridManager gridManager;

    public AStar(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end, int maxDistance)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        var openSet = new PriorityQueue<Vector3Int>();
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var gScore = new Dictionary<Vector3Int, float>();
        var fScore = new Dictionary<Vector3Int, float>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;
        fScore[start] = gridManager.Heuristic(start, end);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == end)
            {
                // Path found, reconstruct path
                path.Clear();
                while (cameFrom.ContainsKey(current))
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse(); // Put the path in correct order

                return path; // Return the found path
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                // Skip if the neighbor is not walkable and not occupied by an object
                if (!gridManager.IsWalkable(neighbor) || gridManager.IsOccupied(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + gridManager.Heuristic(current, neighbor);

                // If the tentative score exceeds the max distance, skip this neighbor because it is too far
                if (tentativeGScore > maxDistance)
                    continue;

                // If the neighbor is not in the open set or has a better score, update it
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + gridManager.Heuristic(neighbor, end);
                    if (!openSet.Elements.Any(x => x.Item1 == neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }

        // No path found, return an empty path
        return path;
    }

    List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        var neighbors = new List<Vector3Int>();

        foreach (var dir in DIRECTIONS)
        {
            var neighbor = cell + dir;
            if (gridManager.Tilemap.HasTile(neighbor))
                neighbors.Add(neighbor);
        }

        return neighbors;
    }
}
