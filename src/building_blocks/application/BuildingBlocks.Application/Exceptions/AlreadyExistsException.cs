using System;

namespace BuildingBlocks.Application.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string entity, string id) : base($"A {entity} with id {id} already exists")
        {
            
        }
    }
}