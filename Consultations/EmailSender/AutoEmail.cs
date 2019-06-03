using Consultations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.EmailSender
{ 
    public interface IEMailService
    {
        //void CheckConsultation();
    }

    internal sealed class AutoEmail : IEMailService
    {
        private static IEmailSender _emailSender;
        private static ApplicationDbContext _context;

        private static  IServiceProvider m_ServiceProvider;


        public AutoEmail(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            m_ServiceProvider = serviceProvider;

            var serviceScope = m_ServiceProvider.CreateScope();
            _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            _emailSender = serviceScope.ServiceProvider.GetService<EmailReminder>();
        }

        public static async void CheckConsultation()
        {
            while (true)
            {
                var consultations = _context.Consultations;

                var consultation = _context.Consultations
                .GroupJoin(_context.UserConsultation, e => e.Id, r => r.ConsultationId, (e, r) =>
                 new
                 {
                     Date = e.Date,
                     Room = e.Room,
                     //  AppStudents = r.Select(t => t.User.Pesel),
                     Emails = r.Select(o => o.User.Email),
                     //  Id = e.Id
                 });
                DateTime dt = DateTime.Now;
                dt.AddDays(-1);
                 foreach(var con in consultation)
                {
                    if (con.Date > dt)
                    {
                        foreach(var em in con.Emails)
                        {
                            await _emailSender.SendEmailAsync(em, "subject",
                        $"Enter email body here");
                        }
                       
                    }
                }
            }


        }

    }
}
