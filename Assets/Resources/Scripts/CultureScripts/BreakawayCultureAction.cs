using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakawayCultureAction : CultureAction
{
    public BreakawayCultureAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        throw new System.NotImplementedException();
    }


}
