using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct SpectrumCellIdentityData : IComponentData
{
    public int RowIndex;
    public int CellIndexInRow;
}
