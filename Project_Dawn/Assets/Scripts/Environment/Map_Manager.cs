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
  static Tile[,,] levelArray;
  static int mapWidth, mapHeight;
  public List<Color> NonBitmaskValues;
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

  public Tile[,,] LevelArray
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
    if (instance != null && instance != this)
    {
      Destroy(this.gameObject);
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
    //NonBitmaskValues = new List<Color>();
  }

  GameObject GetTileObject(List<ColorPair> pairList, Color col)
  {
    foreach (ColorPair pair in pairList)
    {
      if (pair.col == col)
      {
        return pair.obj;
      }
    }
    return null;
  }

  /*IEnumerator StallCreation(Texture2D myPixelMap, List<ColorPair> pairListing){
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
  }*/
  #endregion

  #region Public Methods
  public void LoadMap(Texture2D myPixelMap, List<ColorPair> pairListing, int texDepth)
  {
    //Populating level array & tiles.
    if (levelArray == null)
    {
      levelArray = new Tile[myPixelMap.width, myPixelMap.height, 3];
      mapWidth = myPixelMap.width;
      mapHeight = myPixelMap.height;
    }

    for (int i = 0; i < myPixelMap.width; i++)
    {
      for (int j = 0; j < myPixelMap.height; j++)
      {
        Color col = myPixelMap.GetPixel(i, j);
        if (col.a == 0 || (col.r == 1 && col.g == 1 && col.b == 1))
        {
          //Full Alpha Transparency implies empty tile, open space.
          levelArray[i, j, texDepth].isOccupied = false;
          levelArray[i, j, texDepth].doesBitmask = false;
        }
        else
        {
          bool completed = false;
          foreach (Color entry in NonBitmaskValues){
            if (col == entry){
              completed = true;
              levelArray[i, j, texDepth].isOccupied = true;
              levelArray[i, j, texDepth].doesBitmask = false;
            }
          }
          if (!completed)
          {
            levelArray[i, j, texDepth].isOccupied = true;
            levelArray[i, j, texDepth].doesBitmask = true;
          }
        }
        levelArray[i, j, texDepth].myIDColor = col;
      }
    }

    ////Spawning Tiles into worldspace.
    Transform LevelHolder = new GameObject("Level_Depth_" + texDepth).transform;
    for (int i = 0; i < myPixelMap.width; i++)
    {
      for (int j = 0; j < myPixelMap.height; j++)
      {
        if ((levelArray[i, j, texDepth].isOccupied && LevelArray[i, j, texDepth].myIDColor != Color.clear) || levelArray[i,j,texDepth].myIDColor == Color.white)
        {
          GameObject temp = Instantiate(GetTileObject(pairListing, levelArray[i, j, texDepth].myIDColor), position: new Vector3(i, j, texDepth), rotation: Quaternion.identity);
          if (temp.transform.tag != "Player")
          {
            temp.transform.parent = LevelHolder;
          }
        }
      }
    }
  }


  public int IsTileOpen(Vector3 pos)
  {

    if ((pos.x < 0 || pos.x >= mapWidth) || (pos.y < 0 || pos.y >= mapHeight))
    {
      return 0;
    }
    if (levelArray[(int)pos.x, (int)pos.y, (int)pos.z].isOccupied)
    {
      //Occupied, use in calculation
      return 1;
    }
    //Null Calculation Value.
    return 0;
  }

  public int DoesTileBitmask(Vector3 pos){
    if((pos.x < 0 || pos.x >= mapWidth) || (pos.y < 0 || pos.y >= mapHeight)){
      return 0;
    }
    if (levelArray[(int) pos.x, (int) pos.y, (int) pos.z].isOccupied && levelArray[(int) pos.x, (int) pos.y, (int) pos.z].doesBitmask){
      return 1;
    }
    return 0;
  }
  #endregion
}
