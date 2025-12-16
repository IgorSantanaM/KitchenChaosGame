using System;
using UnityEngine;

public interface IHasProgress
{
    public event Action<float> OnProgressChanged;
}
