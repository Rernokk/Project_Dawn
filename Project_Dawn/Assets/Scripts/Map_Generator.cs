using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class Map_Generator : MonoBehaviour {
    [SerializeField]
    Texture2D mapToGenerate;

    [Serializable]
    public class ColorPair
    {
        public Color color;
        public GameObject gameObject;
    }

    public class Node
    {
        public int x, y;
        public Color col;
        public Node(Color Col, int X, int Y)
        {
            col = Col;
            x = X;
            y = Y;
        }
    }

    public ColorPair[] ColorTable;

    [Button("Generate")]
    void GenerateMap()
    {
        List<Node> myTiles = new List<Node>();
        for (int i = 0; i < mapToGenerate.width; i++)
        {
            for (int j = 0; j < mapToGenerate.height; j++)
            {
                if (mapToGenerate.GetPixel(i,j).a > 0)
                {
                    myTiles.Add(new Node(mapToGenerate.GetPixel(i, j), i, j));
                }
            }
        }

        Mesh myMapMesh = new Mesh();
        foreach (Node n in myTiles)
        {
            GameObject temp = null;
            for (int i = 0; i < ColorTable.Length; i++)
            {
                if (n.col == ColorTable[i].color)
                {
                    temp = ColorTable[i].gameObject;
                    break;
                }
            }
            if (temp != null)
            {
                temp = Instantiate(temp, new Vector3(n.x, n.y, transform.position.z), Quaternion.identity);
                temp.transform.parent = GameObject.Find("Map").transform;
            }
            Vector2[] vertArray = temp.GetComponent<SpriteRenderer>().sprite.vertices;
            Vector3[] castedVertices = new Vector3[vertArray.Length];
            for (int i = 0; i < castedVertices.Length; i++)
            {
                castedVertices[i] = new Vector3(vertArray[i].x, vertArray[i].y, 0);
            }

        }
    }
}
