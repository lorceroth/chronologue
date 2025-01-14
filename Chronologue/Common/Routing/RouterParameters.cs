using System.Collections.Generic;

namespace Chronologue.Common.Routing;

public class RouterParameters : Dictionary<string, object>
{
    public bool TryGetParameter<TValue>(string key, out TValue? value)
    {
        value = default;

        if (TryGetValue(key, out var obj) && obj is TValue casted)
        {
            value = casted;

            return true;
        }

        return false;
    }
}
