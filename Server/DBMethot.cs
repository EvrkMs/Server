using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        // Общий метод для добавления или обновления сотрудника
        private async Task AddOrUpdateUserAsync(User user)
        {
            if (user.Id == 0) // Если новый пользователь
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

            var archivedUser = new ArchivedUser
            {
                Name = user.Name,
                TelegramId = user.TelegramId,
                Count = user.Count,
                Zarp = user.Zarp,
                ArchivedDate = DateTime.UtcNow
            };

            return await SafeExecuteAsync(async () =>
            {
                await _context.ArchivedUsers.AddAsync(archivedUser);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            });
        }
        // Метод для получения зарплаты сотрудника по имени
        public async Task<Salary?> GetSalaryByNameAsync(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null) return null;

            return await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == user.Id);
        }
        // Метод для получения истории зарплат по ID сотрудника
        public async Task<List<SalaryHistory>> GetSalaryHistoryByEmployeeIdAsync(int employeeId)
        {
            return await _context.SalaryHistory.Where(s => s.UserId == employeeId).ToListAsync();
        }
        // Метод для обновления зарплаты сотрудника
        public async Task<bool> UpdateSalaryByIdAsync(int employeeId, int zpChange)
        {
            // Ищем сотрудника по ID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == employeeId);
            if (user == null) return false;

            // Проверяем, существует ли запись о зарплате сотрудника
            var salary = await _context.Salaries.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (salary == null)
            {
                // Если запись о зарплате отсутствует, создаем новую
                salary = new Salary { UserId = user.Id, TotalSalary = 0 };
                await _context.Salaries.AddAsync(salary);
                await _context.SaveChangesAsync(); // Сохраняем новую запись, чтобы получить её ID
            }

            // Обновляем общую зарплату
            salary.TotalSalary += zpChange;

            // Добавляем запись в таблицу изменений зарплаты (SalaryChanges)
            var salaryChange = new SalaryChange
            {
                SalaryId = salary.Id, // Убедись, что SalaryId существует
                ChangeAmount = zpChange,
                ChangeDate = DateTime.Now
            };
            await _context.SalaryChanges.AddAsync(salaryChange);

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();
            return true;
        }
        // Пересчёт зарплат и добавление их в историю
        public async Task FinalizeSalariesAsync()
        {
            var allSalaries = await _context.Salaries.ToListAsync();
            await SafeExecuteAsync(async () =>
            {
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

                _context.Salaries.RemoveRange(allSalaries);
                await _context.SaveChangesAsync();
            });
        }
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
        public async Task FinalizeSafeChangesAsync()
        {
            try
            {
                // Получаем текущую сумму сейфа
                var currentSafe = await _context.Safe.FirstOrDefaultAsync() ?? throw new Exception("Текущая сумма сейфа не найдена.");

                // Получаем все изменения в сейфе
                var safeChanges = await _context.SafeChanges.ToListAsync();
                if (safeChanges.Count == 0)
                {
                    throw new Exception("Изменений в сейфе не найдено.");
                }

                // Копируем изменения в таблицу SafeChangeHistory
                foreach (var change in safeChanges)
                {
                    var historyEntry = new SafeChangeHistory
                    {
                        ChangeAmount = change.ChangeAmount,
                        ChangeDate = change.ChangeDate
                    };
                    _context.SafeChangeHistory.Add(historyEntry);
                }

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
    }
}