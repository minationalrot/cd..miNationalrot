// --------------------------------------------------------------------------------------------------------------------
// <copyright company="o.s.i.s.a. GmbH" file="LighthouseTest.cs">
//    (c) 2014. See licence text in binary folder.
// </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using lighthouse.net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace miNationalrot.Tests
{
    [TestClass]
    public class LighthouseTest
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ExampleComAudit()
        {
            var lh = new Lighthouse();
            var res = lh.Run("http://minationalrot.ch").Result;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Performance);
            Assert.IsTrue(res.Performance > 0.5m);

            Assert.IsNotNull(res.Accessibility);
            Assert.IsTrue(res.Accessibility > 0.5m);

            Assert.IsNotNull(res.BestPractices);
            Assert.IsTrue(res.BestPractices > 0.5m);

            Assert.IsNotNull(res.Pwa);
            Assert.IsTrue(res.Pwa > 0.5m);

            Assert.IsNotNull(res.Seo);
            Assert.IsTrue(res.Seo > 0.5m);

            //https://github.com/petabridge/lighthouse
        }

        #endregion
    }
}