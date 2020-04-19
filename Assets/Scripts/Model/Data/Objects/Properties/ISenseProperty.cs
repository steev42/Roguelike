using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ISenseProperty : IProperty
{
    Vector2 Location { get; }
}

