using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class DBMethod(MyDbContext context)
    {
        private readonly MyDbContext _context = context;
        // Общий метод для обработки ошибок
        private static async Task<bool> SafeExecuteAsync(Func<Task> action)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}, Внутреннее исключение: {ex.InnerException?.Message}");
                return false;
            }
        }
        // Работа с сотруднками
        // Действие со списком сотрудников
        // Общий метод для добавления или обновления сотрудника
        private async Task AddOrUpdateUserAsync(User user)
        {
            if (user.UserId == 0) // Если новый пользователь
            {
                await _context.Users.AddAsync(user);
            }
            await _context.SaveChangesAsync();
        }
        // Метод для добавления сотрудника
        public async Task<bool> AddUserAsync(string name, long telegramId, int count, int zarp)
        {
            var newUser = new User
            {
                Name = name,
                TelegramId = telegramId,
                Count = count,
                Zarp = zarp
            };
            return await SafeExecuteAsync(() => AddOrUpdateUserAsync(newUser));
        }
        // Метод для редактирования сотрудника
        public async Task<bool> UpdateUserAsync(string name, long? telegramId, int? count, int? zarp)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null) return false;

            // Обновляем данные только если они были переданы
            if (telegramId.HasValue) user.TelegramId = telegramId.Value;
            if (count.HasValue) user.Count = count.Value;
            if (zarp.HasValue) user.Zarp = zarp.Value;

            return await SafeExecuteAsync(() => AddOrUpdateUserAsync(user));
        }
        // Метод для архивации сотрудника
        public async Task<bool> ArchiveUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsArchived = true; // Устанавливаем флаг "архивирован"
            await _context.SaveChangesAsync();
            return true;
        }
        // Разархивация пользователя
        public async Task<bool> ReArchiveUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !user.IsArchived) return false;

            user.IsArchived = false; // Убираем флаг "архивирован"
            await _context.SaveChangesAsync();
            return true;
        }
        // Работа с зарплатами сотрудников
        // Метод для получения зарплаты сотрудника по имени
        public async Task<Salary?> GetSalaryByNameAsync(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null) return null;

            return await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == user.UserId);
        }
        // Метод для получения истории зарплат по ID сотрудника
        public async Task<List<SalaryHistory>> GetSalaryHistoryByEmployeeIdAsync(int employeeId)
        {
            return await _context.SalaryHistory.Where(s => s.UserId == employeeId).ToListAsync();
        }
        // Метод для обновления зарплаты сотрудника
        public async Task<bool> UpdateSalaryByIdAsync(int userId, decimal zpChange)
        {
            // Получаем зарплату пользователя
            var salary = await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == userId);

            if (salary == null)
            {
                // Если зарплата не существует, создаем новую запись
                salary = new Salary
                {
                    UserId = userId,
                    TotalSalary = 0
                };
                _context.Salaries.Add(salary);
            }

            // Обновляем зарплату
            salary.TotalSalary += zpChange;

            // Добавляем изменение зарплаты в историю изменений
            var salaryChange = new SalaryChange
            {
                UserId = userId,  // Привязка к UserId
                ChangeAmount = zpChange,
                ChangeDate = DateTime.Now
            };

            _context.SalaryChanges.Add(salaryChange);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<SalaryChange>> GetSalaryChangesByUserIdAsync(int userId)
        {
            return await _context.SalaryChanges
                                 .Where(sc => sc.UserId == userId)  // Привязка к UserId
                                 .ToListAsync();
        }
        // Пересчёт зарплат и добавление их в историю
        public async Task FinalizeSalariesAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var allSalaries = await _context.Salaries.Include(s => s.SalaryChanges).ToListAsync();

                foreach (var salary in allSalaries)
                {
                    var salaryHistory = new SalaryHistory
                    {
                        UserId = salary.UserId,
                        TotalSalary = salary.TotalSalary,
                        FinalizedDate = DateTime.Now,
                        IsPaid = false
                    };

                    _context.SalaryHistory.Add(salaryHistory);
                }

                // Обнуляем актуальные зарплаты после финализации
                _context.Salaries.RemoveRange(allSalaries);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Ошибка при финализации зарплат: {ex.Message}");
            }
        }
        // Работа с сейфом
        // Метод для получения текущей суммы в сейфе
        public async Task<int?> GetSafeAmountAsync()
        {
            var safe = await _context.Safe.FirstOrDefaultAsync();
            return safe?.TotalAmount;
        }
        // Метод для обновления суммы в сейфе
        public async Task<bool> UpdateSafeAmountAsync(int amountChange)
        {
            var safe = await _context.Safe.FirstOrDefaultAsync()
                       ?? new Safe { TotalAmount = 0 };

            safe.TotalAmount += amountChange;

            var safeChange = new SafeChange
            {
                ChangeAmount = amountChange,
                ChangeDate = DateTime.Now
            };

            return await SafeExecuteAsync(async () =>
            {
                if (safe.Id == 0) _context.Safe.Add(safe);
                _context.SafeChanges.Add(safeChange);
                await _context.SaveChangesAsync();
            });
        }
        // Сокращение записи сейфа до одной записи
        public async Task FinalizeSafeChangesAsync()
        {
            try
            {
                // Получаем текущую сумму сейфа
                var currentSafe = await _context.Safe.FirstOrDefaultAsync() ?? throw new Exception("Текущая сумма сейфа не найдена.");

                // Получаем все изменения в сейфе
                var safeChanges = await _context.SafeChanges.ToListAsync();

                // Очищаем таблицу SafeChanges
                _context.SafeChanges.RemoveRange(safeChanges);

                // Добавляем актуальную запись о сейфе обратно в SafeChange
                var currentSafeChange = new SafeChange
                {
                    ChangeAmount = currentSafe.TotalAmount,
                    ChangeDate = DateTime.Now
                };
                _context.SafeChanges.Add(currentSafeChange);

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
                Console.WriteLine("Изменения сейфа успешно финализированы.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при финализации изменений сейфа: {ex.Message}");
            }
        }
        // Работа с телеграм настройками
        // Метод для обновления настроек Telegram
        public async Task<bool> UpdateTelegramSettingsAsync(TelegramSettings settings)
        {
            var existingSettings = await _context.TelegramSettings.FirstOrDefaultAsync();

            if (existingSettings == null)
            {
                existingSettings = new TelegramSettings();
                _context.TelegramSettings.Add(existingSettings);
            }

            existingSettings.TokenBot = settings.TokenBot;
            existingSettings.ForwardChat = settings.ForwardChat;
            existingSettings.ChatId = settings.ChatId;
            existingSettings.TraidSmena = settings.TraidSmena;
            existingSettings.TreidShtraph = settings.TreidShtraph;
            existingSettings.TraidRashod = settings.TraidRashod;
            existingSettings.TraidPostavka = settings.TraidPostavka;
            existingSettings.Password = settings.Password;

            return await SafeExecuteAsync(() => _context.SaveChangesAsync());
        }
    }
}