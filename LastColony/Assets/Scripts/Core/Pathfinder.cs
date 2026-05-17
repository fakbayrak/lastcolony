using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private class Node
    {
        public Vector2Int position;
        public Node parent;
        public int gCost;
        public int hCost;
        public int FCost => gCost + hCost;
    }

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        var openList = new List<Node>();
        var closedSet = new HashSet<Vector2Int>();

        openList.Add(new Node { position = start, gCost = 0, hCost = Manhattan(start, end) });

        while (openList.Count > 0)
        {
            Node current = GetLowestFCost(openList);

            if (current.position == end)
                return ReconstructPath(current);

            openList.Remove(current);
            closedSet.Add(current.position);

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = current.position + dir;

                if (closedSet.Contains(neighborPos))
                    continue;

                // IsCellOccupied returns true for out-of-bounds, handling both checks at once
                bool walkable = !GridManager.Instance.IsCellOccupied(neighborPos.x, neighborPos.y);

                if (!walkable && neighborPos != end)
                    continue;

                int newGCost = current.gCost + 1;
                Node existing = openList.Find(n => n.position == neighborPos);

                if (existing == null)
                {
                    openList.Add(new Node
                    {
                        position = neighborPos,
                        parent = current,
                        gCost = newGCost,
                        hCost = Manhattan(neighborPos, end)
                    });
                }
                else if (newGCost < existing.gCost)
                {
                    existing.gCost = newGCost;
                    existing.parent = current;
                }
            }
        }

        return null;
    }

    private static Node GetLowestFCost(List<Node> nodes)
    {
        Node best = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FCost < best.FCost ||
                (nodes[i].FCost == best.FCost && nodes[i].hCost < best.hCost))
                best = nodes[i];
        }
        return best;
    }

    private static int Manhattan(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static List<Vector2Int> ReconstructPath(Node endNode)
    {
        var path = new List<Vector2Int>();
        Node current = endNode;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }
}
