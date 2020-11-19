using System;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string name)
            : base($"{name} value could not be found!")
        {
        }
        public ObjectNotFoundException(string name, int id)
            : base($"{name} value could not be found with the id = {id}!")
        {
        }
    }
}
