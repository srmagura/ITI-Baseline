using ITI.DDD.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain.ValueObjects;

namespace IntegrationTests
{
    [TestClass]
    public class BaseDataContextTests
    {
        [TestMethod]
        public async Task ItWorksOutsideOfUnitOfWork()
        {
            var contact = new FacilityContact(null, null);
            var dbFacility = new DbFacility("myFacility", contact)
            {
                Id = SequentialGuid.Next(),
            };

            using (var db = new AppDataContext())
            {
                db.Facilities.Add(dbFacility);
                await db.SaveChangesAsync();
            }

            using (var db = new AppDataContext())
            {
                var dbFacility2 = await db.Facilities.SingleAsync();
                Assert.AreEqual(dbFacility.Name, dbFacility2.Name);
            }
        }
    }
}
