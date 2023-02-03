using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Util;
using Transform = UnityEngine.Transform;

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

  public static bool LessThan(this Vector3Int a, Vector3Int b)
  {
    if (a.x < b.x) return true;
    if (a.x > b.x) return false;
    if (a.y < b.y) return true;
    if (a.y > b.y) return false;
    return a.z < b.z;
  }

  public static bool LessThan(this Vector3 a, Vector3 b)
  {
    if (a.x < b.x) return true;
    if (a.x > b.x) return false;
    if (a.y < b.y) return true;
    if (a.y > b.y) return false;
    return a.z < b.z;
  }

  public static Vector3Int ToVector3Int(this Vector3 a)
  {
    return new Vector3Int((int)a.x, (int)a.y, (int)a.z);
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

  public static Vector3 WithX(this Vector3 v, float x)
  {
    v.x = x;
    return v;
  }

  public static Vector3 WithY(this Vector3 v, float y)
  {
    v.y = y;
    return v;
  }

  public static Vector3 WithZ(this Vector3 v, float z)
  {
    v.z = z;
    return v;
  }

  public static Vector3 WithMagnitude(this Vector3 v, float magnitude)
  {
    return v.normalized * magnitude;
  }

  public static Vector2 WithMagnitude(this Vector2 v, float magnitude)
  {
    return v.normalized * magnitude;
  }

  public static Vector2 WithClampedMagnitude(this Vector2 v, float min, float max)
  {
    float mag = v.magnitude;
    if (mag < min) return v.WithMagnitude(min);
    if (mag > max) return v.WithMagnitude(max);
    return v;
  }

  public static Vector3 WithClampedMagnitude(this Vector3 v, float min, float max)
  {
    float mag = v.magnitude;
    if (mag < min) return v.WithMagnitude(min);
    if (mag > max) return v.WithMagnitude(max);
    return v;
  }


  // thanks to https://math.stackexchange.com/a/128999
  public static float TriangleArea(Vector3 a, Vector3 b, Vector3 c)
  {
    return 0.5f * Vector3.Cross(b - a, c - a).magnitude;
  }


  // Storing Spherical Coordinates in a Vector3
  public static float Radius(this Vector3 v)
  {
    return v.x;
  }
  public static Vector3 WithRadius(this Vector3 v, float r)
  {
    return v.WithX(r);
  }

  public static float Longitude(this Vector3 v)
  {
    return v.y;
  }
  public static Vector3 WithLongitude(this Vector3 v, float longitude)
  {
    return v.WithY(longitude);
  }
  public static float Latitude(this Vector3 v)
  {
    return v.z;
  }
  public static Vector3 WithLatitude(this Vector3 v, float latitude)
  {
    return v.WithZ(latitude);
  }

  public static Vector3 WithLongLat(this Vector3 v, float longitude, float latitude)
  {
    return v.WithLongitude(longitude).WithLatitude(latitude);
  }

  


  // thanks to https://stackoverflow.com/questions/10868135/cartesian-to-polar-3d-coordinates

  // r = sqrt(x * x + y * y + z * z)
  // long = acos(x / sqrt(x * x + y * y)) * (y < 0 ? -1 : 1)
  // lat = acos(z / r)

  public static Vector3 ToRLongLatZUp(this Vector3 v)
  {
    var radius = v.magnitude;
    var longitude = Mathf.Acos(v.x / v.XY().magnitude) * (v.y < 0 ? -1 : 1);
    var latitude = Mathf.Acos(v.z / radius);
    return new Vector3(radius, longitude, latitude);
  }

  // For Unity coordinates, with Y Up
  public static Vector3 ToRLongLat(this Vector3 v)
  {
    var radius = v.magnitude;
    var longitude = Mathf.Acos(v.x / v.XZ().magnitude) * (v.z < 0 ? -1 : 1);
    var latitude = Mathf.Acos(v.y / radius);
    return new Vector3(radius, longitude, latitude);
  }

  //  x = r * sin(lat) * cos(long)
  //  y = r * sin(lat) * sin(long)
  //  z = r * cos(lat)

  public static Vector3 FromRLongLatZUp(this Vector3 v)
  {
    var radius = v.x;
    var longitude = v.y;
    var latitude = v.z;

    var x = radius * Mathf.Sin(latitude) * Mathf.Cos(longitude);
    var y = radius * Mathf.Sin(latitude) * Mathf.Sin(longitude);
    var z = radius * Mathf.Cos(latitude);

    return new Vector3(x, y, z);
  }

  // For Unity coordinates, with Y Up
  public static Vector3 FromRLongLat(this Vector3 v)
  {
    var radius = v.x;
    var longitude = v.y;
    var latitude = v.z;

    var x = radius * Mathf.Sin(latitude) * Mathf.Cos(longitude);
    var y = radius * Mathf.Cos(latitude);
    var z = radius * Mathf.Sin(latitude) * Mathf.Sin(longitude);

    return new Vector3(x, y, z);
  }




  #endregion

  #region INTERSECTIONS
  public static Ray Reversed(this Ray ray)
  {
    return new Ray(ray.origin, -ray.direction);
  }

  public static Vector3 Intersection(this Plane plane, Ray ray)
  {
    float distance;
    if (plane.Raycast(ray, out distance))
    {
      return ray.GetPoint(distance);
    }
    throw new ArithmeticException("Ray plane intersection failed");
  }

  public static Bounds BoundsOnPlane(this Camera cam, Plane plane)
  {
    var bounds = new Bounds();
    try
    {
      foreach (var x in new[] { 0, cam.pixelWidth })
      {
        foreach (var y in new[] { 0, cam.pixelHeight })
        {
          var ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
          var point = plane.Intersection(ray);
          bounds.Encapsulate(point);
        }

      }
    }
    catch (Exception e)
    {
      throw new ArithmeticException("Camera plane intersection failed", e);
    }
    return bounds;
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
  public static V Get<K, V>(this Dictionary<K, V> d, K key, V defaultValue)
  {
    if (d.ContainsKey(key))
    {
      return d[key];
    }
    else
    {
      return defaultValue;
    }
  }

  public static Dictionary<G, HashSet<T>> MultiGroupBy<T, G>(this IEnumerable<T> x, Func<T, IEnumerable<G>> f)
  {
    var d = new Dictionary<G, HashSet<T>>();
    foreach (var t in x)
    {
      foreach (var g in f(t))
      {
        if (d.ContainsKey(g))
        {
          d[g].Add(t);
        }
        else
        {
          d[g] = new HashSet<T> { t };
        }
      }
    }
    return d;
  }

  public static Dictionary<K, HashSet<V>> ToDictionary<K, V>(this IEnumerable<IGrouping<K, V>> a)
  {
    return a.ToDictionary(g => g.Key, g => g.ToHashSet());
  }

  // Thanks to https://stackoverflow.com/a/64978120
  public static IEnumerable<T> AsSingleton<T>(this T item) => new[] { item };
  public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences) =>
      sequences.Aggregate(
          Enumerable.Empty<T>().AsSingleton(),
          (accumulator, sequence) => accumulator.SelectMany(
              accseq => sequence,
              (accseq, item) => accseq.Append(item)));

  public static (T, T) Sorted<T>(this (T, T) a) where T : IComparable<T>
  {
    if (a.Item2.CompareTo(a.Item1) > 0) return a;
    return (a.Item2, a.Item1);
  }

  public static (T, T) Reversed<T>(this (T, T) a) where T : IComparable<T>
  {
    return (a.Item2, a.Item1);
  }


  public static IEnumerable<T> WhereIndex<T>(this IEnumerable<T> items, Func<int, bool> f)
  {
    return items.Where((x, i) => f(i));
  }


  public static IEnumerable<IEnumerable<T>> GroupsOf<T>(this IEnumerable<T> items, int n)
  {
    IEnumerable<IEnumerable<T>> aggregate = new List<IEnumerable<T>> { items.Take(n) };
    var source = items.Skip(n);
    while (source.Any())
    {
      aggregate = aggregate.Append(source.Take(n));
      source = source.Skip(n);
    }
    return aggregate;
  }

  public static IEnumerable<T> RotateFirst<T>(this IEnumerable<T> items, int n=1)
  {
    var tail = items.Take(n);
    var head = items.Skip(n);
    return head.Concat(tail);
  }

  public static IEnumerable<(T, T)> PairAdjacentCyclic<T>(this IEnumerable<T> items)
  {
    return items.Zip(items.RotateFirst(), (a, b) => (a, b));
  }

  public static List<T> Ungroup<T>(this IEnumerable<IEnumerable<T>> groups)
  {
    return groups.SelectMany(x => x).ToList();
  }

  public static MultiMap<TKey, TValue> ToMultiMap<TKey, TValue>(this IEnumerable<(TKey, TValue)> kvpairs)
  {
    return new MultiMap<TKey, TValue>(kvpairs);
  }

  public static IEnumerable<(T element, int index)> Enumerated<T>(this IEnumerable<T> x)
  {
    return x.Select((x, i) => (x, i));
  }

  public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> kvpairs)
  {
    var mm = kvpairs.ToMultiMap();
    return mm.ToDictionary(x => x.First());
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
  public static List<Transform> AllDescendants(this Transform root)
  {
    var children = root.GetChildren();
    var list = children.Aggregate(children.ToList(), (acc, x) => { acc.Add(x); acc.AddRange(x.AllDescendants()); return acc; });

    return list;
  }

  public static IEnumerable<Transform> GetChildren(this Transform root)
  {
    return Enumerable.Range(0, root.childCount).Select(i => root.GetChild(i));
  }

  public static IEnumerable<GameObject> GetChildGameObjects(this Transform root)
  {
    return root.GetChildren().Select(t => t.gameObject);
  }

  public static void SetChildrenActive(this Transform root, bool active)
  {
    root.GetChildGameObjects().ToList().ForEach(o => o.SetActive(active));
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

  public static Transform CreateEmpty(this Transform parent, string childName, Vector3? localPosition = null)
  {
    Transform child = (new GameObject()).transform;
    child.SetParent(parent, true);
    child.name = childName;
    if (localPosition != null)
    {
      child.localPosition = (Vector3)localPosition;
    }
    return child;
  }


  public static Transform FindOrCreate(this Transform parent, string childName)
  {
    Transform child = parent.Find(childName);
    if (child == null)
    {
      child = parent.CreateEmpty(childName);
    }
    return child;
  }

  public static T FindComponentOrReplace<T>(this Transform parent, string childName, T prefab) where T : Component
  {
    Transform child = parent.Find(childName);
    T component = child?.GetComponent<T>();
    if (component == null)
    {
      if (child != null) { child.DestroyEA(); }
      component = GameObject.Instantiate<T>(prefab, parent);
      component.name = childName;
    }
    return component;
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

  public static void DestroyChildrenEA(this Transform transform)
  {
    var children = transform.AllDescendants();
    foreach (var child in children)
    {
      if (child != null)
      {
        DestroyEA(child.gameObject);
      }
    }
  }
  #endregion

  #region MONOBEHAVIOUR
  public static T Required<T>(this MonoBehaviour o)
  {
    T c = o.GetComponent<T>();
    if (c == null)
    {
      Debug.LogError($"GameObject {o.name} is missing a required component of type {typeof(T)}");
    }
    return c;
  }
  #endregion

  #region MATH

  public static float mod(float a, float b)
  {
    return ((a % b) + b) % b; // only positive modulus values
  }

  public static int mod(int a, int b)
  {
    return ((a % b) + b) % b; 
  }

  public static int xmod(this int a, int b) => ((a % b) + b) % b;
  public static float xmod(this float a, float b) => ((a % b) + b) % b;
  public static double xmod(this double a, double b) => ((a % b) + b) % b;
  #endregion

  #region MESH
  public static int TriangleCount(this Mesh m) { return m.triangles.Length / 3; }
  public static int[] TriangleVertexIndices(this Mesh m, int t)
  {
    return new int[] { m.triangles[t * 3], m.triangles[t * 3 + 1], m.triangles[t * 3 + 2] };
  }
  public static Vector3[] TriangleVertices(this Mesh m, int t)
  {
    return m.TriangleVertexIndices(t).Select(vi => m.vertices[vi]).ToArray();
  }
  public static Vector3 TriangleNormal(this Mesh m, int t)
  {
    var v = m.TriangleVertices(t);
    return Vector3.Cross(v[1] - v[0], v[2] - v[0]).normalized;
  }
  public static Vector3 TriangleCenter(this Mesh m, int t)
  {
    var v = m.TriangleVertices(t);
    return (v[0] + v[1] + v[2]) / 3;
  }

  public static MultiMap<int, int> VertexIndexToTriangleMapping(this Mesh m)
  {
    var vt_pairs = Enumerable.Range(0, m.TriangleCount()).SelectMany(t => m.TriangleVertexIndices(t).Select(v => (v, t)));

    return new MultiMap<int, int>(vt_pairs);
  }

  public static HashSet<int> ConnectedVertices(this Mesh m, int vi, Dictionary<int, List<int>> vit)
  {
    var connectedVertices = new HashSet<int>();

    var searchFrontier = new HashSet<int> { vi };
    while (searchFrontier.Count > 0)
    {
      vi = searchFrontier.First();
      searchFrontier.Remove(vi);
      connectedVertices.Add(vi);
      foreach (var t in vit[vi])
      {
        searchFrontier.UnionWith(
          m.TriangleVertexIndices(t).Where(v => !connectedVertices.Contains(v)));
      }
    }
    return connectedVertices;
  }

  public static HashSet<int> ConnectedTriangles(this Mesh m, int ti, MultiMap<int, int> vit)
  {
    var connectedVertices = new HashSet<int>();
    var connectedTriangles = new HashSet<int>();

    var searchFrontier = m.TriangleVertexIndices(ti).ToHashSet();
    while (searchFrontier.Count > 0)
    {
      var vi = searchFrontier.First();
      searchFrontier.Remove(vi);
      connectedVertices.Add(vi);
      foreach (var t in vit[vi])
      {
        connectedTriangles.Add(t);
        searchFrontier.UnionWith(
          m.TriangleVertexIndices(t).Where(v => !connectedVertices.Contains(v)));
      }
    }
    return connectedTriangles;
  }

  public static List<HashSet<int>> TriangleIslands(this Mesh m)
  {
    var islands = new List<HashSet<int>>();
    var visitedTriangles = new HashSet<int>();

    var vit = m.VertexIndexToTriangleMapping();
    foreach (var ti in Enumerable.Range(0, m.TriangleCount()))
    {
      if (!visitedTriangles.Contains(ti))
      {
        var island = m.ConnectedTriangles(ti, vit);
        visitedTriangles.UnionWith(island);
        islands.Add(island);
      }
    }
    return islands;
  }
  #endregion
}