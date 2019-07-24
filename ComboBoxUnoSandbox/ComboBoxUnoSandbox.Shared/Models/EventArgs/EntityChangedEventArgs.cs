using System;
using System.Collections.Specialized;

namespace ComboBoxUnoSandbox.Shared.Models.EventArgs
{
    public class EntityChangedEventArgs
    {
        public NotifyCollectionChangedEventArgs NotifyCollectionChangedEventArgs { get; private set; }
        public object ChangedObject { get; set; }
        public string PropertyName { get; set; }
        public object Parent { get; set; }
        public bool IsCollectionSizeChanged { get; private set; }
        public object NotifyCollectionChanged { get; private set; }
        public Type CollectionEntityType { get; private set; }
        public bool IsRunAll { get; protected set; }

        public EntityChangedEventArgs(object changedObject, string propertyName)
        {
            ChangedObject = changedObject;
            PropertyName = propertyName;
            IsCollectionSizeChanged = false;
        }

        public EntityChangedEventArgs(object parent, object collection, NotifyCollectionChangedEventArgs e)
        {
            Parent = parent;
            NotifyCollectionChanged = collection;
            NotifyCollectionChangedEventArgs = e;
            IsCollectionSizeChanged = true;
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                CollectionEntityType = e.NewItems[0].GetType();
            }
            else if (e.OldItems != null && e.OldItems.Count > 0)
            {
                CollectionEntityType = e.OldItems[0].GetType();
            }
        }
    }
}
