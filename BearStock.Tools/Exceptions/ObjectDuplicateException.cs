using System;

namespace BearStock.Tools.Exceptions
{
    public class ObjectDuplicateException : Exception
    {
        public ObjectDuplicateException(string name)
            : base($"Entity with name {name} already exists")
        {

        }

        public ObjectDuplicateException(Guid uuid)
            : base($"Entity with uuid {uuid} already exists")
        {

        }
    }
}