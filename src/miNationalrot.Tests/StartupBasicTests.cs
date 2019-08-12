// --------------------------------------------------------------------------------------------------------------------
// <copyright company="o.s.i.s.a. GmbH" file="StartupBasicTests.cs">
//    (c) 2014. See licence text in binary folder.
// </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace miNationalrot.Tests
{
    [TestClass]
    public class StartupBasicTestsBase : BlazorIntegrationTestsBase<Startup>
    {
        #region Public Methods and Operators

        [TestMethod]
        public void TestServerPing()
        {
            var uri = new Uri("http://localhost:5000");

            // act
            var response = Client.GetAsync(uri).Result;

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = response.Content.ReadAsStringAsync().Result;
            content.Should().Contain(@"<!DOCTYPE html>");
        }

        #endregion
    }
}