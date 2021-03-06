using System.Collections.Generic;
using System.Linq;
using Sledge.DataStructures.MapObjects;
using Sledge.Editor.Actions;
using Sledge.Editor.Actions.MapObjects.Operations;

namespace Sledge.Editor.Problems
{
    public class DuplicateObjectIDs : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map)
        {
            var dupes = from o in map.WorldSpawn.FindAll()
                        group o by o.ID
                        into g
                        where g.Count() > 1
                        select g;
            foreach (var dupe in dupes)
            {
                yield return new Problem(GetType(), map, dupe, Fix, "Multiple objects have the same ID", "More than one object has the same ID. Each object ID should be unique. Fixing the problem will assign the duplicates new IDs.");
            }
        }

        public IAction Fix(Problem problem)
        {
            return new Edit(problem.Objects, (d, x) => x.ID = d.Map.IDGenerator.GetNextObjectID());
        }
    }
}