using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactApp1.Server.Models;
using ReactApp1.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace ReactApp1.Server.Data
{
    public class DataRandomizer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new UserDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<UserDBContext>>()))
            {

                if (context.Users.Any())
                {
                    return;
                }

                var faker = new Faker("pl");


                var users = new Faker<User>("pl")
                    .RuleFor(u => u.Name, f => f.Name.FullName())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .Generate(1);

                context.Users.AddRange(users);
                context.SaveChanges();

                var customers = new Faker<Customer>("pl")
                    .RuleFor(c => c.Name, f => f.Company.CompanyName())
                    .RuleFor(c => c.NIP, f => f.Random.Number(100000000, 999999999).ToString())
                    .RuleFor(c => c.PhoneNumber, f => f.Random.Int(100000000, 999999999))
                    .RuleFor(c => c.ContactPerson, f => f.Name.FullName())
                    .Generate(1);

                context.Customers.AddRange(customers);
                context.SaveChanges();

                var addresses = new Faker<Address>("pl")
                    .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                    .RuleFor(a => a.City, f => f.Address.City())
                    .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(a => a.Country, f => f.Address.Country())
                    .RuleFor(i => i.CustomerId, f => f.PickRandom(customers).Id)
                    .Generate(1);

                context.Address.AddRange(addresses);
                context.SaveChanges();

                var interventions = new Faker<Intervention>("pl")
                    .RuleFor(i => i.Description, f => f.Lorem.Sentence())
                    .RuleFor(i => i.Date, f => f.Date.Past(1))
                    .RuleFor(i => i.UserId, f => f.PickRandom(users).Id)
                    .RuleFor(i => i.AddressId, f => f.PickRandom(addresses).Id)
                    .RuleFor(i => i.CustomerId, f => f.PickRandom(customers).Id)
                    .Generate(1);

                context.Intervention.AddRange(interventions);
                context.SaveChanges();
            }
        }
    }
}