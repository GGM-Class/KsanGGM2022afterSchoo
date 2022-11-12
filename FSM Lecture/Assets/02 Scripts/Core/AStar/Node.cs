using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector3Int pos;
    public Node _parent;

    public float G; 
    public float F;
    // AStar 알고리즘
    // G는 내가 지금까지 온 길
    // H는 내가 길은 모르지만 직선 거리로 가면 이정도 된다.
    // F = G + H




    public int CompareTo(Node other)
    {
        if (other.F == this.F) return 0;
        else 
            return (other.F < this.F) ? -1 : 1;
    }
    public override bool Equals(object obj) => this.Equals(obj as Node);
    public override int GetHashCode() => pos.GetHashCode();
    public bool Equals(Node p)
    {
        if(p is null)
            return false;
        if (this.GetType() != p.GetType())
            return false;

        return pos == p.pos;
    }

    public static bool operator == (Node lhs, Node rhs)
    {
        if(lhs is null)
        {
            if (rhs is null)
                return true;
            else
                return false;
        }
        return lhs.Equals(rhs);
    }
    public static bool operator != (Node lhs, Node rhs) => !(lhs == rhs);
}
