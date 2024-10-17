using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TelegramSettings> TelegramSettings { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryChange> SalaryChanges { get; set; }
        public DbSet<Safe> Safe { get; set; }
        public DbSet<SafeChange> SafeChanges { get; set; }
        public DbSet<SalaryHistory> SalaryHistory { get; set; }
        public DbSet<ArchivedUser> ArchivedUsers { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public decimal Zarp { get; set; }
    }

    public class TelegramSettings
    {
        public int Id { get; set; }
        public string TokenBot { get; set; }
        public long ForwardChat { get; set; }
        public long ChatId { get; set; }
        public long PhotoChat { get; set; }
        public int TraidSmena { get; set; }
        public int TreidShtraph { get; set; }
        public decimal TraidRashod { get; set; }
        public int TraidPostavka { get; set; }
        public int TraidPhoto { get; set; }
        public string Password { get; set; }
    }

    public class Salary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalSalary { get; set; }
        public bool IsArchived { get; set; } = false;  // Новый флаг для архивирования

        // Связь с историей изменений
        public List<SalaryChange> SalaryChanges { get; set; }

        public User User { get; set; }
    }

    public class SalaryChange
    {
        public int Id { get; set; }
        public int SalaryId { get; set; }
        public decimal ChangeAmount { get; set; }
        public DateTime ChangeDate { get; set; }

        public Salary Salary { get; set; }
    }

    // Модель для таблицы Safe
    public class Safe
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
    }

    // Модель для таблицы SafeChanges
    public class SafeChange
    {
        public int Id { get; set; }
        public decimal ChangeAmount { get; set; }
        public DateTime ChangeDate { get; set; }
    }
    // Новая модель для истории зарплат
    public class SalaryHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime FinalizedDate { get; set; }

        // Связь с пользователем
        public User User { get; set; }
    }
    public class ArchivedUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public decimal Zarp { get; set; }
        public DateTime ArchivedDate { get; set; }
    }
}