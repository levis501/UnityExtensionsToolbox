using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{

  #region VECTORS
  public static Vector2 XY(this Vector3 v)
  {
    return new Vector2(v.x, v.y);
  }
  public static Vector2 XZ(this Vector3 v)
  {
    return new Vector2(v.x, v.z);
  }
  public static Vector3 Abs(this Vector3 v)
  {
    return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
  }

  public static Vector3 ToVector3(this Vector2 v, float z = 0)
  {
    return new Vector3(v.x, v.y, z);
  }

  public static float Max(this Vector2 v)
  {
    return Mathf.Max(v.x, v.y);
  }

  public static float Max(this Vector3 v)
  {
    return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
  }

  public static Vector2 Map(this Vector2 v, Func<float, float> f)
  {
    return new Vector2(f(v.x), f(v.y));
  }

  public static Vector3 Map(this Vector3 v, Func<float, float> f)
  {
    return new Vector3(f(v.x), f(v.y), f(v.z));
  }
  
  public static Vector3 WithY(this Vector3 v, float y)
  {
    v.y = y;
    return v;
  }
  #endregion

  #region COLLECTIONS
  public static HashSet<T> ToHashSet<T>(this IEnumerable<T> e)
  {
    var hashSet = new HashSet<T>(e);
    return hashSet;
  }
  public static List<GameObject> RemoveNulls(this List<GameObject> e)
  {
    e.RemoveAll(x => x == null);
    return e;
  }
  public static V Get<K,V>(this Dictionary<K, V> d, K key, V defaultValue)
  {
    if (d.ContainsKey(key))
    {
      return d[key];
    } else
    {
      return defaultValue;
    }
  }
  #endregion

  #region COLOR AND TEXTURE
  public static Color WithAlpha(this Color original, float alpha)
  {
    original.a = alpha;
    return original;
  }

  public static Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];
    for (int i = 0; i < pix.Length; ++i)
    {
      pix[i] = col;
    }
    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();
    return result;
  }

  public static Color HueComplement(this Color c)
  {
    float h, s, v;
    Color.RGBToHSV(c, out h, out s, out v);
    h = (h + 0.5f) % 1.0f;
    return Color.HSVToRGB(h, s, v);
  }

  #endregion

  #region LAYOUT CALCULATIONS
  public static float SquareTileSize(float width, float height, float border, float spacing, int rows, int columns)
  {
    float maxWidth = (width - 2 * border - spacing * (columns - 1)) / columns;
    float maxHeight = (height - 2 * border - spacing * (rows - 1)) / rows;
    float dimension = Mathf.Min(maxWidth, maxHeight);
    //Debug.Log($"maxWidth is {maxWidth}, maxHeight is {maxHeight}");
    return dimension;
  }

  // Returns the local x,y position of the lower left corner of the tile at row r, column c
  public static Vector2 SquareTileLocation(int row, int column, float size, float border, float spacing)
  {

    Func<int, float> loc = (int a) => border + a * (size + spacing);
    return new Vector2(loc(column), loc(row));
  }

  public static (int, int) ClosestSquareTile(Vector2 p, float size, float border, float spacing)
  {
    Func<float, int> iLoc = (float a) => Mathf.RoundToInt((a - border) / (size + spacing));
    return (iLoc(p.y), iLoc(p.x));
  }

  #endregion

  #region TRANSFORMS
  public static List<Transform> GetAllChildren(this Transform root)
  {
    var children = Enumerable.Range(0, root.childCount).Select(i => root.GetChild(i));

    var list = children.Aggregate(children.ToList(), (acc, x) => { acc.AddRange(x.GetAllChildren()); return acc; });

    return list;
  }

  public static Transform OverwriteChild(this Transform parent, string childName, GameObject prefab = null)
  {
    Transform child = parent.Find(childName);
    if (child != null)
    {
      child.gameObject.DestroyEA();
    }
    if (prefab == null)
    {
      child = (new GameObject()).transform;
      child.SetParent(parent, true);
    }
    else
    {
      child = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent).transform;
    }
    child.name = childName;
    return child;
  }

  public static Transform FindOrCreate(this Transform parent, string childName)
  {
    Transform child = parent.Find(childName);
    if (child == null)
    {
      child = (new GameObject()).transform;
      child.SetParent(parent, true);
    }
    child.name = childName;
    return child;
  }

  public static void DestroyEA(this UnityEngine.Object o)
  {
    if (Application.isPlaying)
    {
      UnityEngine.Object.Destroy(o);
    }
    else
    {
      UnityEngine.Object.DestroyImmediate(o);
    }
  }
  #endregion

  #region MONOBEHAVIOUR
  public static T Required<T>(this MonoBehaviour o)
  {
    T c = o.GetComponent<T>();
    if (c == null)
    {
      Debug.LogError($"GameObject {o.name} is missing a require component of type {typeof(T)}");
    }
    return c;
  }
  #endregion

  #region MATH

  public static float mod(float a, float b)
  {
    return ((a % b) + b) % b; // only positive modulus values
  }
  #endregion
}