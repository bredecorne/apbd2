using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository = new ClientRepository();
        private readonly IUserCreditService _userCreditService = new UserCreditService();
        private readonly IUserDataAccessAdapter _userDataAccessAdapter = new UserDataAccessAdapter();

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUser(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }
            
            var client = _clientRepository.GetById(clientId);

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            CalculateCreditLimit(user, client);

            if (!ValidateCreditLimit(user))
            {
                return false;
            }

            _userDataAccessAdapter.AddUser(user);
            return true;
        }
        
        private bool ValidateUser(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }
            
            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }
            
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }
        
        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }
        
        private void CalculateCreditLimit(User user, Client client)
        {
            var creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                user.HasCreditLimit = true;
                creditLimit *= 2;
                user.CreditLimit = creditLimit;
            }
            else
            {
                user.HasCreditLimit = true;
                user.CreditLimit = creditLimit;
            }
        }
        
        private bool ValidateCreditLimit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }
    }
}