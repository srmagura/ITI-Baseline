﻿using System;
using System.Linq;
using AppConfig;
using CoreTests.Helpers;
using DataContext;
using Domain;
using Iti.Auth;
using Iti.Core.DataContext;
using Iti.Core.DomainEvents;
using Iti.Core.DTOs;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;
using Iti.ValueObjects;
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
                Address = new Address("4034 Winecott Drive", "", "Apex", "NC", "27502"),
                PhoneNumber = new PhoneNumber("9198675309"),
                PersonName = new PersonName("Test", "Test", "Test"),
                ValueParent = new ValueParent("Parent", new ValueChild("Child")),
            };

            voh.ConsoleDump();
            Assert.IsNotNull(voh.Address);
            Assert.IsNotNull(voh.PhoneNumber);
            Assert.IsNotNull(voh.PersonName);
            Assert.IsNotNull(voh.ValueParent);
            Assert.IsNotNull(voh.ValueParent?.Child);

            Console.WriteLine("\n\n=== ENTITY TO DB ===");

            using (var db = new SampleDataContext())
            {
                var dbvoh = DbEntity.ToDb<DbValObjHolder>(voh);

                dbvoh.ConsoleDump();
                Assert.IsNotNull(dbvoh.Address);
                Assert.IsNotNull(dbvoh.PhoneNumber);
                Assert.IsNotNull(dbvoh.PersonName);
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
                Assert.IsNotNull(dto.Address);
                Assert.IsNotNull(dto.PhoneNumber);
                Assert.IsNotNull(dto.PersonName);
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
                Assert.IsNotNull(item.Address);
                Assert.IsNotNull(item.PhoneNumber);
                Assert.IsNotNull(item.PersonName);
                Assert.IsNotNull(item.ValueParent);
                Assert.IsNotNull(item.ValueParent?.Child);
            }

            Console.WriteLine("\n\n=== SET NEW VALUES ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.Address = new Address("New","New","New","New","New");
                item.PersonName = new PersonName("New","New","New");
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
                Assert.IsNotNull(item.Address);
                Assert.IsNotNull(item.PhoneNumber);
                Assert.IsNotNull(item.PersonName);
                Assert.IsNotNull(item.ValueParent);
                Assert.IsNotNull(item.ValueParent?.Child);

                Assert.AreEqual("New", item.Address.Line1);
                Assert.AreEqual("9999999999", item.PhoneNumber.Value);
                Assert.AreEqual("New", item.PersonName.First);
                Assert.AreEqual("New", item.ValueParent.ParentValue);
                Assert.AreEqual("New", item.ValueParent.Child.ChildValue);
            }

            Console.WriteLine("\n\n=== SET NULLS ===");

            using (var db = new SampleDataContext())
            {
                var item = db.ValObjHolders
                    .FirstOrDefault(p => p.Id == voh.Id.Guid)
                    ?.ToEntity<ValObjHolder>();

                item.Address = null;
                item.PersonName = null;
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
                Assert.IsNull(dto.Address);
                Assert.IsNull(dto.PhoneNumber);
                Assert.IsNull(dto.PersonName);
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
                Address = null,
                PhoneNumber = null,
                PersonName = null,
                ValueParent = null,
            };

            voh.ConsoleDump();
            Assert.IsNull(voh.Address);
            Assert.IsNull(voh.PhoneNumber);
            Assert.IsNull(voh.PersonName);
            Assert.IsNull(voh.ValueParent);
            Assert.IsNull(voh.ValueParent?.Child);

            Console.WriteLine("\n\n=== ENTITY TO DB ===");

            using (var db = new SampleDataContext())
            {
                var dbvoh = DbEntity.ToDb<DbValObjHolder>(voh);

                dbvoh.ConsoleDump();
                Assert.IsNotNull(dbvoh.Address);
                Assert.IsNotNull(dbvoh.PhoneNumber);
                Assert.IsNotNull(dbvoh.PersonName);
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
                Assert.IsNull(dto.Address);
                Assert.IsNull(dto.PhoneNumber);
                Assert.IsNull(dto.PersonName);
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
                Assert.IsNull(item.Address);
                Assert.IsNull(item.PhoneNumber);
                Assert.IsNull(item.PersonName);
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
                Address = new Address("4034 Winecott Drive", "", "Apex", "NC", "27502"),
                PhoneNumber = new PhoneNumber("9198675309"),
                PersonName = new PersonName("Test", "Test", "Test"),
                ValueParent = new ValueParent("Parent", null),
            };

            voh.ConsoleDump();
            Assert.IsNotNull(voh.Address);
            Assert.IsNotNull(voh.PhoneNumber);
            Assert.IsNotNull(voh.PersonName);
            Assert.IsNotNull(voh.ValueParent);
            Assert.IsNull(voh.ValueParent.Child);

            Console.WriteLine("\n\n=== ENTITY TO DB ===");

            using (var db = new SampleDataContext())
            {
                var dbvoh = DbEntity.ToDb<DbValObjHolder>(voh);

                dbvoh.ConsoleDump();
                Assert.IsNotNull(dbvoh.Address);
                Assert.IsNotNull(dbvoh.PhoneNumber);
                Assert.IsNotNull(dbvoh.PersonName);
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
                Assert.IsNotNull(dto.Address);
                Assert.IsNotNull(dto.PhoneNumber);
                Assert.IsNotNull(dto.PersonName);
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
                Assert.IsNotNull(item.Address);
                Assert.IsNotNull(item.PhoneNumber);
                Assert.IsNotNull(item.PersonName);
                Assert.IsNotNull(item.ValueParent);
                Assert.IsNull(item.ValueParent.Child);
            }
        }
    }
}