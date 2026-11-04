using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("--Працюємо із масивом файлів--");
string url = "https://i.pravatar.cc/1200";
int count = 0;

Console.WriteLine("Вкажіть кількість фото:");
count = int.Parse(Console.ReadLine());

string dir; // ім'я папки для збереження фото
Console.WriteLine("Вкажіть назву папки:");
dir = Console.ReadLine();

Directory.CreateDirectory(dir);

using (HttpClient myClient = new HttpClient())
{
    for (int i = 0; i < count; i++) // качаю певну кількість фото
    {
        try
        {
            byte[] bytes = myClient.GetByteArrayAsync(url).Result;

            // Guid -- унікальна назва файлу
            string fileName = Guid.NewGuid().ToString() + ".jpg"; // Додаємо розширення для зручності

            // path -- для роботи із шляхами у файловій системі
            string fileSavePath = Path.Combine(dir, fileName);

            File.WriteAllBytes(fileSavePath, bytes); // Зберігаємо у папку
            Console.WriteLine($"Збережено {i + 1} Файл успішно :)");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Сталася помилка " + ex.Message);
        }
    }
}

// Хочу провести аналіз папки
string[] files = Directory.GetFiles(dir);

Console.WriteLine("\nСписок файлів, які ви скачали:");
string reportPath = "reports.txt";

// Використовуємо StreamWriter з параметром append = true для додавання записів
using (StreamWriter sw = new StreamWriter(reportPath, true, Encoding.UTF8))
{
    
    sw.WriteLine($"--- ЗВІТ РОБОТИ ПРОГРАМИ ВІД {DateTime.Now:dd.MM.yyyy HH:mm:ss} ---");
    sw.WriteLine($"Папка: {dir}");
    sw.WriteLine($"Назва файлу\tРозмір\tДата створення");

    foreach (var file in files)
    {
        FileInfo fileInfo = new FileInfo(file);
        long sizeInBytes = fileInfo.Length;
        string fileName = Path.GetFileName(file);
        DateTime fileCreated = fileInfo.CreationTime;
        string strDateCreate = fileCreated.ToString("dd.MM.yyyy HH:mm:ss");
        string reportLine = $"{fileName}\t{sizeInBytes / 1024} Кб.\t{strDateCreate}";
        Console.WriteLine(reportLine);
        sw.WriteLine(reportLine);
    }

    sw.WriteLine(new string('-', 50) + "\n"); // Розділювач для наступних запусків
}

Console.WriteLine($"\nЗвіт успішно збережено/додано у файл: {Path.GetFullPath(reportPath)}");
