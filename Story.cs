using System;
using System.Threading;
using System.Threading.Tasks;

class Story
{
    private bool storySkipped = false;

    // Menambahkan parameter Music untuk mengontrol pemutaran musik
    public void Display(Music music)
    {
        // Mulai musik latar belakang cerita
        music.PlayMusic("background_music.mp3");

        // Menangani tombol Enter untuk melewati cerita
        Task.Run(() =>
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                storySkipped = true;
            }
        });

        Console.Clear();
        Console.WriteLine("Press Enter to skip story...");
        PrintWithColor("\n\n3024", ConsoleColor.Blue);
        Console.WriteLine();
        SleepWithSkip(4000);

        DisplayParagraph("\nTeknologi virtual reality telah melampaui batas imajinasi manusia. " +
                          "\nHampir setiap rumah memiliki perangkat VR, dan bermain game dengan teknologi tersebut telah menjadi rutinitas sehari-hari. " +
                          "\nNamun, sebuah revolusi besar mengguncang dunia gaming—terobosan yang tidak hanya menjanjikan pengalaman, tetapi juga membuka pintu ke dunia baru.", 4000);

        DisplayParagraph("\n\nEXVision, perusahaan terdepan dalam industri VR, memperkenalkan Visionary, perangkat full-dive VR pertama di dunia. " +
                          "\nTidak seperti headset VR biasa, Visionary memungkinkan pengguna sepenuhnya masuk ke dunia virtual. " +
                          "\nTubuh fisik mereka tetap diam, sementara pikiran mereka bergerak bebas di dalam game.", 4000);

        DisplayParagraph("\n\nUntuk merayakan peluncuran ini, EXVision bekerja sama dengan Wibusoft, pengembang game terkenal, untuk menciptakan Elder Tale, " +
                          "\nsebuah game MMORPG yang digadang-gadang akan menjadi standar baru dalam industri game. " +
                          "\nGame ini dirancang khusus untuk memanfaatkan teknologi revolusioner Visionary.", 4000);

        DisplayParagraph("\n\nHari ini, 31 Februari 3024, adalah hari yang ditunggu-tunggu. Beta test Elder Tale dimulai, hanya untuk 100 orang terpilih dari ribuan pendaftar. " +
                          "\nSalah satu dari mereka adalah seorang pemuda bernama Nact.", 4000);

        DisplayParagraph("\n\"Beta test dimulai,\" gumam Nact, tangannya menggenggam perangkat Visionary yang baru saja tiba di rumahnya. " +
                          "\nPerangkat itu tampak elegan, dengan desain futuristik yang memancarkan cahaya lembut. Jantungnya berdegup kencang.", 4000);

        DisplayParagraph("\n\nIa mengenakan perangkat tersebut dan merebahkan diri. Dengan napas dalam, ia menekan tombol Start. " +
                          "\nSeketika, dunia di sekitarnya memudar. Tubuhnya terasa ringan, seperti melayang di ruang kosong. " +
                          "\nSebuah logo muncul, memancarkan cahaya biru yang memukau: EXVision - Express Your Vision.", 4000);

        DisplayParagraph("\n\nSebuah suara lembut namun penuh wibawa terdengar. \"Selamat datang, Beta Tester. Bersiaplah untuk menjelajahi dunia Elder Tale.\"", 4000);

        DisplayParagraph("\n\nCahaya biru berubah menjadi hamparan dunia baru. Langit terbentang luas dengan warna yang lebih cerah dari dunia nyata. " +
                          "\nGunung, hutan, dan kota-kota megah terhampar sejauh mata memandang. Udara terasa nyata, begitu segar hingga Nact hampir lupa bahwa tubuhnya sebenarnya hanya berbaring di kamar.", 4000);

        DisplayParagraph("\n\n\"Ini luar biasa,\" bisiknya, matanya tidak bisa lepas dari keindahan dunia baru ini.", 4000);

        DisplayParagraph("\n\nNamun, di balik kemegahan dunia Elder Tale, sesuatu yang besar sedang menanti—sebuah rahasia yang akan menguji batas keberanian, kecerdasan, dan kemanusiaannya. " +
                          "\nTanpa disadari, Nact baru saja melangkahkan kaki ke dalam petualangan terbesar dalam hidupnya.\n", 4000);

        // Menghentikan musik setelah cerita selesai
        music.StopMusic();

        if (storySkipped)
        {
            Console.WriteLine("\n[Story skipped!]");
        }
    }

    private void PrintWithColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    private void DisplayParagraph(string paragraph, int delay)
    {
        if (!storySkipped)
        {
            Console.WriteLine(paragraph);
            SleepWithSkip(delay);
        }
    }

    private void SleepWithSkip(int milliseconds)
    {
        int elapsed = 0;
        while (elapsed < milliseconds && !storySkipped)
        {
            Thread.Sleep(100);
            elapsed += 100;
        }
    }
}
