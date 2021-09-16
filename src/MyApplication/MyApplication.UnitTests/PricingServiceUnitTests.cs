using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyApplication.UnitTests
{
    [TestClass]
    public class PricingServiceUnitTests
    {
        [TestMethod]
        public void PricingService_CalculateDiscount_CanHandlePossitivePriceQuantity()
        {
            PricingService ps = new PricingService();
            double price = ps.CalculateDiscount(1, 1);

            Assert.AreEqual(price, 1);
        }

        //[TestMethod]
        //public void PricingService_CalculateDiscount_CanHandleCeroPrice()
        //{
        //    PricingService ps = new PricingService();
        //    double price = ps.CalculateDiscount(0, 12);

        //    Assert.AreEqual(price, 0);
        //}

        //[TestMethod]
        //public void PricingService_CalculateDiscount_CanHandleNegativeQuantity()
        //{
        //    PricingService ps = new PricingService();
        //    double negativePrice = ps.CalculateDiscount(10, -10);
        //    double positivePrice = ps.CalculateDiscount(10, 10);

        //    Assert.AreEqual(negativePrice, positivePrice);
        //}
    }
}
