using System.Collections;

class Program
{

    public static async Task Main(string[] args)
    {

            ArrayList changesLog = new ArrayList();  
            MediaItem previousItem = null;  

            bool isFirstTime = false;         

            while (true)
            {

                    List<MediaItem> currentData = GenerateDummyData(10);

                    MediaItem currentItem = currentData[0];

                    if (previousItem != null)
                    {


                        if (previousItem.Id != currentItem.Id)
                        {
                            
                            Console.WriteLine("Change detected based on Id!");

                            changesLog.Add(currentItem);  

                        }
                        else
                        {
                            Console.WriteLine("No changes detected.");
                        }


                    }

                    previousItem = currentItem;

                    Console.WriteLine($"Total changes logged: {changesLog.Count}");

                    await Task.Delay(5000);

            }
                
                
            List<MediaItem> dummyData = GenerateDummyData(10);


            var tasks = new Task[dummyData.Count];


            for (int j = 0; j < dummyData.Count; j++)
            {

                    tasks[j] = Task.Run(async () =>
                    {


                            string result =  await test();

                            Console.WriteLine(result);



                            bool b = await boolean();  

                            Console.WriteLine(b);  



                            int i = await integer();

                            Console.WriteLine(i);


                    });                
            }

            await Task.WhenAll(tasks);



    }


    private static async Task<string> test()
    {
        await Task.Delay(1000);

        return "hello word";

    }





    private static async Task<bool> boolean()
    {
        await Task.Delay(1000);

        string[] lengths = new string[] {"hello","a string","two string"};

        var tasks = new Task[lengths.Length];

        for (int i = 0; i < tasks.Length; i++)
        {
            
            tasks[i] = Task.Run(async () =>
            {

                Console.WriteLine("multi thread 1");

                await Task.Delay(1000);

                Console.WriteLine("mult thread 2");

            });

        }
       
        await Task.WhenAll(tasks);
    
        return true;

    }




    private static async Task<int> integer()
    {
        await Task.Delay(1000);
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
                Status = i % 2 == 0 ? "failure" : "success", 
                Hd = i % 2 == 0, 
                DeletedAt = i % 2 == 0 ? (DateTime?)DateTime.Now : null, 
                DeletedBy = i % 2 == 0 ? "admin" : null, 
                Id = Guid.NewGuid().ToString() 
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