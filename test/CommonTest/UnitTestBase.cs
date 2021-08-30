using Xunit.Abstractions;

namespace CommonTest
{
    public abstract class UnitTestBase
    {
        protected ITestOutputHelper Output { get; }

        public UnitTestBase(ITestOutputHelper outputHelper)
        {
            Output = outputHelper;
        }
    }
}