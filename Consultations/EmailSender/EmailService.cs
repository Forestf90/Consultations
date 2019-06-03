﻿using Consultations.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Consultations.EmailSender
{
    public class EmailService : IHostedService
    {

        private readonly IServiceScopeFactory scopeFactory;
        private EmailReminder _emailReminder;

        public EmailService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            _emailReminder = new EmailReminder();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(TaskRoutine, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Sync Task stopped");
            return null;
        }

        public async Task TaskRoutine()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
               

                while (true)
                {
                    var tommorow = DateTime.Now;
                    tommorow.AddDays(-1);
                    // sendEmail();
                    var consultation = dbContext.Consultations
                        .GroupJoin(dbContext.UserConsultation, e => e.Id, r => r.ConsultationId, (e, r) =>
                         new {
                             Date = e.Date,
                             Room =e.Room,
                             Emails = r.Select(o => o.User.Email),
                 })
                .Where(x=>x.Date<tommorow);

                    foreach( var consult in consultation)
                    {
                        //var emails = consult.AppUsers.Select(n => n.User.Email);
                        foreach(var em in consult.Emails)
                        {
                            await _emailReminder.SendEmailAsync(em, "Nadchodzące konsultacje", 
                                consult.Date.ToString("g")+" bierzesz udzial w konsultacjach w sali numer "+ consult.Room);
                        }
                    }

                   // await emailSender.SendEmailAsync(consultation.AppUsers.FirstOrDefault().User.Email, "tak", "tersc meila");
                    //Wait 10 minutes till next execution
                    DateTime nextStop = DateTime.Now.AddMinutes(10);
                    var timeToWait = nextStop - DateTime.Now;
                    var millisToWait = timeToWait.TotalMilliseconds;
                    Thread.Sleep((int)millisToWait);
                }
            }
        }
    }
}
