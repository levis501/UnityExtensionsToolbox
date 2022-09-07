using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// Initially writing this as a momentum optimizer.  
// Thanks to https://www.geeksforgeeks.org/intuition-of-adam-optimizer/

public class MomentumOptimizer {

  System.Func<List<float>, float> costFunction;
  IEnumerable<float> momentum;
  float currentCost;
  List<float> values;
  int n;
  public float learning_rate = 0.03f;
  public float momemtum_retention = 0.7f;

  public MomentumOptimizer(System.Func<List<float>, float> costFunction, List<float> initial_values)
  {
    this.costFunction = costFunction;
    values = initial_values;
    n = initial_values.Count();
    momentum = values.Select(x => 0f);
    currentCost = costFunction(values);

  }

  public List<float> IterateOptimization(int maximum_iteration_count)
  {

    float a = learning_rate;
    float b = momemtum_retention; 

    for (int iteration=0; iteration < maximum_iteration_count; iteration++)
    {
      var g = Enumerable.Range(0, n).Select(i => LocalPartialGradient(values, i, currentCost));
      momentum = momentum.Zip(g, (m, g) => (b * m + (1 - b) * g));
      values = values.Zip(momentum, (v, m) => (v - a * m)).ToList();
      var newCost = costFunction(values);
      if (newCost == currentCost) break;
      currentCost = newCost;
    }
    return values.ToList();
  }

  private float LocalPartialGradient(IEnumerable<float> v, int i, float c)
  {
    float epsilon = 0.001f;
    var vv = v.ToList();
    vv[i] += epsilon;
    var cc = costFunction(vv);

    return (cc - c) / epsilon;
  }
}