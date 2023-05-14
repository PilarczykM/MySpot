namespace MySpot.Core.Exceptions
{
    public sealed class InvalidCapacityException : CustomException
    {
        public InvalidCapacityException(int capacity) : base($"Capacity {capacity} is invalid.")
        {
            Capacity = capacity;
        }

        public int Capacity { get; }
    }
}

