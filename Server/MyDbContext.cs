using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ArchivedUser> ArchivedUsers { get; set; }
        public DbSet<TelegramSettings> TelegramSettings { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryChange> SalaryChanges { get; set; }
        public DbSet<SalaryHistory> SalaryHistory { get; set; }
        public DbSet<Safe> Safe { get; set; }
        public DbSet<SafeChange> SafeChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь один-к-одному между User и Salary
            modelBuilder.Entity<User>()
                .HasOne(u => u.Salary) // Один пользователь имеет одну зарплату
                .WithOne() // Не требуется обратная ссылка в Salary
                .HasForeignKey<Salary>(s => s.UserId) // Внешний ключ Salary ссылается на User
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление

            // Связь один-ко-многим между Salary и SalaryChange
            modelBuilder.Entity<Salary>()
                .HasMany(s => s.SalaryChanges) // Один Salary имеет много изменений
                .WithOne() // Каждое изменение связано с одним Salary
                .HasForeignKey(sc => sc.SalaryId) // Внешний ключ SalaryChange ссылается на Salary
                .OnDelete(DeleteBehavior.Cascade); // При удалении Salary, изменения удаляются

            // Связь между User и SalaryHistory
            modelBuilder.Entity<SalaryHistory>()
                .HasOne<User>() // Один User может иметь много записей истории зарплат
                .WithMany() // У User может быть много записей SalaryHistory
                .HasForeignKey(sh => sh.UserId) // Внешний ключ SalaryHistory ссылается на User
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление

            // Настройка архивации: связь между User и ArchivedUser по UserId
            modelBuilder.Entity<ArchivedUser>()
                .HasKey(au => au.UserId); // UserId остаётся неизменным для архивированных пользователей

            // Устанавливаем значение по умолчанию для TotalSalary
            modelBuilder.Entity<Salary>()
                .Property(s => s.TotalSalary)
                .HasDefaultValue(0); // Зарплата по умолчанию 0

            // Отключаем каскадное удаление для TelegramSettings
            modelBuilder.Entity<TelegramSettings>()
                .HasKey(ts => ts.Id); // Уникальный ключ для настроек Telegram

            base.OnModelCreating(modelBuilder);
        }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; } // Автоинкрементируемый первичный ключ
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }

        public virtual Salary Salary { get; set; } // Связь с таблицей Salary
    }

    public class ArchivedUser
    {
        [Key]
        public int UserId { get; set; } // Идентификатор пользователя остаётся неизменным
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }
        public DateTime ArchivedDate { get; set; }
    }

    public class Salary
    {
        [Key]
        public int SalaryId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; } // Привязка к таблице Users

        public decimal TotalSalary { get; set; } // Текущая зарплата

        public virtual List<SalaryChange> SalaryChanges { get; set; } // Связь с изменениями зарплат
    }

    public class SalaryChange
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Salary")]
        public int SalaryId { get; set; } // Связь с таблицей Salary

        public int ChangeAmount { get; set; } // Изменение суммы
        public DateTime ChangeDate { get; set; } // Дата изменения
    }

    public class SalaryHistory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; } // Привязка к пользователю

        public decimal TotalSalary { get; set; } // Итоговая зарплата после пересчёта
        public DateTime FinalizedDate { get; set; } // Дата пересчёта
        public bool IsPaid { get; set; } = false; // Поле для отметки, выплачена ли зарплата
    }

    public class Safe
    {
        public int Id { get; set; }
        public int TotalAmount { get; set; } // Общая сумма в сейфе
    }

    public class SafeChange
    {
        public int Id { get; set; }
        public int ChangeAmount { get; set; } // Изменение суммы
        public DateTime ChangeDate { get; set; } // Дата изменения
    }

    public class TelegramSettings
    {
        public int Id { get; set; }
        public string? TokenBot { get; set; }
        public long ForwardChat { get; set; }
        public long ChatId { get; set; }
        public int TraidSmena { get; set; }
        public int TreidShtraph { get; set; }
        public int TraidRashod { get; set; }
        public int TraidPostavka { get; set; }
        public string? Password { get; set; }
    }
}