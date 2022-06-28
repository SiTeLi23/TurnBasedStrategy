


using System;

public struct GridPosition :IEquatable<GridPosition>
{

    public int x;
    public int z;


    //constructor
    public GridPosition(int x,int z) 
    {
        this.x = x;
        this.z = z;
    
    }



    #region Handle Equality
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    //interface
    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }


    //default showing text
    public override string ToString()
    {
        return $"x: {x}; z: {z}";
    }



    //we need to override the comparison method ourself for our custom data struct
    public static bool operator ==(GridPosition a,GridPosition b) 
    {
        //for eaqual
        return a.x == b.x && a.z == b.z;


    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        //for not eaqual
        //return a.x != b.x || a.z != b.z;
        return !(a == b);


    }

    #endregion

}