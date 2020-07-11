using System;
using System.Linq;
using AppConfig;
using CoreTests.Helpers;
using DataContext;
using Domain;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.DTOs;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Iti.Baseline.Utilities;
using Iti.Baseline.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Application.Dto;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class ValueObjectTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            DomainEvents.ClearRegistrations();

            IOC.RegisterType<IAuthContext, TestAuthContext>();
            IOC.RegisterType<IAppAuthContext, TestAuthContext>();

            IOC.RegisterType<ILogWriter, ConsoleLogWriter>();
        }

        [TestMethod]
        public void NestedValueObjectsWithInitialValues()
        {
            var voh = new ValObjHolder()
            {
                Name = Guid.NewGuid().ToString(),
                SimpleAddress = new SimpleAddress("4034 Winecott Drive", "", "Apex", "NC", "27502"),
                PhoneNumber = new PhoneNumber("9198675309"),
                SimplePersonName = new SimplePersonName("Test", "Test", "Test"),
                ValueParent = new ValueParent("Parent", new ValueChild("Child")),
            };

            voh.ConsoleDump();
            Assert.IsNotNull(voh.SimpleAddress);
            Assert.IsNotNull(voh.PhoneNumber);
            Assert.IsNotNull(voh.SimplePersonName);
            Assert.IsNotNull(voh.ValueParent);
            Assert.IsNotNull(voh.ValueParent?.Child);

            Console.WriteLine("\n\n=== ENTITY TO DB ===");

            using (var db = new SampleDataContext())
            {
                var dbvoh = DbEntity.ToDb<DbValObjHolder>(voh);

                dbvoh.ConsoleDump();
                Assert.IsNotNull(dbvoh.SimpleAddress);
                Assert.IsNotNull(dbvoh.PhoneNumber);
                Assert.IsNotNull(dbvoh.SimplePersonName);
                Assert.IsNotNull(dbvoh.ValueParent);
                Assert.IsNotNull(dbvoh.ValueParent?.Child);

                db.ValObjHolders.Add(dbvoh);

                db.SaveChanges();
            }

            Console.WriteLine("\n\n=== READBACK DTO ===");

            using (var db = new SampleDataContext())
            {
                var dto = db.ValObjHolders
                    .Where(p => p.Id == voh.Id.Guid)
                    .ProjectToDto<ValObjHolderDto>();

                dto.ConsoleDump();
                Assert.IsNotNull(dto.SimpleAddress);
                Assert.IsNotNull(dto.PhoneNumber);
                Assert.IsNotNull(dto.SimplePersonName);
                Assert.IsNotNull(dto.ValueParent);
                Assert.IsNotNull(dto.ValueParent?.Child);
            }

            Console.WriteLine("\n\n=== READBACK ENTITY ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.ConsoleDump();
                Assert.IsNotNull(item.SimpleAddress);
                Assert.IsNotNull(item.PhoneNumber);
                Assert.IsNotNull(item.SimplePersonName);
                Assert.IsNotNull(item.ValueParent);
                Assert.IsNotNull(item.ValueParent?.Child);
            }

            Console.WriteLine("\n\n=== SET NEW VALUES ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.SimpleAddress = new SimpleAddress("New","New","New","New","New");
                item.SimplePersonName = new SimplePersonName("New","New","New");
                item.PhoneNumber = new PhoneNumber("9999999999");
                item.ValueParent = new ValueParent("New", new ValueChild("New"));

                db.SaveChanges();
            }

            Console.WriteLine("\n\n=== READBACK ENTITY ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.ConsoleDump();
                Assert.IsNotNull(item.SimpleAddress);
                Assert.IsNotNull(item.PhoneNumber);
                Assert.IsNotNull(item.SimplePersonName);
                Assert.IsNotNull(item.ValueParent);
                Assert.IsNotNull(item.ValueParent?.Child);

                Assert.AreEqual("New", item.SimpleAddress.Line1);
                Assert.AreEqual("9999999999", item.PhoneNumber.Value);
                Assert.AreEqual("New", item.SimplePersonName.First);
                Assert.AreEqual("New", item.ValueParent.ParentValue);
                Assert.AreEqual("New", item.ValueParent.Child.ChildValue);
            }

            Console.WriteLine("\n\n=== SET NULLS ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.SimpleAddress = null;
                item.SimplePersonName = null;
                item.PhoneNumber = null;
                item.ValueParent = null;

                db.SaveChanges();
            }

            Console.WriteLine("\n\n=== READBACK DTO ===");

            using (var db = new SampleDataContext())
            {
                var dto = db.ValObjHolders
                    .Where(p => p.Id == voh.Id.Guid)
                    .ProjectToDto<ValObjHolderDto>();

                dto.ConsoleDump();
                Assert.IsNull(dto.SimpleAddress);
                Assert.IsNull(dto.PhoneNumber);
                Assert.IsNull(dto.SimplePersonName);
                Assert.IsNull(dto.ValueParent);
                Assert.IsNull(dto.ValueParent?.Child);
            }
        }

        [TestMethod]
        public void NestedValueObjectsWithNullValues()
        {
            var voh = new ValObjHolder()
            {
                Name = Guid.NewGuid().ToString(),
                SimpleAddress = null,
                PhoneNumber = null,
                SimplePersonName = null,
                ValueParent = null,
            };

            voh.ConsoleDump();
            Assert.IsNull(voh.SimpleAddress);
            Assert.IsNull(voh.PhoneNumber);
            Assert.IsNull(voh.SimplePersonName);
            Assert.IsNull(voh.ValueParent);
            Assert.IsNull(voh.ValueParent?.Child);

            Console.WriteLine("\n\n=== ENTITY TO DB ===");

            using (var db = new SampleDataContext())
            {
                var dbvoh = DbEntity.ToDb<DbValObjHolder>(voh);

                dbvoh.ConsoleDump();
                Assert.IsNotNull(dbvoh.SimpleAddress);
                Assert.IsNotNull(dbvoh.PhoneNumber);
                Assert.IsNotNull(dbvoh.SimplePersonName);
                Assert.IsNotNull(dbvoh.ValueParent);
                Assert.IsNotNull(dbvoh.ValueParent?.Child);

                db.ValObjHolders.Add(dbvoh);

                db.SaveChanges();
            }

            Console.WriteLine("\n\n=== READBACK DTO ===");

            using (var db = new SampleDataContext())
            {
                var dto = db.ValObjHolders
                    .Where(p => p.Id == voh.Id.Guid)
                    .ProjectToDto<ValObjHolderDto>();

                dto.ConsoleDump();
                Assert.IsNull(dto.SimpleAddress);
                Assert.IsNull(dto.PhoneNumber);
                Assert.IsNull(dto.SimplePersonName);
                Assert.IsNull(dto.ValueParent);
                Assert.IsNull(dto.ValueParent?.Child);
            }

            Console.WriteLine("\n\n=== READBACK ENTITY ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.ConsoleDump();
                Assert.IsNull(item.SimpleAddress);
                Assert.IsNull(item.PhoneNumber);
                Assert.IsNull(item.SimplePersonName);
                Assert.IsNull(item.ValueParent);
                Assert.IsNull(item.ValueParent?.Child);
            }
        }

        [TestMethod]
        public void NestedValueObjectsWithNullChildValues()
        {
            var voh = new ValObjHolder()
            {
                Name = Guid.NewGuid().ToString(),
                SimpleAddress = new SimpleAddress("4034 Winecott Drive", "", "Apex", "NC", "27502"),
                PhoneNumber = new PhoneNumber("9198675309"),
                SimplePersonName = new SimplePersonName("Test", "Test", "Test"),
                ValueParent = new ValueParent("Parent", null),
            };

            voh.ConsoleDump();
            Assert.IsNotNull(voh.SimpleAddress);
            Assert.IsNotNull(voh.PhoneNumber);
            Assert.IsNotNull(voh.SimplePersonName);
            Assert.IsNotNull(voh.ValueParent);
            Assert.IsNull(voh.ValueParent.Child);

            Console.WriteLine("\n\n=== ENTITY TO DB ===");

            using (var db = new SampleDataContext())
            {
                var dbvoh = DbEntity.ToDb<DbValObjHolder>(voh);

                dbvoh.ConsoleDump();
                Assert.IsNotNull(dbvoh.SimpleAddress);
                Assert.IsNotNull(dbvoh.PhoneNumber);
                Assert.IsNotNull(dbvoh.SimplePersonName);
                Assert.IsNotNull(dbvoh.ValueParent);
                Assert.IsNotNull(dbvoh.ValueParent?.Child);

                db.ValObjHolders.Add(dbvoh);

                db.SaveChanges();
            }

            Console.WriteLine("\n\n=== READBACK DTO ===");

            using (var db = new SampleDataContext())
            {
                var dto = db.ValObjHolders
                    .Where(p => p.Id == voh.Id.Guid)
                    .ProjectToDto<ValObjHolderDto>();

                dto.ConsoleDump();
                Assert.IsNotNull(dto.SimpleAddress);
                Assert.IsNotNull(dto.PhoneNumber);
                Assert.IsNotNull(dto.SimplePersonName);
                Assert.IsNotNull(dto.ValueParent);
                Assert.IsNull(dto.ValueParent.Child);
            }

            Console.WriteLine("\n\n=== READBACK ENTITY ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.ConsoleDump();
                Assert.IsNotNull(item.SimpleAddress);
                Assert.IsNotNull(item.PhoneNumber);
                Assert.IsNotNull(item.SimplePersonName);
                Assert.IsNotNull(item.ValueParent);
                Assert.IsNull(item.ValueParent.Child);
            }
        }
    }
}