using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Transform = UnityEngine.Transform;
using UnityEngine.UIElements;

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

  public static Vector3 ToVector3(this float[] f)
  {
    var n = f.Length;
    return new Vector3(
      n > 0 ? f[0] : 0,
      n > 1 ? f[1] : 0,
      n > 2 ? f[2] : 0
      );
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

  public static Vector3 Sum(this IEnumerable<Vector3> vs)
  {
    Func<Vector3, Vector3, Vector3> add = (x, y) => (x + y);
    return vs.Aggregate(add);
  }

  public static Vector3 Mean(this IEnumerable<Vector3> vs)
  {
    return vs.Sum() / vs.Count();
  }


  #endregion

  #region RANDOM

  public static float Range(this System.Random rng, float min, float max)
  {
    return (float)rng.NextDouble() * (max - min) + min;
  }

  public static Vector3 InBounds(this System.Random rng, Bounds b)
  {
    return new Vector3(
      rng.Range(b.min.x, b.max.x),
      rng.Range(b.min.y, b.max.y),
      rng.Range(b.min.z, b.max.z)
    );
  }

  public static T Choice<T>(this System.Random rng, IEnumerable<T> items)
  {
    return items.ElementAt(rng.Next(items.Count()));
  }
  #endregion

  #region QUATERNIONS
  public static Vector3 Up(this Quaternion q)
  {
    return q * Vector3.up;
  }

  public static Vector3 Right(this Quaternion q)
  {
    return q * Vector3.right;
  }

  public static Vector3 Forward(this Quaternion q)
  {
    return q * Vector3.forward;
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

  public static IEnumerable<(Tx, Ty)> PairWith<Tx, Ty>(this IEnumerable<Tx> xs, Ty y)
  {
    return xs.Select(x => (x, y));
  }

  public static (Ty, Tx) FlipTuple<Tx, Ty>((Tx, Ty) xy) => (xy.Item2, xy.Item1);
  public static (Tx, Ty, Tz) FlattenTuple<Tx, Ty, Tz>(((Tx, Ty), Tz) xyz) => (xyz.Item1.Item1, xyz.Item1.Item2, xyz.Item2);

  public static IEnumerable<(Tx, Ty)> Product<Tx, Ty>(this IEnumerable<Tx> xs, IEnumerable<Ty> ys)
  {
    var yx = xs.PairWith(ys).Select(FlipTuple).Select(ysx => ysx.Item1.PairWith(ysx.Item2)).SelectMany(a => a);
    return yx.Select(FlipTuple);
  }

  public static IEnumerable<(Tx, Ty)> Product<Tx, Ty>(this IEnumerable<Tx> xs, Ty y)
  {
    return xs.Select(x => (x, y));
  }

  public static IEnumerable<(Tx, Ty, Tz)> Product<Tx, Ty, Tz>(this IEnumerable<Tx> xs, IEnumerable<Ty> ys, IEnumerable<Tz> zs)
  {
    var a = xs.Product(ys).Product(zs);
    return a.Select(FlattenTuple);
  }


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

  public static IEnumerable<T> RotateFirst<T>(this IEnumerable<T> items, int n = 1)
  {
    var tail = items.Take(n);
    var head = items.Skip(n);
    return head.Concat(tail);
  }

  public static IEnumerable<(T, T)> PairAdjacentCyclic<T>(this IEnumerable<T> items)
  {
    return items.Zip(items.RotateFirst(), (a, b) => (a, b));
  }

  public static IEnumerable<(Tx, Ty)> Zip2<Tx, Ty>(this IEnumerable<Tx> xs, IEnumerable<Ty> ys)
  {
    return xs.Zip(ys, (x, y) => (x, y));
  }

  public static IEnumerable<(Tx, Ty, Tz)> Zip3<Tx, Ty, Tz>(this IEnumerable<Tx> xs, IEnumerable<Ty> ys, IEnumerable<Tz> zs)
  {
    return xs.Zip2(ys).Zip(zs, (xy, z) => (xy.Item1, xy.Item2, z));
  }

  public static IEnumerable<TResult> ZipWith3<Tx, Ty, Tz, TResult>(this IEnumerable<Tx> xs, IEnumerable<Ty> ys, IEnumerable<Tz> zs, Func<Tx, Ty, Tz, TResult> f)
  {
    return xs.Zip3(ys, zs).Select<(Tx, Ty, Tz), TResult>(f.TupleArgs());
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

  public static MultiMap<TKey, TValue> ToMultiMap<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict)
  {
    return new MultiMap<TKey, TValue>(dict);
  }

  public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, System.Random rng)
  {
    return items.OrderBy(x => rng.Next());
  }

  // Thanks to https://stackoverflow.com/a/914198/233142
  public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
      Func<TSource, TKey> selector, IComparer<TKey> comparer)
  {
    if (source == null) throw new ArgumentNullException("source");
    if (selector == null) throw new ArgumentNullException("selector");
    comparer ??= Comparer<TKey>.Default;

    using (var sourceIterator = source.GetEnumerator())
    {
      if (!sourceIterator.MoveNext())
      {
        throw new InvalidOperationException("Sequence contains no elements");
      }
      var min = sourceIterator.Current;
      var minKey = selector(min);
      while (sourceIterator.MoveNext())
      {
        var candidate = sourceIterator.Current;
        var candidateProjected = selector(candidate);
        if (comparer.Compare(candidateProjected, minKey) < 0)
        {
          min = candidate;
          minKey = candidateProjected;
        }
      }
      return min;
    }
  }

  public static T RandomElement<T>(this IEnumerable<T> items, System.Random rng)
  {
    var index = rng.Next(items.Count());
    return items.ElementAt(index);
  }


  #endregion

  #region FUNCTIONS
  public static Func<(Tx, Ty, Tz), TResult> TupleArgs<Tx, Ty, Tz, TResult>(this Func<Tx, Ty, Tz, TResult> f)
  {
    return (xyz) => f(xyz.Item1, xyz.Item2, xyz.Item3);
  }

  public static Func<(Tx, Ty), TResult> TupleArgs<Tx, Ty, TResult>(this Func<Tx, Ty, TResult> f)
  {
    return (xy) => f(xy.Item1, xy.Item2);
  }

  public static Func<Tx, Ty, Tz, TResult> UntupleArgs<Tx, Ty, Tz, TResult>(this Func<(Tx, Ty, Tz), TResult> f)
  {
    return (x, y, z) => f((x, y, z));
  }

  public static Func<Tx, Ty, TResult> UntupleArgs<Tx, Ty, TResult>(this Func<(Tx, Ty), TResult> f)
  {
    return (x, y) => f((x, y));
  }

  public static Func<Ta, Tc> After<Ta, Tb, Tc>(this Func<Tb, Tc> fbc, Func<Ta, Tb> fab) => (a => fbc(fab(a)));

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

  // map a float value from 0..1 to a color gradient
  public static Color HueGradient(float f) => Color.HSVToRGB(f, 1, 1);

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

  public static IEnumerable<T> GetChildrenWith<T>(this Transform root)
  {
    return root.GetChildren().Select(c => c.GetComponent<T>()).Where(s => s != null);
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

  public static float Fract(float v)
  {
    return v - Mathf.Floor(v);
  }
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

  public static float SignedTriangleVolume(IEnumerable<Vector3> vs)
  {
    if ((vs == null) || (vs.Count() != 3)) return 0;
    var a = vs.ElementAt(0);
    var b = vs.ElementAt(1);
    var c = vs.ElementAt(2);
    return Vector3.Dot(Vector3.Cross(a, b), c) / 6;
  }

  public static float SignedTriangleVolume(IEnumerable<Vector3> vertices, IEnumerable<int> vis)
  {
    return SignedTriangleVolume(vis.Select(vi => vertices.ElementAt(vi)));
  }

  public static float SignedTriangleVolume(this Mesh m, IEnumerable<int> vis)
  {
    return SignedTriangleVolume(m.vertices, vis);
  }

  public static float SignedVolume(IEnumerable<Vector3> vertices, IEnumerable<int> triangles)
  {
    return triangles.GroupsOf(3).Select(vis => SignedTriangleVolume(vertices, vis)).Sum();
  }

  public static float SignedVolume(this Mesh m)
  {
    return SignedVolume(m.vertices, m.triangles);
  }
  public static float Volume(this Mesh m)
  {
    return Mathf.Abs(m.SignedVolume());
  }

  // returns a vertex index lookup to a list of mergable vertex indices
  public static MultiMap<int, int> MergeMap(this Mesh m, float mergeDistance)
  {
    Func<float, int> eqC = c => Mathf.FloorToInt(c / mergeDistance);
    Func<Vector3, Vector3Int> eqV =
      v => (new Vector3Int(eqC(v.x), eqC(v.y), eqC(v.z)));

    var eqVerts = m.vertices.Select((v, i) => (eqV(v), i)).ToMultiMap();

    var result = m.vertices.Select((v, i) => (i, eqVerts[eqV(v)])).ToDictionary();
    return new MultiMap<int, int>(result);
  }

  public static List<List<int>> MergeSets(this Mesh m, float eqDistance)
  {
    Func<float, int> eqC = c => Mathf.FloorToInt(c / eqDistance);
    Func<Vector3, Vector3Int> eqV =
      v => (new Vector3Int(eqC(v.x), eqC(v.y), eqC(v.z)));

    var eqVerts = m.vertices.Select((v, i) => (eqV(v), i)).ToMultiMap();

    var mergeSets = eqVerts.Values.ToList();

    return mergeSets;
  }

  public static List<(int a, int b)> QuadTriangles(this Mesh importMesh)
  {
    var vit = importMesh.VertexIndexToTriangleMapping();
    var result = new List<(int a, int b)>();

    while (vit.Any())
    {
      var singleton_vertex = vit.Keys.First(i => vit[i].Count == 1);
      var a = vit[singleton_vertex].First();
      var va = importMesh.TriangleVertexIndices(a);
      var shared_verts = va.Where(i => i != singleton_vertex).ToArray();
      var b = vit[shared_verts[0]].Intersect(vit[shared_verts[1]]).Where(t => t != a).First();
      var vb = importMesh.TriangleVertexIndices(b);
      result.Add((a, b));
      vit.RemoveAll(va.Select(v => (v, a)));
      vit.RemoveAll(vb.Select(v => (v, b)));
    }
    return result;
  }

  public static int[] QuadVertsFromTriangles(this Mesh m, int a, int b)
  {
    var va = new List<int> { m.triangles[3 * a], m.triangles[(3 * a) + 1], m.triangles[(3 * a) + 2] };
    var vb = new List<int> { m.triangles[3 * b], m.triangles[(3 * b) + 1], m.triangles[(3 * b) + 2] };
    var common = va.Intersect(vb).ToArray();
    var uncommon = va.Union(vb).Except(common).ToArray();
    if ((common.Length != 2) || (uncommon.Length != 2))
    {
      throw new NotSupportedException("Triangles mush share two vertices");
    }
    var vi = new int[] { uncommon[0], common[0], uncommon[1], common[1] };
    var verts = vi.Select(i => m.vertices[i]).ToArray();
    var norm = m.normals[common[0]];
    // if normal is not consitant with the winding above, reverse the order of the vertices
    if (Vector3.Dot(norm, Vector3.Cross(verts[3] - verts[0], verts[0] - verts[1])) < 0)
    {
      vi = vi.Reverse().ToArray();
    }
    return vi;
  }

  public static List<int> Quads(this Mesh mesh)
  {
    var quadTriangles = mesh.QuadTriangles();
    var quads = quadTriangles.SelectMany(qt => mesh.QuadVertsFromTriangles(qt.a, qt.b));
    return quads.ToList();
  }

  #endregion
  #region GIZMOS

  public static void GizmoArrow(Vector3 pFrom, Vector3 pTo)
  {
    Gizmos.DrawLine(pFrom, pTo);
    var d = pTo - pFrom;
    var r = d.magnitude * 0.1f;
    var ah = pFrom + 0.8f * d;
    for (float a = 0; a <= 1; a += 0.1f)
    {
      Gizmos.DrawSphere(ah + 0.2f * a * d, r * 0.5f * (1 - a));
    }
  }

  #endregion
}