using Model.Data;
using Model.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.Manager
{
    #region Customer
    public class CustomerManager
    {
        public static List<Customer> GetCustomers()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<Customer> Customers = entities.Customers.ToList();
                return Customers;
            }
        }
        public static ResponseViewModel<List<GetCustomerById_Result>> GetCustomersById(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<List<GetCustomerById_Result>> response = new ResponseViewModel<List<GetCustomerById_Result>>();
                List<GetCustomerById_Result> result = entities.GetCustomerById(id).ToList();
                response.Data = result;
                return response;
            }
        }

        public static ResponseViewModel<List<ValidateUser_Result>> ValidateCustomer(string username,string password)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<List<ValidateUser_Result>> response = new ResponseViewModel<List<ValidateUser_Result>>();
                List<ValidateUser_Result> result = entities.ValidateUser(username, password).ToList();
                if (result.Count > 0 )
                {
                    User user = entities.Users.Where(entry => entry.Username == username && entry.Password == password).FirstOrDefault();
                    user.LastLoginTime = DateTime.UtcNow;
                    response.Data = result;
                    return response;
                }
                else
                {
                    response.errorViewModel = new ErrorViewModel();
                    response.errorViewModel.statusCode = 300;
                    return response;
                }
            }
        }

        public static ResponseViewModel<Customer> RegisterCustomer()
        {
            ResponseViewModel<Customer> responseViewModel = new ResponseViewModel<Customer>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                string imageName = null;
                HttpRequest httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files["Image"];
                imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
                postedFile.SaveAs(filePath);
                CustomerViewModel viewModel = JsonConvert.DeserializeObject<CustomerViewModel>(httpRequest["customerDetails"]);
                if (CheckDuplicate(viewModel))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 100;
                }
                else
                {
                    Customer customer = new Customer();
                    customer.FirstName = viewModel.firstName;
                    customer.LastName = viewModel.lastName;
                    customer.EmailId = viewModel.emailId;
                    customer.City = viewModel.city;
                    customer.State = viewModel.state;
                    customer.Street = viewModel.street;
                    customer.ZipCode = viewModel.zipCode;
                    customer.Image = imageName;
                    customer.PhoneNumber = viewModel.PhoneNumber;
                    User user = new User();
                    user.Password =  Guid.NewGuid().ToString();
                    user.RoleId = 2;
                    user.Username = viewModel.emailId;
                    user.Password = viewModel.password;
                    user.LastLoginTime = null;
                    entities.Users.Add(user);
                    Cart cart = new Cart();
                    entities.Carts.Add(cart);
                    entities.SaveChanges();
                    customer.CartID = cart.ID;
                    customer.UserID = user.ID;
                    entities.Customers.Add(customer);
                    entities.SaveChanges();
                    responseViewModel.Data = customer;
                }
            }

            return responseViewModel;
        }

        public static ResponseViewModel<Customer> UpdateCustomerProfile()
        {
            ResponseViewModel<Customer> responseViewModel = new ResponseViewModel<Customer>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                string imageName = null;
                HttpRequest httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files["Image"];
                imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
                postedFile.SaveAs(filePath);
                CustomerViewModel viewModel = JsonConvert.DeserializeObject<CustomerViewModel>(httpRequest["customerDetails"]);
               // int customerId = Convert.ToInt32(JsonConvert.DeserializeObject(httpRequest["customerId"]));
                if (CheckDuplicateCustomer(viewModel,viewModel.Id))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 100;
                }
                else
                {
                    Customer customer = entities.Customers.Where(entry => entry.ID == viewModel.Id).FirstOrDefault();
                    customer.FirstName = viewModel.firstName;
                    customer.LastName = viewModel.lastName;
                    //customer.EmailId = viewModel.emailId;
                    customer.City = viewModel.city;
                    customer.State = viewModel.state;
                    customer.Street = viewModel.street;
                    customer.ZipCode = viewModel.zipCode;
                    customer.PhoneNumber = viewModel.PhoneNumber;
                    customer.Image = imageName;
                    //User user = entities.Users.Where(entry => entry.Username == customer.EmailId).FirstOrDefault();
                    //user.Password = viewModel.password;
                    entities.SaveChanges();
                    responseViewModel.Data = customer;
                }
            }

            return responseViewModel;
        }

        public static ResponseViewModel<User> SendEmail(string emailId,string password,Boolean registrationFlag=false)
        {
            ResponseViewModel<User> responseViewModel = new ResponseViewModel<User>();
            using (EcommerceEntities entites = new EcommerceEntities())
            {
                if((entites.Users.Any(entry => entry.Username == emailId)) || registrationFlag)
                {
                    var senderEmail = new MailAddress("sanketrawade11@gmail.com", "sanket");
                    var receiverEmail = new MailAddress(emailId, "Receiver");
                    var senderPassword = "";
                    var sub = "";
                    var body = "";
                    if (registrationFlag == true)
                    {
                        sub = "Registration of Ecommerce";
                        body = "\nThanks for registration :) " + "\n" + " Your password is " + password;
                    }
                    else
                    {
                        sub = "Password reset Succesfully.";
                        body = "Your new password is  " + password;
                    }
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, senderPassword)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    User user = entites.Users.Where(entry => entry.Username == emailId).FirstOrDefault();
                    responseViewModel.Data = user;
                }
                else
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 300;
                }
                return responseViewModel;
            }
        }



        public static User UpdateLastLogin(string emailId)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                User user = entities.Users.Where(entry => entry.Username == emailId).FirstOrDefault();
                user.LastLoginTime = null;
                entities.SaveChanges();
                return user;
            }
        }


        public static ResponseViewModel<User> ChangePassword(PasswordViewModel viewModel)
        {
            ResponseViewModel<User> responseViewModel = new ResponseViewModel<User>();
            using(EcommerceEntities entities = new EcommerceEntities())
            {
                if (viewModel.oldPassword != null)
                {
                    User user = entities.Users.Where(entry => entry.ID == viewModel.id).FirstOrDefault();
                    if (user.Password == viewModel.oldPassword)
                    {
                        user.Password = viewModel.newPassword;
                        user.LastLoginTime = DateTime.Now;
                        responseViewModel.Data = user;
                        entities.SaveChanges();
                    }
                    else
                    {
                        responseViewModel.errorViewModel = new ErrorViewModel();
                        responseViewModel.errorViewModel.statusCode = 300;
                    }
                    return responseViewModel;
                }
                else
                {
                    User user = entities.Users.Where(entry => entry.Username == viewModel.emailId).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = viewModel.newPassword;
                        responseViewModel.Data = user;
                        entities.SaveChanges();
                    }
                    else
                    {
                        responseViewModel.errorViewModel = new ErrorViewModel();
                        responseViewModel.errorViewModel.statusCode = 300;
                    }
                    return responseViewModel;
                }
            }
        }

        /*public static ResponseViewModel<User> ChangePasswordToRandom(PasswordViewModel viewModel)
        {
            ResponseViewModel<User> responseViewModel = new ResponseViewModel<User>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                User user = entities.Users.Where(entry => entry.Username == viewModel.emailId).FirstOrDefault();
                if (user != null)
                {
                    user.Password = viewModel.newPassword;
                    responseViewModel.Data = user;
                    entities.SaveChanges();
                }
                else
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 300;
                }
                return responseViewModel;
            }
        }*/



        public static Boolean CheckDuplicateCustomer(CustomerViewModel viewModel,int userId)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (entities.Customers.Any(entry => entry.PhoneNumber == viewModel.PhoneNumber && entry.ID != userId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Boolean CheckDuplicate(CustomerViewModel viewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (entities.Customers.Any(entry => entry.EmailId == viewModel.emailId && entry.PhoneNumber == viewModel.PhoneNumber))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void AddCustomer(Customer customer)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                entities.Customers.Add(customer);
                entities.SaveChanges();
            }
        }
        public static void DeleteCustomer(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Customer customer = entities.Customers.Where(entry => entry.ID == id).FirstOrDefault();
                entities.Customers.Remove(customer);
                entities.SaveChanges();
            }
        }
    }
    #endregion
}
