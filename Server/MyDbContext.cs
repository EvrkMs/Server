using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
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
                .HasForeignKey(sc => sc.UserId) // Внешний ключ SalaryChange ссылается на Salary
                .OnDelete(DeleteBehavior.Cascade); // При удалении Salary, изменения удаляются

            // Связь между User и SalaryHistory
            modelBuilder.Entity<SalaryHistory>()
                .HasOne<User>() // Один User может иметь много записей истории зарплат
                .WithMany() // У User может быть много записей SalaryHistory
                .HasForeignKey(sh => sh.UserId) // Внешний ключ SalaryHistory ссылается на User
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление

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
        public int UserId { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }
        // Флаг для архивирования
        public bool IsArchived { get; set; } = false; // По умолчанию пользователь не архивирован
        public virtual Salary Salary { get; set; } // Связь с таблицей Salary
    }

    public class Salary
    {
        [Key]
        [ForeignKey("User")]  // Привязка к таблице Users
        public int UserId { get; set; }

        public decimal TotalSalary { get; set; }  // Текущая сумма зарплаты

        public virtual List<SalaryChange> SalaryChanges { get; set; }  // Связь с изменениями зарплаты
    }

    public class SalaryChange
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }  // Привязка к таблице Users (или ArchivedUsers)

        public decimal ChangeAmount { get; set; }  // Сумма изменения (прибавка/вычитание)

        public DateTime ChangeDate { get; set; }  // Дата изменения
    }
    public class SalaryHistory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }  // Привязка к пользователю

        public decimal TotalSalary { get; set; }  // Итоговая зарплата после пересчёта

        public DateTime FinalizedDate { get; set; }  // Дата финализации зарплаты

        public bool IsPaid { get; set; } = false;  // Статус выплаты (по умолчанию false)
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