using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Customer;
using Newtonsoft.Json;

namespace RedundancyTests
{
    [TestClass]
    public class GenerateLoadTest
    {
        private const string SERVICE_URL = "http://xxx.trafficmanager.net/"; // Traffic Manager
        private const string EAST_SERVICE_URL = "http://xxx-east.cloudapp.net/"; //EAST
        private const string WEST_SERVICE_URL = "http://xxx-west.cloudapp.net/"; //WEST
        private const int TOTAL_RECORDS = 100;
        private const double SECONDS_TO_WAIT = 0.1;

        [TestMethod]
        [TestCategory("Load")]
        public void CreateNewUsersTest()
        {
            int successCount = 0;
            int errorCount = 0;
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"..\..\..\", "TestFiles");

            string currentFile = Path.Combine(path, "customer_without_ids2.json");

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(currentFile));

            foreach (Customer customer in customers)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SERVICE_URL);
                    using (HttpContent content = new StringContent(JsonConvert.SerializeObject(customer)))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PostAsync("customers", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResult = response.Content.ReadAsStringAsync().Result;
                            Customer customerResult = JsonConvert.DeserializeObject<Customer>(jsonResult);
                            try
                            {
                                Customer checkCustomer = JsonConvert.DeserializeObject<Customer>(client.GetStringAsync($"customers/{customerResult.CustomerId}").Result);
                                if (checkCustomer == null)
                                    errorCount += 1;
                                else
                                    successCount += 1;
                            }
                            catch
                            {
                                errorCount += 1;
                            }
                        }
                        else
                            errorCount += 1;
                    }

                    if ((successCount + errorCount) == TOTAL_RECORDS)
                        break;
                }
            }

            Assert.IsTrue(errorCount == 0, "There were errors.");
        }

        [TestMethod]
        [TestCategory("Load")]
        [Ignore]
        public void CreateNewUsersAndRetrieveInAnotherRegionTest()
        {
            int successCount = 0;
            int errorCount = 0;
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"..\..\..\", "TestFiles");

            string currentFile = Path.Combine(path, "customer_without_ids3.json");

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(currentFile));

            foreach (Customer customer in customers)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(EAST_SERVICE_URL);
                    using (HttpContent content = new StringContent(JsonConvert.SerializeObject(customer)))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PostAsync("customers", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResult = response.Content.ReadAsStringAsync().Result;
                            Customer customerResult = JsonConvert.DeserializeObject<Customer>(jsonResult);

                            Thread.Sleep(TimeSpan.FromSeconds(SECONDS_TO_WAIT));
                            try
                            {
                                using (HttpClient client2 = new HttpClient())
                                {
                                    client2.BaseAddress = new Uri(WEST_SERVICE_URL);
                                    Customer checkCustomer = JsonConvert.DeserializeObject<Customer>(client2.GetStringAsync($"customers/{customerResult.CustomerId}").Result);
                                    if (checkCustomer == null)
                                        errorCount += 1;
                                    else
                                        successCount += 1;
                                }
                            }
                            catch
                            {
                                errorCount += 1;
                            }
                        }
                        else
                            errorCount += 1;
                    }

                    if ((successCount + errorCount) == TOTAL_RECORDS)
                        break;
                }
            }

            Assert.IsTrue(errorCount == 0, "There were errors.");
        }

        [TestMethod]
        [TestCategory("Load")]
        public void CreateNewUsersWithPutTest()
        {
            int successCount = 0;
            int errorCount = 0;
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"..\..\..\", "TestFiles");
            string currentFile = Path.Combine(path, "customer_without_phone1.json");

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(currentFile));
            foreach (Customer customer in customers)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SERVICE_URL);

                    using (HttpContent content = new StringContent(JsonConvert.SerializeObject(customer)))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PutAsync($"customers/{customer.CustomerId}", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                Customer checkCustomer = JsonConvert.DeserializeObject<Customer>(client.GetStringAsync($"customers/{customer.CustomerId}").Result);
                                if (checkCustomer == null)
                                    errorCount += 1;
                                else
                                    successCount += 1;
                            }
                            catch
                            {
                                errorCount += 1;
                            }
                        }
                        else
                            errorCount += 1;
                    }

                    if ((successCount + errorCount) == TOTAL_RECORDS)
                        break;
                }
            }
        }

        [TestMethod]
        [TestCategory("Load")]
        public void DeleteUsersWithPutTest()
        {
            int successCount = 0;
            int errorCount = 0;
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"..\..\..\", "TestFiles");
            string currentFile = Path.Combine(path, "customer_with_phone1.json");

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(currentFile));
            foreach (Customer customer in customers)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SERVICE_URL);

                    HttpResponseMessage response = client.DeleteAsync($"customers/{customer.CustomerId}").Result;
                    if (response.IsSuccessStatusCode)
                        successCount += 1;
                    else
                        errorCount += 1;

                    if ((successCount + errorCount) == TOTAL_RECORDS)
                        break;
                }
            }
        }

        [TestMethod]
        [TestCategory("Load")]
        public void UpdateUsersWithPutTest()
        {
            int successCount = 0;
            int errorCount = 0;
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"..\..\..\", "TestFiles");
            string currentFile = Path.Combine(path, "customer_with_phone1.json");

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(currentFile));
            foreach (Customer customer in customers)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SERVICE_URL);

                    using (HttpContent content = new StringContent(JsonConvert.SerializeObject(customer)))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PutAsync($"customers/{customer.CustomerId}", content).Result;
                        if (response.IsSuccessStatusCode)
                            successCount += 1;
                        else
                            errorCount += 1;
                    }

                    if ((successCount + errorCount) == TOTAL_RECORDS)
                        break;
                }
            }
        }
    }
}