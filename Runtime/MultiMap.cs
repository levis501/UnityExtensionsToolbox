using System.Collections;
using System.Collections.Generic;

public class MultiMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>

{


  Dictionary<TKey, List<TValue>> d;

  public Dictionary<TKey, List<TValue>>.KeyCollection Keys
  {
    get
    {
      return d.Keys;
    }
  }

  public MultiMap()
  {
    d = new Dictionary<TKey, List<TValue>>();
  }

  public void RemoveAll(IEnumerable<(TKey k, TValue v)> kv_pairs)
  {
    foreach (var p in kv_pairs) {
      this.Remove(p.k, p.v);
    }
  }

  private void Remove(TKey k, TValue v)
  {
    if (d.ContainsKey(k))
    {
      d[k].Remove(v);
      if (d[k].Count == 0)
      {
        d.Remove(k);
      }
    }
  }

  public MultiMap(IEnumerable<(TKey k, TValue v)> kv_pairs)
  {
    d = new Dictionary<TKey, List<TValue>>();
    foreach (var kvp in kv_pairs)
    {
      this.Add(kvp);
    }
  }

  public void Add(TKey k, TValue v)
  {
    if (d.ContainsKey(k))
    {
      d[k].Add(v);
    }
    else
    {
      d[k] = new List<TValue> { v };
    }
  }

  public void Add((TKey k, TValue v) a)
  {
    this.Add(a.k, a.v);
  }

  public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
  {
    foreach (var k in d.Keys)
    {
      foreach (var v in d[k])
      {
        yield return new KeyValuePair<TKey, TValue>(k, v);
      }
    }
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return (IEnumerator)GetEnumerator();
  }

  public List<TValue> this[TKey key]
  {
    get => this.d[key];
  }

}
