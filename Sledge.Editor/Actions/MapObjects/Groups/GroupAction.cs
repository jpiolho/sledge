using System.Collections.Generic;
using System.Linq;
using Sledge.Common.Mediator;
using Sledge.DataStructures.MapObjects;
using Sledge.Editor.Documents;

namespace Sledge.Editor.Actions.MapObjects.Groups
{
    public class GroupAction : IAction
    {
        private readonly List<long> _groupedObjects;
        private long _groupId;
        private Dictionary<long, long> _originalChildParents;

        public GroupAction(IEnumerable<MapObject> groupedObjects)
        {
            _groupedObjects = groupedObjects.Select(x => x.ID).ToList();
        }

        public void Perform(Document document)
        {
            var objects = _groupedObjects
                .Select(x => document.Map.WorldSpawn.FindByID(x))
                .Where(x => x != null && x.Parent != null)
                .ToList();
            _originalChildParents = objects.ToDictionary(x => x.ID, x => x.Parent.ID);
            _groupId = document.Map.IDGenerator.GetNextObjectID();
            var group = new Group(_groupId);
            objects.ForEach(x => x.SetParent(group));
            group.SetParent(document.Map.WorldSpawn);
            group.UpdateBoundingBox();

            Mediator.Publish(EditorMediator.DocumentTreeStructureChanged);
        }

        public void Reverse(Document document)
        {
            var group = document.Map.WorldSpawn.FindByID(_groupId);
            var children = group.Children.ToList();
            children.ForEach(x => x.SetParent(document.Map.WorldSpawn.FindByID(_originalChildParents[x.ID])));
            children.ForEach(x => x.UpdateBoundingBox());
            group.SetParent(null);
            Dispose();

            Mediator.Publish(EditorMediator.DocumentTreeStructureChanged);
        }

        public void Dispose()
        {
            _originalChildParents = null;
            _groupId = 0;
        }
    }
}