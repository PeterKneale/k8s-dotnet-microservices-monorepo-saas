using System;

namespace BuildingBlocks.Application.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public ValidationFailedException(string error) : base(error)
        {
        }
    }
}