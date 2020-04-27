using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ISenseProperty : IProperty
{
    SenseEnum Sense { get; }
    bool NeedsUpdate { get; }

    Vector2 UpdatedAt { get; set; }
}

