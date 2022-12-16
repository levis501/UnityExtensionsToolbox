using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultiMap<TKey, TValue> : Dictionary<TKey, List<TValue>>

{

  public void RemoveAll(IEnumerable<(TKey k, TValue v)> kv_pairs)
  {
    foreach (var p in kv_pairs) {
      this.Remove(p.k, p.v);
    }
  }

  private void Remove(TKey k, TValue v)
  {
    if (this.ContainsKey(k))
    {
      this[k].Remove(v);
      if (this[k].Count == 0)
      {
        this.Remove(k);
      }
    }
  }

  public MultiMap(IEnumerable<(TKey k, TValue v)> kv_pairs)
  {
    foreach (var kvp in kv_pairs)
    {
      this.Add(kvp);
    }
  }

  public void Add(TKey k, TValue v)
  {
    if (this.ContainsKey(k))
    {
      this[k].Add(v);
    }
    else
    {
      this.Add(k, new List<TValue> { v });
    }
  }

  public void Add((TKey k, TValue v) a)
  {
    this.Add(a.k, a.v);
  }

  public Dictionary<TKey, TValue> ToDictionary(Func<IEnumerable<TValue>, TValue> f)
  {
    return this.Keys.ToDictionary(k => k, k => f(this[k]));
  }

}
