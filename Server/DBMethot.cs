// DBMethod.cs
using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class DBMethod(MyDbContext context)
    {
        private readonly MyDbContext _context = context;

        // Метод для обновления зарплаты сотрудника
        public async Task<bool> UpdateSalaryAsync(string name, decimal zpChange)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null) return false;

            var salary = await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (salary == null)
            {
                salary = new Salary { UserId = user.Id, TotalSalary = 0 };
                _context.Salaries.Add(salary);
            }

            salary.TotalSalary += zpChange;

            var salaryChange = new SalaryChange
            {
                SalaryId = salary.Id,
                ChangeAmount = zpChange,
                ChangeDate = DateTime.Now
            };
            _context.SalaryChanges.Add(salaryChange);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddUserAsync(string name, long telegramId, int count, decimal zarp)
        {
            try
            {
                // Создаём нового сотрудника
                var newUser = new User
                {
                    Name = name,
                    TelegramId = telegramId,
                    Count = count,
                    Zarp = zarp
                };

                // Добавляем сотрудника в базу данных
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Логируем ошибку, если добавление не удалось
                Console.WriteLine($"Ошибка при добавлении сотрудника: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSafeAmountAsync(decimal amountChange)
        {
            // Получаем текущую сумму в сейфе
            var safe = await _context.Safe.FirstOrDefaultAsync();
            if (safe == null)
            {
                // Если сейф ещё не создан, создаём его с начальной суммой 0
                safe = new Safe { TotalAmount = 0 };
                _context.Safe.Add(safe);
            }

            // Обновляем сумму в сейфе
            safe.TotalAmount += amountChange;

            // Добавляем запись в историю изменений
            var safeChange = new SafeChange
            {
                ChangeAmount = amountChange,
                ChangeDate = DateTime.Now
            };
            _context.SafeChanges.Add(safeChange);

            // Сохраняем изменения
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ArchiveUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            // Добавляем сотрудника в архив
            var archivedUser = new ArchivedUser
            {
                Id = user.Id,
                Name = user.Name,
                TelegramId = user.TelegramId,
                Count = user.Count,
                Zarp = user.Zarp,
                ArchivedDate = DateTime.Now
            };
            _context.ArchivedUsers.Add(archivedUser);

            // Удаляем сотрудника из активных пользователей
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task FinalizeSalariesAsync()
        {
            // Получаем всех сотрудников с их зарплатами
            var allSalaries = await _context.Salaries.ToListAsync();

            // Переносим данные о зарплатах в историю
            foreach (var salary in allSalaries)
            {
                var salaryHistory = new SalaryHistory
                {
                    UserId = salary.UserId,
                    TotalSalary = salary.TotalSalary,
                    FinalizedDate = DateTime.Now
                };

                _context.SalaryHistory.Add(salaryHistory);
            }

            // Удаляем все записи из таблицы зарплат (очищаем её)
            _context.Salaries.RemoveRange(allSalaries);

            // Сохраняем изменения
            await _context.SaveChangesAsync();
        }

        // Метод для получения текущей суммы в сейфе
        public async Task<decimal?> GetSafeAmountAsync()
        {
            var safe = await _context.Safe.FirstOrDefaultAsync();
            return safe?.TotalAmount;
        }

        // Метод для получения зарплаты сотрудника по имени
        public async Task<Salary> GetSalaryByNameAsync(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null) return null;

            var salary = await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == user.Id);
            return salary;
        }
    }
}