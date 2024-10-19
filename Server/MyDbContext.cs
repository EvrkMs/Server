using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TelegramSettings> TelegramSettings { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryChange> SalaryChanges { get; set; }
        public DbSet<Safe> Safe { get; set; }
        public DbSet<SafeChange> SafeChanges { get; set; }
        public DbSet<SalaryHistory> SalaryHistory { get; set; }
        public DbSet<ArchivedUser> ArchivedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Устанавливаем связь между пользователем и зарплатой
            modelBuilder.Entity<Salary>()
                .HasOne(s => s.User) // Связываем с таблицей Users
                .WithOne() // Один к одному
                .HasForeignKey<Salary>(s => s.UserId) // Внешний ключ
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление, чтобы зарплата не удалялась

            // Устанавливаем значение по умолчанию для зарплаты
            modelBuilder.Entity<Salary>()
                .Property(s => s.TotalSalary)
                .HasDefaultValue(0); // Зарплата по умолчанию 0

            base.OnModelCreating(modelBuilder);
        }

        // Добавляем пользователя вместе с записью о зарплате
        public async Task AddUserAsync(User newUser)
        {
            using var transaction = await Database.BeginTransactionAsync();

            // Добавляем нового пользователя
            Users.Add(newUser);
            await SaveChangesAsync();

            // Создаем запись о зарплате
            var salary = new Salary
            {
                UserId = newUser.Id,
                TotalSalary = 0  // Зарплата по умолчанию
            };
            Salaries.Add(salary);
            await SaveChangesAsync();

            await transaction.CommitAsync();
        }

        // Архивирование пользователя
        public async Task ArchiveUserAsync(int userId)
        {
            var user = await Users.FindAsync(userId);
            if (user == null) throw new Exception("Пользователь не найден.");

            // Перемещаем пользователя в архив
            ArchivedUsers.Add(new ArchivedUser
            {
                Id = user.Id,
                Name = user.Name,
                TelegramId = user.TelegramId,
                Count = user.Count,
                Zarp = user.Zarp,
                ArchivedDate = DateTime.UtcNow
            });

            // Удаляем пользователя из основной таблицы
            Users.Remove(user);
            await SaveChangesAsync();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public int Count { get; set; }
        public int Zarp { get; set; }
    }

    public class TelegramSettings
    {
        public int Id { get; set; }
        public string TokenBot { get; set; }
        public long ForwardChat { get; set; }
        public long ChatId { get; set; }
        public int TraidSmena { get; set; }
        public int TreidShtraph { get; set; }
        public int TraidRashod { get; set; }
        public int TraidPostavka { get; set; }
        public string Password { get; set; }
    }

    public class Salary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalSalary { get; set; }
        public bool? IsArchived { get; set; } = false;  // Новый флаг для архивирования
        public User User { get; set; }

        // Связь с историей изменений
        public List<SalaryChange> SalaryChanges { get; set; }
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