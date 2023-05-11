using MySpot.Api.Services;

namespace MySpot.Tests.Unit.Shared
{
    public class TestClock : IClock
    {
        public DateTime Current() => new(2023, 01, 02);
    }
}
