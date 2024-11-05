using System.Collections;
using System.Collections.Concurrent;

class Program
{
    private static ConcurrentQueue<MediaItem> taskQueue = new ConcurrentQueue<MediaItem>();
    private static HashSet<string> queuedItems = new HashSet<string>(); 

    private static bool processingComplete = false;

    public static async Task Main(string[] args)
    {
       ArrayList changesLog = new ArrayList();

        MediaItem previousItem = null;

        var cancellationTokenSource = new CancellationTokenSource();

        Task processorTask = Task.Run(() => ProcessQueue(cancellationTokenSource.Token));

        List<MediaItem> dummyData = GenerateDummyData(2);

        Task foreachTask = Task.Run(() =>
        {
            foreach (var item in dummyData)
            {
                if (queuedItems.Add(item.Id))
                {
                    taskQueue.Enqueue(item);
                }
            }
        });


        Task whileTask = Task.Run(async () =>
        {
            Console.WriteLine("hello while");
            

            while (!processingComplete && !taskQueue.IsEmpty)
            {
                List<MediaItem> processingDataWhileTranscoding = GenerateDummyData(1);

                Console.WriteLine("dummy while");

                MediaItem newItem = processingDataWhileTranscoding.First();

                if (!dummyData.Any(pdata => pdata.Id == newItem.Id) && queuedItems.Add(newItem.Id))
                {
                    Console.WriteLine("Ma tu buu");
                    taskQueue.Enqueue(newItem);
                }

                await Task.Delay(5000);  
            }
        });


        await foreachTask;

        await processorTask;  

        processingComplete = true;

        await whileTask;

        Console.WriteLine("Both foreach and while loops have finished, and processing is complete.");

    }

    private static async Task ProcessQueue(CancellationToken token)
    {
        while (!token.IsCancellationRequested || !taskQueue.IsEmpty)
        {
            if (taskQueue.TryDequeue(out MediaItem item))
            {
                await ProcessItem(item);
                queuedItems.Remove(item.Id);
            }
            else
            {
                await Task.Delay(500); 
            }
        }
        Console.WriteLine("Processing complete. All items have been processed.");
    }


    private static async Task ProcessItem(MediaItem item)
    {
        Console.WriteLine($"Processing item with Id: {item.Id}");

        string result = await test();
        Console.WriteLine(result);

        bool b = await boolean();
        Console.WriteLine(b);

        int i = await integer();
        Console.WriteLine(i);

        Console.WriteLine($"Completed processing for item Id: {item.Id}");
    }

    private static async Task<string> test()
    {
        Console.WriteLine("hello world");
        await Task.Delay(1000);
        return "hello world";
    }

    private static async Task<bool> boolean()
    {
        await Task.Delay(1000);
        string[] lengths = new string[] { "hello", "a string", "two string" };

        var tasks = new Task[lengths.Length];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                Console.WriteLine("multi thread");
                await Task.Delay(1000);
            });
        }
        await Task.WhenAll(tasks);
        return true;
    }

    private static async Task<int> integer()
    {
        await Task.Delay(1000);
        Console.WriteLine("1");
        return 1;
    }

    private static List<MediaItem> GenerateDummyData(int numberOfItems)
    {
        var dummyData = new List<MediaItem>();
        Random random = new Random();

        for (int i = 0; i < numberOfItems; i++)
        {
            dummyData.Add(new MediaItem
            {
                Number = i,
                Name = i % 2 == 0 ? "tiger.mp4" : "lion.mp4",
                Type = "movie",
                Status = i % 2 == 0 ? "processing" : "success",
                Hd = i % 2 == 0,
                DeletedAt = i % 2 == 0 ? (DateTime?)DateTime.Now : null,
                DeletedBy = i % 2 == 0 ? "admin" : null,
                Id = "e89cbec7-f4ce-4a59-a2af-5dff77c6ec76",
            });
        }

        return dummyData;
    }

    public class MediaItem
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public bool Hd { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public string Id { get; set; }
    }
}
