using Domain;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.ValueObjects;

namespace DataContext
{
    public class DbValObjHolder : DbEntity
    {
        public string Name { get; set; }

        public SimpleAddress SimpleAddress { get; set; }
        public SimplePersonName SimplePersonName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }

        public ValueParent ValueParent { get; set; }
    }
}