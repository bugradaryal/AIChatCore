using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options): base(options)
        {
        }

        public DbSet<AiMessage> aiMessages { get; set; }
        public DbSet<UserMessage> userMessages { get; set; }
        public DbSet<Logs> logs { get; set; }
        public DbSet<Session> sessions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AiMessage>().HasKey(x => x.id);
            modelBuilder.Entity<AiMessage>().Property(x => x.ai_message).HasColumnType("text");
            modelBuilder.Entity<AiMessage>().Property(x => x.ai_message).HasDefaultValue("No Response!");
            modelBuilder.Entity<AiMessage>().Property(x => x.ai_message_date).HasColumnType("timestamptz");
            modelBuilder.Entity<AiMessage>().Property(x => x.ai_message_date).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AiMessage>().Property(x => x.UserMessageId).IsRequired();
            //---------------------------------------------------------------------------------
            modelBuilder.Entity<UserMessage>().HasKey(x => x.id);
            modelBuilder.Entity<UserMessage>().Property(x => x.user_message).HasColumnType("text");
            modelBuilder.Entity<UserMessage>().Property(x => x.user_message).IsRequired();
            modelBuilder.Entity<UserMessage>().Property(x=>x.user_message_date).HasColumnType("timestamptz");
            modelBuilder.Entity<UserMessage>().Property(x => x.user_message_date).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //---------------------------------------------------------------------------------
            modelBuilder.Entity<Logs>().HasKey(x => x.id);
            modelBuilder.Entity<Logs>().Property(x => x.ip_adress).HasColumnType("inet");
            modelBuilder.Entity<Logs>().Property(x => x.date).HasColumnType("timestamptz");
            modelBuilder.Entity<Logs>().Property(x => x.date).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Logs>().Property(x => x.prop).HasColumnType("text");
            modelBuilder.Entity<Logs>().Property(x => x.prop).HasDefaultValue("Unknown");
            //---------------------------------------------------------------------------------
            modelBuilder.Entity<Session>().HasKey(x => x.id);
            modelBuilder.Entity<Session>().Property(x => x.title).HasColumnType("text");
            modelBuilder.Entity<Session>().Property(x => x.created_date).HasColumnType("timestamptz");
            modelBuilder.Entity<Session>().Property(x => x.created_date).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //---------------------------------------------------------------------------------
            modelBuilder.Entity<UserMessage>()
                .HasOne(x => x.session)
                .WithMany()
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserMessage>()
                .HasOne(x => x.aiMessage)
                .WithOne(x => x.userMessage)
                .HasForeignKey<AiMessage>(x => x.UserMessageId);

            modelBuilder.Entity<Logs>()
                .HasOne<UserMessage>()
                .WithMany()
                .HasForeignKey(x => x.UserMessageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Logs>()
                .HasOne<AiMessage>()
                .WithMany()
                .HasForeignKey(x => x.AiMessageId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
