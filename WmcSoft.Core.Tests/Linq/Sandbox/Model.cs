using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Linq.Sandbox
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class Northwind
    {
        public readonly Queryable<Customer> Customers;
        public readonly Queryable<Order> Orders;

        public Northwind(IDbConnection connection) {
            var provider = new DbQueryProvider(connection);
            Customers = new Queryable<Customer>(provider);
            Orders = new Queryable<Order>(provider);
        }
    }
}
