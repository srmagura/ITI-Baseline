using ITI.DDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;

namespace IntegrationTests
{
    [TestClass]
    public class BaseDataContextTests
    {
        [TestMethod]
        public async Task ItWorksOutsideOfUnitOfWork()
        {
            var dbFacility = new DbFacility
            {
                Id = SequentialGuid.Next(),
                Name = "myFacility"
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
