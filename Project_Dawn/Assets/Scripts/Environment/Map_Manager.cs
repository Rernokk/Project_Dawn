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
    public Color myIDColor;
    public Tile(bool isOccupied, bool doesBitmask, Color myIDColor)
    {
      this.doesBitmask = doesBitmask;
      this.isOccupied = isOccupied;
      this.myIDColor = myIDColor;
    }
  }
  #endregion

  #region Variables
  private static Map_Manager instance = null;
  static Tile[,] levelArray;
  static int mapWidth, mapHeight;
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
  private void Awake()
  {
    if(instance != null && instance != this){
      Destroy(this.gameObject);
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
  }

  GameObject GetTileObject(List<ColorPair> pairList, Color col){
    foreach (ColorPair pair in pairList){
      if (pair.col == col){
        return pair.obj;
      }
    }
    return null;
  }

  IEnumerator StallCreation(Texture2D myPixelMap, List<ColorPair> pairListing){
    yield return null;
    Transform LevelHolder = GameObject.Find("Level_Loader").transform;
    for (int i = 0; i < myPixelMap.width; i++)
    {
      for (int j = 0; j < myPixelMap.height; j++)
      {
        if (levelArray[i, j].isOccupied)
        {
          Instantiate(GetTileObject(pairListing, levelArray[i, j].myIDColor), position: new Vector3(i, j, 0), rotation: Quaternion.identity).transform.parent = LevelHolder;
        }
      }
    }
  }
  #endregion

  #region Public Methods
  public void LoadMap(Texture2D myPixelMap, List<ColorPair> pairListing)
  {
    //Populating level array & tiles.
    levelArray = new Tile[myPixelMap.width, myPixelMap.height];
    mapWidth = myPixelMap.width;
    mapHeight = myPixelMap.height;
    for (int i = 0; i < myPixelMap.width; i++)
    {
      for (int j = 0; j < myPixelMap.height; j++)
      {
        Color col = myPixelMap.GetPixel(i, j);
        if (col.a == 0)
        {
          //Full Alpha Transparency implies empty tile, open space.
          levelArray[i, j].isOccupied = false;
          levelArray[i, j].doesBitmask = false;
        }
        else
        {
          levelArray[i, j].isOccupied = true;
          levelArray[i, j].doesBitmask = true;
        }
        levelArray[i, j].myIDColor = col;
      }
    }

    ////Spawning Tiles into worldspace.
    Transform LevelHolder = GameObject.Find("Level_Loader").transform;
    for (int i = 0; i < myPixelMap.width; i++)
    {
      for (int j = 0; j < myPixelMap.height; j++)
      {
        if (levelArray[i, j].isOccupied && LevelArray[i,j].myIDColor != Color.clear)
        {
          Instantiate(GetTileObject(pairListing, levelArray[i, j].myIDColor), position: new Vector3(i, j, 0), rotation: Quaternion.identity).transform.parent = LevelHolder;
        }
      }
    }
  }


  public int IsTileOpen(Vector2 pos){
    
    if ((pos.x < 0 || pos.x >= mapWidth) || (pos.y < 0 || pos.y >= mapHeight))
    {
      //Debugger, throw -1 for out of range.
      return 0;
    }
    if (levelArray[(int) pos.x, (int) pos.y].isOccupied){
      //Occupied, use in calculation
      return 1;
    }
    //Null Calculation Value.
    return 0;
  }
  #endregion
}
