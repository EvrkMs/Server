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
        public async Task<bool> AddUserAsync(string name, long telegramId, int count, int zarp)
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
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Создаём объект для архивирования без явной установки Id
            var archivedUser = new ArchivedUser
            {
                Name = user.Name,
                TelegramId = user.TelegramId,
                Count = user.Count,
                Zarp = user.Zarp,
                ArchivedDate = DateTime.UtcNow
            };

            // Добавляем в таблицу ArchivedUsers
            await _context.ArchivedUsers.AddAsync(archivedUser);

            // Удаляем пользователя из таблицы Users
            _context.Users.Remove(user);

            // Сохраняем изменения
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
        public async Task<bool> UpdateUserAsync(string name, long? telegramId, int? count, int? zarp)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null) return false;

            // Обновляем данные только если они были переданы
            if (telegramId.HasValue)
            {
                user.TelegramId = telegramId.Value;
            }

            if (count.HasValue)
            {
                user.Count = count.Value;
            }

            if (zarp.HasValue)
            {
                user.Zarp = zarp.Value;
            }

            await _context.SaveChangesAsync();
            return true;
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

        public async Task<Salary> GetCurrentSalaryByEmployeeIdAsync(int employeeId)
        {
            return await _context.Salaries
                                 .Include(s => s.SalaryChanges)
                                 .FirstOrDefaultAsync(s => s.UserId == employeeId);
        }

        // Метод для получения истории зарплат по ID сотрудника
        public async Task<List<SalaryHistory>> GetSalaryHistoryByEmployeeIdAsync(int employeeId)
        {
            return await _context.SalaryHistory
                                 .Where(s => s.UserId == employeeId)
                                 .ToListAsync();
        }

        // Метод для добавления истории зарплат после финализации периода
        public async Task AddSalaryHistoryAsync(int employeeId, decimal totalSalary)
        {
            var salaryHistory = new SalaryHistory
            {
                UserId = employeeId,
                TotalSalary = totalSalary,
                FinalizedDate = DateTime.UtcNow
            };

            await _context.SalaryHistory.AddAsync(salaryHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateTelegramSettingsAsync(TelegramSettings settings)
        {
            try
            {
                var existingSettings = await _context.TelegramSettings.FirstOrDefaultAsync();

                if (existingSettings == null)
                {
                    // Если запись не найдена, создаём новую
                    Console.WriteLine("Настройки с Id = 1 не найдены. Создаём новую запись.");

                    var newSettings = new TelegramSettings
                    {
                        TokenBot = settings.TokenBot,
                        ForwardChat = settings.ForwardChat,
                        ChatId = settings.ChatId,
                        TraidSmena = settings.TraidSmena,
                        TreidShtraph = settings.TreidShtraph,
                        TraidRashod = settings.TraidRashod,
                        TraidPostavka = settings.TraidPostavka,
                        Password = settings.Password
                    };

                    await _context.TelegramSettings.AddAsync(newSettings);
                }
                else
                {
                    // Обновляем существующие поля
                    existingSettings.TokenBot = settings.TokenBot;
                    existingSettings.ForwardChat = settings.ForwardChat;
                    existingSettings.ChatId = settings.ChatId;
                    existingSettings.TraidSmena = settings.TraidSmena;
                    existingSettings.TreidShtraph = settings.TreidShtraph;
                    existingSettings.TraidRashod = settings.TraidRashod;
                    existingSettings.TraidPostavka = settings.TraidPostavka;
                    existingSettings.Password = settings.Password;

                    Console.WriteLine("Настройки с Id = 1 обновлены.");
                }

                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                Console.WriteLine("Настройки успешно сохранены.");
                return true;
            }
            catch (Exception ex)
            {
                // Логируем полное сообщение об ошибке, включая внутреннее исключение
                Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}, Внутреннее исключение: {ex.InnerException?.Message}");
                return false; // Возвращаем false, если сохранение не удалось
            }
        }
    }
}