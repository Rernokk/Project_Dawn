using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
  #region Structs
  public struct Tile
  {
    public bool doesBitmask;
    public bool isOccupied;
    public Tile(bool isOccupied, bool doesBitmask)
    {
      this.doesBitmask = doesBitmask;
      this.isOccupied = isOccupied;
    }
  }
  #endregion

  #region Variables
  private static Map_Manager instance;
  Tile[,] levelArray;
  #endregion
  #region Properties
  public static Map_Manager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new Map_Manager();
      }
      return instance;
    }
  }

  public Tile[,] LevelArray
  {
    get
    {
      return levelArray;
    }
  }
  #endregion

  #region Private Methods
  private Map_Manager() { }

  private void Start()
  {
    DontDestroyOnLoad(gameObject);
  }
  #endregion
  #region Public Methods
  public void LoadMap(Texture2D myPixelMap)
  {
    levelArray = new Tile[myPixelMap.width, myPixelMap.height];
    for (int i = 0; i < myPixelMap.width; i++)
    {
      for (int j = 0; j < myPixelMap.height; j++)
      {
        Color col = myPixelMap.GetPixel(i, j);
        if (col.a == 0){
          levelArray[i, j].isOccupied = false;
          levelArray[i, j].doesBitmask = false;
        } else {
          levelArray[i, j].isOccupied = true;
          levelArray[i, j].doesBitmask = true;
        }
      }
    }
    int tileCount = 0;
    foreach (Tile t in levelArray){
      if (t.isOccupied){
        tileCount++;
      }
    }
    print(tileCount);
  }
  #endregion
}
