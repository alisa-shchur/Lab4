using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;

namespace Lab4
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public List<Client> GetClients()
        {
            List<Client> list = new List<Client>();
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(str))
            {
                SqlCommand com = new SqlCommand("Select * from Clients", con);
                con.Open();
                SqlDataReader r = com.ExecuteReader();
                while (r.Read())
                {
                    Client c = new Client();
                    c.Id = (int)r["Id"];
                    c.LastNameClient = r["LastNameClient"].ToString();
                    c.FirstNameClient = r["FirstNameClient"].ToString();
                    c.Phone = r["Phone"].ToString();
                    c.Email = r["Email"].ToString();
                    c.Adress = r["Adress"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public List<Order> GetOrders(int Id)
        {
            List<Order> list = new List<Order>();
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(str))
            {
                SqlCommand com = new SqlCommand("Select * from Orders where IdClient=@Id", con);
                com.Parameters.Add(new SqlParameter("@Id", Id));
                con.Open();
                SqlDataReader r = com.ExecuteReader();
                while (r.Read())
                {
                    Order or = new Order();
                    or.Id = (int)r["Id"];
                    or.IdClient = (int)r["IdClient"];
                    or.Description = r["Description"].ToString();
                    or.Price = (int)r["Price"];
                    list.Add(or);
                }
            }
            return list;
        }
    }
}
