using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DltLib.Tests.Util
{
    public static class DltLibAssert
    {
        public static void AssertException<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action();
                Assert.Fail($"Expected {typeof(TException).FullName} is not thrown");
            }
            catch (TException)
            {
            }
        }

        public static void AssertException<TException>(Action action, Predicate<TException> isExpectedException)
            where TException : Exception
        {
            try
            {
                action();
                Assert.Fail($"Expected {typeof(TException).FullName} is not thrown");
            }
            catch (TException ex)
            {
                if (!isExpectedException(ex))
                {
                    Assert.Fail($"Exception {ex.GetType().FullName} could not meet expectation");
                }
            }
        }
    }
}