using UnityEngine;

public class GridSystem
{
    private int _width;
    private int _height;

    public GridSystem(int width, int height)
    {
        _width = width;
        _height = height;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z), Color.white, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z);
    }

}
