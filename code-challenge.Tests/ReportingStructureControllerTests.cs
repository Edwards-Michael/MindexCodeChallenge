using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetEmployeeReportingStructure_Returns_Ok()
        {
            // Arrange
            String employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            int expectedNumberOfDirectReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedNumberOfDirectReports, reportingStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetEmployeeReportingStructureWhoReportsToThemseleves_Returns_None()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;
            var newEmployee = response.DeserializeContent<Employee>();
            List<Employee> directReports = new List<Employee>();
            directReports.Add(new Employee() { EmployeeId = newEmployee.EmployeeId });
            newEmployee.DirectReports = directReports;

            requestContent = new JsonSerialization().ToJson(employee);

            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            response = putRequestTask.Result;

            String employeeId = newEmployee.EmployeeId;
            int expectedNumberOfDirectReports = 0;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            // Assert
            Assert.IsNotNull(newEmployee.EmployeeId);
            Assert.AreEqual(expectedNumberOfDirectReports, reportingStructure.NumberOfReports);
        }
    }
}
