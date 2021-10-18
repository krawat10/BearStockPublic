using System;

namespace BearStock.Tools.Exceptions
{
    public class ObjectNotFoundException: Exception
    {

        public ObjectNotFoundException(string name)
        :base($"Entity with name {name} not found")
        {
            
        }

        public ObjectNotFoundException(Guid uuid)
        :base($"Entity with uuid {uuid} not found")
        {
            
        }
    }
}