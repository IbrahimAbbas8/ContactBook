using ContactBook.Core.Entities;
using ContactBook.Core.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Data.Config
{
    public class SeedingData
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    FirstName = "Ibrahim",
                    LastName = "Abbas",
                    Email = "ibrahim@gmail.com",
                    UserName = "ibrahim@gmail.com",
                    Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            FirstName = "Ibrahim",
                            LastName = "Abbas",
                            Email = "ibrahim@gmail.com",
                            Email2 = "ibrahim123@gmail.com",
                            Address = "Azaz",
                            Address2 = "Edlib",
                            ContactPicture = "http://...",
                            PhoneNumber = "+905637006227",
                            MobileNumber = "+905637006227",
                        },
                        new Contact
                        {
                            FirstName = "Baraa",
                            LastName = "Abbas",
                            Email = "Baraa@gmail.com",
                            Email2 = "Baraa123@gmail.com",
                            Address = "Azaz",
                            Address2 = "Edlib",
                            ContactPicture = "http://...",
                            PhoneNumber = "+905637006227",
                            MobileNumber = "+905637006227",
                        },
                        new Contact
                        {
                            FirstName = "Jamal",
                            LastName = "Abbas",
                            Email = "Jamal@gmail.com",
                            Email2 = "Jamal123@gmail.com",
                            Address = "Azaz",
                            Address2 = "Edlib",
                            ContactPicture = "http://...",
                            PhoneNumber = "+905637006227",
                            MobileNumber = "+905637006227",
                        },
                    },
                    inviteUsers = new List<InviteUser>()
                    {
                        new InviteUser
                        {
                            FirstName = "Ibrahim",
                            LastName = "Abbas",
                            Email = "ibrahim@gmail.com",
                            PhoneNumber = "+905637006227",
                            StatusUser = StatusUser.Active,
                            UserType = UserType.Administrator,
                        },
                        new InviteUser
                        {
                            FirstName = "Baraa",
                            LastName = "Abbas",
                            Email = "Baraa@gmail.com",
                            PhoneNumber = "+905637006227",
                            StatusUser = StatusUser.Active,
                            UserType = UserType.Administrator,
                        },
                        new InviteUser
                        {
                            FirstName = "Jamal",
                            LastName = "Abbas",
                            Email = "Jamal@gmail.com",
                            PhoneNumber = "+905637006227",
                            StatusUser = StatusUser.Active,
                            UserType = UserType.Administrator,
                        },
                    },
                    Profile = new Profile
                    {
                        CompanyName = "MMM",
                        VatNumber = 0,
                        Street = "test street",
                        Street2 = "test street",
                        City = "Azaz",
                        State = "haram",
                        ZipCode = "123z",
                        Country = Country.Syria
                    }
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}
