using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CriticalSectionApp
{
    private readonly object _lockObject = new object(); // Объект для блокировки
    private int[] _data;

    public CriticalSectionApp(int[] data)
    {
        _data = data;
    }

    public void Run(int numberToFind)
    {
        // Первый поток: сортировка массива
        Thread sortThread = new Thread(() => SortArray());
        sortThread.Start();

        // Второй поток: проверка наличия числа
        Thread searchThread = new Thread(() => SearchNumber(numberToFind));
        searchThread.Start();

        // Ожидание завершения обоих потоков (не обязательно, но полезно для демонстрации)
        sortThread.Join();
        searchThread.Join();

        Console.WriteLine("Оба потока завершены.");
    }

    private void SortArray()
    {
        lock (_lockObject) // Блокировка критической секции
        {
            try
            {
                Array.Sort(_data);
                Console.WriteLine("Массив отсортирован.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сортировке: {ex.Message}");
            }
        }
    }

    private void SearchNumber(int numberToFind)
    {
        // Ожидание завершения сортировки (можно заменить на другие механизмы синхронизации)
        Thread.Sleep(100); // Небольшая задержка, чтобы дать первому потоку время отсортировать массив. Не идеально, но работает для демонстрации. В реальном коде лучше использовать Event или другие методы синхронизации.


        lock (_lockObject) // Блокировка критической секции (не нужна, если сортировка уже завершена и мы только читаем данные)
        {
            try
            {
                bool found = _data.Contains(numberToFind);
                Console.WriteLine($"Число {numberToFind} {(found ? "найдено" : "не найдено")} в массиве.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            }
        }
    }


    public static void Main(string[] args)
    {
        int[] data = { 5, 2, 8, 1, 9, 4, 7, 3, 6 };
        int numberToFind = 7;

        CriticalSectionApp app = new CriticalSectionApp(data);
        app.Run(numberToFind);
    }
}
