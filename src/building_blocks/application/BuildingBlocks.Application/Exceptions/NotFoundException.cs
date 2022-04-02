using System;

namespace BuildingBlocks.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, string id) : base($"Could not find {entity} with id {id}")
        {
            
        }
    }

}