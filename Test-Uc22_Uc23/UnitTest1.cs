using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Uc1_Uc2_AddressBookJSON37;

namespace Test_Uc22_Uc23
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000/employees");
        }

        public RestResponse getEmployeeList()
        {
            // Arrange
            // Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/addressBook", Method.Get);

            // Act
            // Execute the request
            RestResponse response = client.ExecuteAsync(request).Result;
            return response;
        }
        //UC 22
        [TestMethod]
        public void onCallingGETApi_ReturnAddressBookList()
        {
            RestResponse response = getEmployeeList();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);     // Comes from using System.Net namespace
            List<AddressBook> dataResponse = JsonConvert.DeserializeObject<List<AddressBook>>(response.Content);
            Assert.AreEqual(3, dataResponse.Count);

            foreach (AddressBook a in dataResponse)
            {
                System.Console.WriteLine("FirstName : " + a.firstName + " LastName : " + a.lastName + " PhoneNumber : " + a.phoneNumber + " Address : " + a.address + " City : " + a.city + " State : " + a.state + " Zip : " + a.zip + " email : " + a.email);
            }
        }

        //UC 23
        [TestMethod]
        public void OnCallingPostAPIForAContactListWithMultipleContacts_ReturnContactObject()
        {
            // Arrange
            List<AddressBook> contactList = new List<AddressBook>();
            contactList.Add(new AddressBook { firstName = "Aron", lastName = "Stone", phoneNumber = "1234567890", address = "Dholakpur", city = "Varanasi", state = "UP", zip = "229554", email = "ps@gmail.com" });
            contactList.Add(new AddressBook { firstName = "Vishal", lastName = "Seth", phoneNumber = "781654987", address = "Charashu Chauraha", city = "Jaunpur", state = "UP", zip = "442206", email = "vs@gmail.com" });
            contactList.Add(new AddressBook { firstName = "Ekta", lastName = "Kumbhare", phoneNumber = "7856239865", address = "Bajaj Nagar", city = "Pune", state = "Maharashtra", zip = "442203", email = "ek@gmail.com" });

            //Iterate the loop for each contact
            foreach (var v in contactList)
            {
                //Initialize the request for POST to add new contact
                RestRequest request = new RestRequest("/addressBook", Method.Post);
                request.RequestFormat = DataFormat.Json;

                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddBody(v);

                //Act
                RestResponse response = client.ExecuteAsync(request).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                AddressBook contact = JsonConvert.DeserializeObject<AddressBook>(response.Content);
                Assert.AreEqual(v.firstName, contact.firstName);
                Assert.AreEqual(v.lastName, contact.lastName);
                Assert.AreEqual(v.phoneNumber, contact.phoneNumber);
                Console.WriteLine(response.Content);
            }


        }
    }
}