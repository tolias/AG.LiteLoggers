using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AG.LiteLoggers.Tests
{
    [TestClass]
    public class ExceptionWrapper_Test
    {
        [TestMethod]
        public void TryGetValueOrExceptionString_Can_ReturnValues()
        {
            string someString = "Some string...";
            ExceptionWrapper.Returner<string> stringReturner = () => someString;
            ExceptionWrapper.ExceptionConverter<string> exceptionConverter = ExceptionInfoProvider.GetExceptionInfo;

            string gottenString = ExceptionWrapper.TryGetValueOrExceptionString(stringReturner, exceptionConverter);
            Assert.AreEqual<string>(someString, gottenString);
        }

        [TestMethod]
        public void TryGetValueOrExceptionString_Can_ReturnExceptionInfo()
        {
            ApplicationException exceptionToThrow = new ApplicationException("Threwn exception");
            ExceptionWrapper.Returner<string> stringReturner = () =>
            {
                throw exceptionToThrow;
            };
            ExceptionWrapper.ExceptionConverter<string> exceptionConverter = ExceptionInfoProvider.GetExceptionInfo;

            string gottenString = ExceptionWrapper.TryGetValueOrExceptionString(stringReturner, exceptionConverter);

            string expected = ExceptionInfoProvider.GetExceptionInfo(exceptionToThrow);
            Assert.AreEqual<string>(expected, gottenString);
        }
    }
}
