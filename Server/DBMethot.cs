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
        public async Task<bool> UpdateUserAsync(string name, long? telegramId, int? count, decimal? zarp)
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
                var existingSettings = await _context.TelegramSettings.FirstOrDefaultAsync(s => s.Id == 1); // Ищем запись с Id = 1

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
        public async Task<bool> UpdateTelegramSettingAsync(string settingKey, string settingValue)
        {
            var setting = await _context.TelegramSettings.FirstOrDefaultAsync(s => s.Id == 1); // Например, ищем по Id

            if (setting == null)
            {
                return false; // Если запись не найдена, возвращаем false
            }

            // Обновляем соответствующее поле
            switch (settingKey)
            {
                case "TokenBot":
                    setting.TokenBot = settingValue;
                    break;
                case "ForwardChat":
                    setting.ForwardChat = long.TryParse(settingValue, out var forwardChat) ? forwardChat : 0;
                    break;
                case "ChatId":
                    setting.ChatId = long.TryParse(settingValue, out var chatId) ? chatId : 0;
                    break;
                case "TraidSmena":
                    setting.TraidSmena = int.TryParse(settingValue, out var traidSmena) ? traidSmena : 0;
                    break;
                case "TreidShtraph":
                    setting.TreidShtraph = int.TryParse(settingValue, out var treidShtraph) ? treidShtraph : 0;
                    break;
                case "TraidRashod":
                    setting.TraidRashod = decimal.TryParse(settingValue, out var traidRashod) ? traidRashod : 0m;
                    break;
                case "TraidPostavka":
                    setting.TraidPostavka = int.TryParse(settingValue, out var traidPostavka) ? traidPostavka : 0;
                    break;
                case "Password":
                    setting.Password = !string.IsNullOrEmpty(settingValue) ? settingValue : "";
                    break;
                default:
                    return false; // Если ключ не распознан
            }

            try
            {
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                return true; // Обновление прошло успешно
            }
            catch (Exception ex)
            {
                // Логируем полное сообщение об ошибке, включая внутреннее исключение
                Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}, Внутреннее исключение: {ex.InnerException?.Message}");
                return false; // Возвращаем false, если сохранение не удалось
            }
        }

        public async Task InsertTelegramSettingAsync(string settingKey, string settingValue)
        {
            var setting = new TelegramSettings
            {
                TokenBot = settingKey == "TokenBot" ? settingValue : null,
                ForwardChat = settingKey == "ForwardChat" ? (long.TryParse(settingValue, out var forwardChat) ? forwardChat : 0) : (long)0,
                ChatId = settingKey == "ChatId" ? (long.TryParse(settingValue, out var chatId) ? chatId : 0) : (long)0,
                TraidSmena = settingKey == "TraidSmena" ? (int.TryParse(settingValue, out var traidSmena) ? traidSmena : 0) : (int)0,
                TreidShtraph = settingKey == "TreidShtraph" ? (int.TryParse(settingValue, out var treidShtraph) ? treidShtraph : 0) : (int)0,
                TraidRashod = settingKey == "TraidRashod" ? (decimal.TryParse(settingValue, out var traidRashod) ? traidRashod : 0m) : (decimal)0,
                TraidPostavka = settingKey == "TraidPostavka" ? (int.TryParse(settingValue, out var traidPostavka) ? traidPostavka : 0) : (int)0,
            };

            // Сохраняем настройку в базе данных
            _context.TelegramSettings.Add(setting);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> InsertNewTelegramSettingAsync(string tokenBot, long forwardChat, long chatId, int traidSmena, int treidShtraph, decimal traidRashod, int traidPostavka, string password)
        {
            try
            {
                // Создаем объект настроек Telegram для добавления в базу данных
                var newSetting = new TelegramSettings
                {
                    TokenBot = tokenBot,
                    ForwardChat = forwardChat,
                    ChatId = chatId,
                    TraidSmena = traidSmena,
                    TreidShtraph = treidShtraph,
                    TraidRashod = traidRashod,
                    TraidPostavka = traidPostavka,
                    Password = password
                };

                // Добавляем новую запись в таблицу и сохраняем изменения
                _context.TelegramSettings.Add(newSetting);
                await _context.SaveChangesAsync();

                return true; // Успешно добавлено
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении новых настроек: {ex.Message}");
                return false; // Возвращаем false, если что-то пошло не так
            }
        }
    }
}